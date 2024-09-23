using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AssignTeacherToLessonCommand(
    [property: JsonIgnore]
    Guid SchoolId) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AssignTeacherToLessonCommand, Guid>
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

        public async Task<Result<Guid>> Handle(AssignTeacherToLessonCommand request, CancellationToken cancellationToken)
        {
            var headmaster = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(headmaster);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (school.HeadmasterId != headmaster.Id)
                return Result.BadRequest<Guid>("You are not the headmaster of this school");

            var teachers = await _schoolUserRepository.GetTeachersByIdsAsync(school.TeacherIds, cancellationToken);

            foreach (var @class in school.Classes)
            {
                var teachersByLanguage = teachers.Where(x => x.LanguageProficiencyIds.All(y => y.Value == @class.LanguageProficiency!.Id)).ToList();

                foreach (var lesson in @class.Lessons)
                {
                    var lessonTimes = @class.Lessons.Select(x => new
                    {
                        x.Day,
                        x.StartTime,
                        x.EndTime
                    }).ToList();

                    var availableTeachersByScheduleItemsForAll = teachersByLanguage
                        .Where(teacher => lessonTimes
                            .TrueForAll(lessonTime => !teacher.Schedule.ScheduleItems
                                .Any(scheduleItem =>
                                    scheduleItem.Day == lessonTime.Day &&
                                    scheduleItem.Start <= lessonTime.EndTime &&
                                    scheduleItem.End >= lessonTime.StartTime))
                        ).ToList();

                    if (availableTeachersByScheduleItemsForAll.Count != 0 && lesson.AssignedTeacher == null)
                    {
                        var assignedTeacher = availableTeachersByScheduleItemsForAll.FirstOrDefault();
                        if (assignedTeacher == null)
                            return Result.BadRequest<Guid>("There is no teacher for all lessons");

                        foreach (var lessonToAssign in @class.Lessons)
                        {
                            lessonToAssign.AssignTeacher(assignedTeacher.Id);
                            assignedTeacher.Schedule.AddScheduleItem(
                                ScheduleItem.CreateLessonItem(lessonToAssign.Day, lessonToAssign.StartTime, lessonToAssign.EndTime)
                            );
                        }
                    }
                    else
                    {
                        if (lesson.AssignedTeacher != null)
                            continue;
                        // second approach
                        // var availableTeachersByScheduleItems = teachersByLanguage.Where(teacher =>
                        //     !teacher.Schedule.ScheduleItems.Any(scheduleItem =>
                        //         scheduleItem.Day == lesson.Day &&
                        //         (
                        //             (lesson.StartTime >= scheduleItem.Start && lesson.StartTime < scheduleItem.End) ||
                        //             (lesson.EndTime > scheduleItem.Start && lesson.EndTime <= scheduleItem.End) ||
                        //             (scheduleItem.Start >= lesson.StartTime && scheduleItem.Start < lesson.EndTime) ||
                        //             (scheduleItem.End > lesson.StartTime && scheduleItem.End <= lesson.EndTime)
                        //         )
                        //     )
                        // ).ToList();
                        var availableTeachersByScheduleItems = teachersByLanguage
                            .Where(teacher => !teacher.Schedule.ScheduleItems
                                .Any(scheduleItem =>
                                    scheduleItem.Day == lesson.Day &&
                                    scheduleItem.Start <= lesson.EndTime &&
                                    scheduleItem.End >= lesson.StartTime)
                            ).ToList();
                        NullValidator.ValidateNotNull(availableTeachersByScheduleItems);

                        var notAssignedTeacher = availableTeachersByScheduleItems.FirstOrDefault();
                        NullValidator.ValidateNotNull(notAssignedTeacher);

                        lesson.AssignTeacher(notAssignedTeacher.Id);
                        notAssignedTeacher.Schedule.AddScheduleItem(
                            ScheduleItem.CreateLessonItem(lesson.Day, lesson.StartTime, lesson.EndTime)
                        );
                    }
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(school.Id.Value);
        }
    }
}