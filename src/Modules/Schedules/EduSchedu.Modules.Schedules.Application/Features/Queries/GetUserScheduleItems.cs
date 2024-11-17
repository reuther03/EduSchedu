using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Modules.Schedules.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schedules.Application.Features.Queries;

public record GetUserScheduleItems(int Page = 1, int PageSize = 10) : IQuery<PaginatedList<ScheduleItemDto>>
{
    internal sealed class Handler : IQueryHandler<GetUserScheduleItems, PaginatedList<ScheduleItemDto>>
    {
        private readonly ISchedulesDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchedulesDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<ScheduleItemDto>>> Handle(GetUserScheduleItems request, CancellationToken cancellationToken)
        {
            var scheduleItems = await _context.Schedules
                .Where(x => x.UserId == _userService.UserId)
                .SelectMany(x => x.ScheduleItems)
                .Skip((request.Page - 1) * request.PageSize)
                .ToListAsync(cancellationToken);

            NullValidator.ValidateNotNull(scheduleItems);

            var scheduleItemsDto = scheduleItems.Select(ScheduleItemDto.AsDto).ToList();

            return Result.Ok(new PaginatedList<ScheduleItemDto>(request.Page, request.PageSize, scheduleItems.Count, scheduleItemsDto));
        }
    }
}