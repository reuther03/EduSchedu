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
// public record AssignTeacherToClassLessonsCommand(
//     [property: JsonIgnore]
//     Guid SchoolId,
//     [property: JsonIgnore]
//     Guid ClassId) : ICommand
// {
//     internal sealed class Handler : ICommandHandler<AssignTeacherToClassLessonsCommand>
//     {
//         private readonly ISchoolRepository _schoolRepository;
//         private readonly ISchoolUserRepository _schoolUserRepository;
//         private readonly IUserService _userService;
//         private readonly ISchoolUnitOfWork _unitOfWork;
//
//         public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService, ISchoolUnitOfWork unitOfWork)
//         {
//             _schoolRepository = schoolRepository;
//             _schoolUserRepository = schoolUserRepository;
//             _userService = userService;
//             _unitOfWork = unitOfWork;
//         }
//
//         public async Task<Result> Handle(AssignTeacherToClassLessonsCommand request, CancellationToken cancellationToken)
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
//             if (!school.TeacherIds.Contains(headmaster.Id))
//                 return Result.BadRequest<Guid>("You are not a headmaster of this school");
//
//             var teachers = await _schoolUserRepository.GetTeachersByIdsAsync(school.TeacherIds, cancellationToken);
//             NullValidator.ValidateNotNull(teachers);
//
//             var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
//             NullValidator.ValidateNotNull(@class);
//
//             var filteredTeachers = teachers.Where(x => x.LanguageProficiencyIds.Any(y => y.Value == @class.LanguageProficiency.Id)).ToList();
//
//             var lessons = @class.Lessons.ToList();
//             lessons.RemoveAll(x => x.AssignedTeacher != null);
//
//             var lessonTimes = @class.Lessons.Select(x => new
//             {
//                 x.Day,
//                 x.StartTime,
//                 x.EndTime
//             }).ToList();
//
//             var availableTeachersByScheduleItemsForAll = filteredTeachers
//                 .Where(teacher => lessonTimes
//                     .TrueForAll(lessonTime => !teacher.Schedule.ScheduleItems
//                         .Any(scheduleItem =>
//                             scheduleItem.Day == lessonTime.Day &&
//                             scheduleItem.Start <= lessonTime.EndTime &&
//                             scheduleItem.End >= lessonTime.StartTime))
//                 ).ToList();
//
//             var teacher = availableTeachersByScheduleItemsForAll.FirstOrDefault();
//
//             if (teacher is not null)
//             {
//                 foreach (var lesson in lessons.Where(lesson => lesson.AssignedTeacher == null))
//                 {
//                     if (!@class.Lessons.Contains(lesson))
//                         return Result.BadRequest<Guid>("This lesson is not in this class");
//
//                     lesson.AssignTeacher(teacher.Id);
//                     var scheduleItem = ScheduleItem.CreateLessonItem(lesson.Day, lesson.StartTime, lesson.EndTime);
//                     teacher.Schedule.AddScheduleItem(scheduleItem);
//                 }
//             }
//             else
//             {
//                 foreach (var lesson in lessons)
//                 {
//                     if (lesson.AssignedTeacher != null)
//                         continue;
//
//                     var availableTeachersByScheduleItems = filteredTeachers
//                         .Where(teacher2 => !teacher2.Schedule.ScheduleItems
//                             .Any(scheduleItem =>
//                                 scheduleItem.Day == lesson.Day &&
//                                 scheduleItem.Start <= lesson.EndTime &&
//                                 scheduleItem.End >= lesson.StartTime)
//                         ).ToList();
//                     NullValidator.ValidateNotNull(availableTeachersByScheduleItems);
//
//                     var notAssignedTeacher = availableTeachersByScheduleItems.FirstOrDefault();
//                     lesson.AssignTeacher(notAssignedTeacher!.Id);
//                     notAssignedTeacher.Schedule.AddScheduleItem(
//                         ScheduleItem.CreateLessonItem(lesson.Day, lesson.StartTime, lesson.EndTime)
//                     );
//                 }
//             }
//
//             await _unitOfWork.CommitAsync(cancellationToken);
//             return Result.Ok();
//         }
//     }
// }