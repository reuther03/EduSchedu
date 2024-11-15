using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddScheduleItemCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid UserId,
    ScheduleItemType Type,
    DayOfWeek Day,
    TimeOnly Start,
    TimeOnly End
) : ICommand
{
    internal sealed class Handler : ICommandHandler<AddScheduleItemCommand>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _schoolUnitOfWork;
        private readonly IPublisher _publisher;

        public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService,
            IUnitOfWork schoolUnitOfWork, IPublisher publisher)
        {
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _schoolUnitOfWork = schoolUnitOfWork;
            _publisher = publisher;
        }

        public async Task<Result> Handle(AddScheduleItemCommand request, CancellationToken cancellationToken)
        {
            //plan: czy na pewno tutaj powinien byc tylko teacher czy tez student?

            var admin = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(admin);

            if (admin.Role != Role.HeadMaster && admin.Role != Role.BackOffice)
                Result.BadRequest("You are not a admin");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (!school.TeacherIds.Contains(admin.Id))
                Result.BadRequest("You are not a admin in this school");

            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            if (!school.TeacherIds.Contains(teacher.Id))
                Result.BadRequest("This teacher is not in this school");

            await _schoolUnitOfWork.CommitAsync(cancellationToken);

            await _publisher.Publish(
                new ScheduleItemAddedIntegrationEvent
                (
                    teacher.Id,
                    request.Type,
                    request.Day,
                    request.Start,
                    request.End
                ), cancellationToken);

            return Result.Ok();
        }
    }
}