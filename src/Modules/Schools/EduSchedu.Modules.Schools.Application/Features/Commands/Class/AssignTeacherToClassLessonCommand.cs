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
// namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;
//
// public record AssignTeacherToClassLessonCommand(
//     [property: JsonIgnore]
//     Guid SchoolId,
//     [property: JsonIgnore]
//     Guid ClassId,
//     [property: JsonIgnore]
//     Guid LessonId,
//     Guid TeacherId) : ICommand<Guid>
// {
//     internal sealed class Handler : ICommandHandler<AssignTeacherToClassLessonCommand, Guid>
//     {
//         private readonly IUserService _userService;
//         private readonly ISchoolRepository _schoolRepository;
//         private readonly ISchoolUserRepository _schoolUserRepository;
//         private readonly ISchoolUnitOfWork _unitOfWork;
//
//         public Handler(IUserService userService, ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, ISchoolUnitOfWork unitOfWork)
//         {
//             _userService = userService;
//             _schoolRepository = schoolRepository;
//             _schoolUserRepository = schoolUserRepository;
//             _unitOfWork = unitOfWork;
//         }
//
//         public async Task<Result<Guid>> Handle(AssignTeacherToClassLessonCommand request, CancellationToken cancellationToken)
//         {
//             var headmaster = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
//             NullValidator.ValidateNotNull(headmaster);
//
//             if (headmaster.Role != Role.HeadMaster)
//                 return Result.BadRequest<Guid>("You are not a headmaster");
//
//             var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
//             NullValidator.ValidateNotNull(school);
//
//             var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.TeacherId, cancellationToken);
//             NullValidator.ValidateNotNull(teacher);
//
//             if (!school.TeacherIds.Contains(teacher.Id))
//                 return Result.BadRequest<Guid>("Teacher is not a teacher of this school");
//
//             var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
//             NullValidator.ValidateNotNull(@class);
//
//             if (teacher.LanguageProficiencyIds.All(x => x.Value != @class.LanguageProficiency.Id))
//                 return Result.BadRequest<Guid>("Teacher is not proficient in this language");
//
//             var lesson = @class.Lessons.FirstOrDefault(x => x.Id == request.LessonId);
//             NullValidator.ValidateNotNull(lesson);
//
//             var isTeacherAvailable = !teacher.Schedule.ScheduleItems
//                 .Any(scheduleItem =>
//                     scheduleItem.Day == lesson.Day &&
//                     scheduleItem.Start <= lesson.StartTime &&
//                     scheduleItem.End >= lesson.EndTime
//                 );
//
//             if (!isTeacherAvailable)
//                 return Result.BadRequest<Guid>("Teacher is not available at this time");
//
//             lesson.AssignTeacher(teacher.Id);
//             teacher.Schedule.AddScheduleItem(ScheduleItem.CreateLessonItem(lesson.Day, lesson.StartTime, lesson.EndTime));
//
//             await _unitOfWork.CommitAsync(cancellationToken);
//
//             return Result.Ok(lesson.Id);
//         }
//     }
// }