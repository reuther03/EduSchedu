using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddScheduleItemCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid UserId,
    ScheduleItemType Type,
    DayOfWeek Day,
    TimeOnly Start,
    TimeOnly End,
    string Description
) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddScheduleItemCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _schoolUnitOfWork;

        public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService,
            ISchoolUnitOfWork schoolUnitOfWork)
        {
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _schoolUnitOfWork = schoolUnitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddScheduleItemCommand request, CancellationToken cancellationToken)
        {
            var admin = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(admin);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (!school.TeacherIds.Contains(admin.Id))
                Result.BadRequest<Guid>("You are not a admin in this school");

            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            if (!school.TeacherIds.Contains(teacher.Id))
                Result.BadRequest<Guid>("This teacher is not in this school");

            var scheduleItem = ScheduleItem.Create(request.Type, request.Day, request.Start, request.End, request.Description);
            teacher.Schedule.AddScheduleItem(scheduleItem);

            await _schoolUnitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(scheduleItem.Id);
        }
    }
}