using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddScheduleLessonCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    Guid LessonId,
    [property: JsonIgnore]
    Guid UserId,
    Guid ClassId
) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddScheduleLessonCommand, Guid>
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

        public async Task<Result<Guid>> Handle(AddScheduleLessonCommand request, CancellationToken cancellationToken)
        {
            var admin = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            if (admin is null)
                return Result<Guid>.BadRequest("User not found");

            if (admin.Role == Role.Teacher)
                return Result<Guid>.BadRequest("You are not allowed to create class");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            if (school is null)
                return Result<Guid>.BadRequest("School not found");

            var @class = await _schoolRepository.GetClassByIdAsync(school.Id, request.ClassId, cancellationToken);
            if (@class is null)
                return Result<Guid>.BadRequest("Class not found");

            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.UserId, cancellationToken);
            if (teacher is null)
                return Result<Guid>.BadRequest("User not found");

            if (!school.TeacherIds.Contains(teacher.Id))
                return Result<Guid>.BadRequest("Teacher not found in school");

            var lesson = @class.Lessons.FirstOrDefault(x => x.Id == request.LessonId);
            if (lesson is null)
                return Result<Guid>.BadRequest("Lesson not found");

            if (lesson.ScheduleId is not null)
                return Result<Guid>.BadRequest("Lesson already added to schedule");

            //todo: naprawic zebym mogl dodac lekcje do planu nauczyciela bezposrednio przez propke a nie przez repo
            var schedule = await _schoolUserRepository.GetTeacherScheduleAsync(teacher.Id, cancellationToken);
            if (schedule is null)
                return Result<Guid>.BadRequest("Teacher schedule not found");

            schedule.AddLesson(lesson);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(lesson.Id);
        }
    }
}