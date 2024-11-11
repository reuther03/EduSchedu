using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Features.Queries.Users;

public record GetScheduleItemQuery(
    [property: JsonIgnore]
    Guid ScheduleItemId)
    : IQuery<ScheduleItemDto>
{
    internal sealed class Handler : IQueryHandler<GetScheduleItemQuery, ScheduleItemDto>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<ScheduleItemDto>> Handle(GetScheduleItemQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.SchoolUsers.FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var scheduleItem = await _context.Schedules
                .Where(x => x.SchoolUserId == user.Id)
                .SelectMany(x => x.ScheduleItems)
                .FirstOrDefaultAsync(x => x.Id == request.ScheduleItemId, cancellationToken);

            NullValidator.ValidateNotNull(scheduleItem);

            var scheduleItemDto = ScheduleItemDto.AsDto(scheduleItem);

            return Result.Ok(scheduleItemDto);
        }
    }
}