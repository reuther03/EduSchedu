using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
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
                    var availableTeachersByLessons = teachersByLanguage
                        .Where(x => x.Schedule.Lessons
                            .All(y => y.Day == lesson.Day && y.StartTime <= lesson.EndTime && y.EndTime >= lesson.StartTime))
                        .ToList();

                    var availableTeachersByScheduleItems = availableTeachersByLessons
                        .Where(x => x.Schedule.ScheduleItems
                            .All(y => y.Start.DayOfWeek == lesson.Day && y.Start.TimeOfDay <= lesson.EndTime.ToTimeSpan() &&
                                y.End.TimeOfDay >= lesson.StartTime.ToTimeSpan())).ToList();

                    //powinno byc tak zeby nauczyciel byl przypisany do wszystkich lekcji jesli to mozliwe a jak nie to else uzupelni brakujacego nauczyciela
                    if (availableTeachersByScheduleItems.Count != 0)
                    {
                        var assignedTeacher = availableTeachersByScheduleItems.First();
                        // Assign the teacher to all lessons
                        foreach (var lessonToAssign in @class.Lessons)
                        {
                            lessonToAssign.AssignTeacher(assignedTeacher.Id);
                            assignedTeacher.Schedule.AddLesson(lessonToAssign);
                        }
                    }
                    else
                    {
                        var notAssignedTeacher = availableTeachersByLessons.FirstOrDefault();
                        if (notAssignedTeacher == null)
                            continue;

                        foreach (var lessonToAssign in @class.Lessons.Where(x => x.AssignedTeacher == null))
                        {
                            lessonToAssign.AssignTeacher(notAssignedTeacher.Id);
                            notAssignedTeacher.Schedule.AddLesson(lessonToAssign);
                        }
                    }

                    // if (availableTeachersByScheduleItems.Count == 0)
                    //     continue;

                    // var assignedTeacher = availableTeachersByScheduleItems.FirstOrDefault();
                    // NullValidator.ValidateNotNull(assignedTeacher);
                    //
                    // lesson.AssignTeacher(assignedTeacher.Id);
                    // assignedTeacher.Schedule.AddLesson(lesson);
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(school.Id.Value);
        }
    }
}