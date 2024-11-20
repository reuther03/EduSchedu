using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Integration.Events.Schools;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AssignTeacherToClassLessonsCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId) : ICommand
{
    internal sealed class Handler : ICommandHandler<AssignTeacherToClassLessonsCommand>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly IScheduleService _scheduleService;
        private readonly IPublisher _publisher;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService,
            IScheduleService scheduleService, IPublisher publisher, IUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _scheduleService = scheduleService;
            _publisher = publisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignTeacherToClassLessonsCommand request, CancellationToken cancellationToken)
        {
            var headmaster = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(headmaster);

            if (headmaster.Role != Role.HeadMaster)
                return Result.BadRequest<Guid>("You are not a headmaster");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (!school.TeacherIds.Contains(headmaster.Id))
                return Result.BadRequest<Guid>("You are not a headmaster of this school");

            var teachers = await _schoolUserRepository.GetTeachersByIdsAsync(school.TeacherIds, cancellationToken);
            NullValidator.ValidateNotNull(teachers);

            var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
            NullValidator.ValidateNotNull(@class);

            var filteredTeachers = teachers.Where(x => x.LanguageProficiencyIds.Any(y => y.Value == @class.LanguageProficiency.Id)).ToList();
            NullValidator.ValidateNotNull(filteredTeachers);

            var lessons = @class.Lessons.ToList();
            lessons.RemoveAll(x => x.AssignedTeacher != null);

            var lessonTimes = lessons.Select(x => new ScheduleItemPayload
            {
                Day = x.Day,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            }).ToList();

            var availableTeachersByScheduleItemsForAll =
                await _scheduleService.GetAvailableTeachersByScheduleItemsAsync(lessonTimes, filteredTeachers.Select(x => x.Id).ToList(), cancellationToken);

            NullValidator.ValidateNotNull(availableTeachersByScheduleItemsForAll);
            var teacherId = availableTeachersByScheduleItemsForAll.FirstOrDefault();

            var teacherLessons = new Dictionary<UserId, List<ScheduleItemPayload>>();
            if (teacherId != null)
            {
                foreach (var lesson in lessons)
                {
                    lesson.AssignTeacher(teacherId);

                    if (teacherLessons.TryGetValue(teacherId, out var value))
                        value.Add(new ScheduleItemPayload
                        {
                            Day = lesson.Day,
                            StartTime = lesson.StartTime,
                            EndTime = lesson.EndTime
                        });
                    else
                        teacherLessons.Add(teacherId, [
                            new ScheduleItemPayload
                            {
                                Day = lesson.Day,
                                StartTime = lesson.StartTime,
                                EndTime = lesson.EndTime
                            }
                        ]);
                }
            }
            else
            {
                foreach (var lesson in lessons)
                {
                    var availableTeacherForLesson = filteredTeachers
                        .Find(x => _scheduleService.IsUserAvailableAsync(x.Id, lesson.Day, lesson.StartTime, lesson.EndTime, cancellationToken).Result);

                    if (availableTeacherForLesson == null)
                        continue;

                    NullValidator.ValidateNotNull(availableTeacherForLesson);

                    lesson.AssignTeacher(availableTeacherForLesson.Id);

                    if (teacherLessons.TryGetValue(availableTeacherForLesson.Id, out var value))
                        value.Add(new ScheduleItemPayload
                        {
                            Day = lesson.Day,
                            StartTime = lesson.StartTime,
                            EndTime = lesson.EndTime
                        });
                    else
                        teacherLessons.Add(availableTeacherForLesson.Id, [
                            new ScheduleItemPayload
                            {
                                Day = lesson.Day,
                                StartTime = lesson.StartTime,
                                EndTime = lesson.EndTime
                            }
                        ]);
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            await _publisher.Publish(new TeacherAssignedToClassLessonsIntegrationEvent(teacherLessons), cancellationToken);

            return Result.Ok();
        }
    }
}