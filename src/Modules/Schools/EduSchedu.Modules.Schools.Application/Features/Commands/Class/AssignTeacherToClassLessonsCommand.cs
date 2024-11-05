using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AssignTeacherToClassLessonsCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    List<Guid> LessonIds) : ICommand
{
    internal sealed class Handler : ICommandHandler<AssignTeacherToClassLessonsCommand>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService, ISchoolUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignTeacherToClassLessonsCommand request, CancellationToken cancellationToken)
        {
            //todo: dokonczyc testowanie/ dodac jakis schedule item ktory usunie lekcje z listy i nie bedzie mozna ich przypisac
            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            if (teacher.Role != Role.HeadMaster)
                return Result.BadRequest<Guid>("You are not a headmaster");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (!school.TeacherIds.Contains(teacher.Id))
                return Result.BadRequest<Guid>("You are not a teacher of this school");

            var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
            NullValidator.ValidateNotNull(@class);

            if (teacher.LanguageProficiencyIds.All(x => x.Value != @class.LanguageProficiency.Id))
                return Result.BadRequest<Guid>("You are not proficient in the language of this class");

            var lessons = await _schoolRepository.GetLessonsByIdsAsync(request.LessonIds, cancellationToken);

            var lessonTimes = @class.Lessons.Select(x => new
            {
                x.Day,
                x.StartTime,
                x.EndTime
            }).ToList();

            var scheduleItems = teacher.Schedule.ScheduleItems;

            var scheduleTimes = scheduleItems.Select(x => new
            {
                x.Day,
                x.Start,
                x.End
            }).ToList();

            var badLessons = lessons.Where(_ =>
                    lessonTimes.Exists(lessonTime =>
                        scheduleTimes.Exists(scheduleTime =>
                            scheduleTime.Day == lessonTime.Day &&
                            scheduleTime.Start <= lessonTime.EndTime &&
                            scheduleTime.End >= lessonTime.StartTime)))
                .ToList();

            lessons.RemoveAll(x => badLessons.Contains(x));

            foreach (var lesson in lessons)
            {
                if (!@class.Lessons.Contains(lesson))
                    return Result.BadRequest<Guid>("This lesson is not in this class");

                lesson.AssignTeacher(teacher.Id);
                var scheduleItem = ScheduleItem.CreateLessonItem(lesson.Day, lesson.StartTime, lesson.EndTime);
                teacher.Schedule.AddScheduleItem(scheduleItem);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok();
        }
    }
}