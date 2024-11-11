using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Features.Queries.Users;

public class GetUserScheduleQuery : IQuery<List<ScheduleItemDateDto>>
{
    internal sealed class Handler : IQueryHandler<GetUserScheduleQuery, List<ScheduleItemDateDto>>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<List<ScheduleItemDateDto>>> Handle(GetUserScheduleQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.SchoolUsers.FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var scheduleItems = await _context.Schedules
                .Include(x => x.ScheduleItems)
                .Where(x => x.SchoolUserId == user.Id)
                .SelectMany(x => x.ScheduleItems)
                .OrderBy(x => x.Day)
                .ThenBy(x => x.Start)
                .ToListAsync(cancellationToken);

            var scheduleItemsDto = scheduleItems.Select(ScheduleItemDateDto.AsDto).ToList();

            return Result.Ok(scheduleItemsDto);
        }
    }
}