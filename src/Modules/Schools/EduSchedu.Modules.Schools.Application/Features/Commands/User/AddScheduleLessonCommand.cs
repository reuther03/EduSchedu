// using System.Text.Json.Serialization;
// using EduSchedu.Modules.Schools.Application.Abstractions;
// using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
// using EduSchedu.Modules.Schools.Domain.Users;
// using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
// using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
// using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
// using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
// using EduSchedu.Shared.Abstractions.Services;
//
// namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;
//
// public record AddScheduleLessonCommand(
//     [property: JsonIgnore]
//     Guid SchoolId,
//     Guid LessonId,
//     [property: JsonIgnore]
//     Guid UserId,
//     Guid ClassId
// ) : ICommand<Guid>
// {
//     internal sealed class Handler : ICommandHandler<AddScheduleLessonCommand, Guid>
//     {
//         private readonly ISchoolRepository _schoolRepository;
//         private readonly ISchoolUserRepository _schoolUserRepository;
//         private readonly IUserService _userService;
//         private readonly ISchoolUnitOfWork _unitOfWork;
//
//
//         public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService, ISchoolUnitOfWork unitOfWork)
//         {
//             _schoolRepository = schoolRepository;
//             _schoolUserRepository = schoolUserRepository;
//             _userService = userService;
//             _unitOfWork = unitOfWork;
//         }
//
//         public async Task<Result<Guid>> Handle(AddScheduleLessonCommand request, CancellationToken cancellationToken)
//         {
//             var admin = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
//             NullValidator.ValidateNotNull(admin);
//
//             var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
//             NullValidator.ValidateNotNull(school);
//
//             if (admin.Role == Role.Teacher && !school.TeacherIds.Contains(admin.Id))
//                 return Result<Guid>.BadRequest("You are not allowed to create class");
//
//             var @class = await _schoolRepository.GetClassByIdAsync(school.Id, request.ClassId, cancellationToken);
//             NullValidator.ValidateNotNull(@class);
//
//             var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.UserId, cancellationToken);
//             NullValidator.ValidateNotNull(teacher);
//
//             if (!school.TeacherIds.Contains(teacher.Id))
//                 return Result<Guid>.BadRequest("Teacher not found in school");
//
//             var lesson = @class.Lessons.FirstOrDefault(x => x.Id == request.LessonId);
//             NullValidator.ValidateNotNull(lesson);
//
//             var scheduleItem = ScheduleItem.CreateLessonItem(lesson.Day, lesson.StartTime, lesson.EndTime);
//
//             //todo: naprawic zebym mogl dodac lekcje do planu nauczyciela bezposrednio przez propke a nie przez repo
//             var schedule = await _schoolUserRepository.GetTeacherScheduleAsync(teacher.Id, cancellationToken);
//             NullValidator.ValidateNotNull(schedule);
//
//             schedule.AddScheduleItem(scheduleItem);
//             await _unitOfWork.CommitAsync(cancellationToken);
//
//             return Result.Ok(lesson.Id);
//         }
//     }
// }