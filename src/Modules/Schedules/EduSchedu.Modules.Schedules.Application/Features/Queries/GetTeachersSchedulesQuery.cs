using System.Text.Json.Serialization;
using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Extensions;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schedules.Application.Features.Queries;

public record GetTeachersSchedulesQuery : IQuery<PaginatedList<ScheduleDto>>
{
    [property: JsonIgnore]
    public Guid SchoolIdQuery { get; init; }

    [property: JsonIgnore]
    public int Page { get; init; } = 1;

    [property: JsonIgnore]
    public int PageSize { get; init; } = 10;

    public Guid? UserId { get; init; }
    public ScheduleItemType? ScheduleType { get; init; }
    public DayOfWeek? Day { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }

    internal sealed class Handler : IQueryHandler<GetTeachersSchedulesQuery, PaginatedList<ScheduleDto>>
    {
        private readonly ISchedulesDbContext _dbContext;
        private readonly ISchoolService _schoolService;
        private readonly IUserService _userService;

        public Handler(ISchedulesDbContext dbContext, ISchoolService schoolService, IUserService userService)
        {
            _dbContext = dbContext;
            _schoolService = schoolService;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<ScheduleDto>>> Handle(GetTeachersSchedulesQuery query, CancellationToken cancellationToken)
        {
            if (!await _schoolService.IsHeadmasterAsync(_userService.UserId!, query.SchoolIdQuery, cancellationToken))
                return Result<PaginatedList<ScheduleDto>>.Unauthorized("You are not authorized to perform this action.");

            var schoolUsersId = await _schoolService.GetSchoolTeachersAsync(SchoolId.From(query.SchoolIdQuery), cancellationToken);
            NullValidator.ValidateNotNull(schoolUsersId);

            //var schedules = await _scheduleRepository.GetSchedulesByUserIdsAsync(schoolUsersId, cancellationToken);
            var schedules = await _dbContext.Schedules
                .Include(x => x.ScheduleItems)
                .Where(x => schoolUsersId.Contains(x.UserId))
                .WhereIf(query.UserId.HasValue, x => x.UserId == Shared.Abstractions.Kernel.ValueObjects.UserId.From(query.UserId!.Value))
                .WhereIf(query.ScheduleType.HasValue, x => x.ScheduleItems.Any(y => y.Type == query.ScheduleType))
                .WhereIf(query.Day.HasValue, x => x.ScheduleItems.Any(y => y.Day == query.Day))
                //og: check if this is correct
                .WhereIf(query.StartTime.HasValue, x => x.ScheduleItems.Any(y => y.StartTime >= query.StartTime))
                .WhereIf(query.EndTime.HasValue, x => x.ScheduleItems.Any(y => y.EndTime <= query.EndTime))
                .ToListAsync(cancellationToken);
            NullValidator.ValidateNotNull(schedules);

            var schedulesDto = schedules.Select(ScheduleDto.AsDto).ToList();

            return Result.Ok(new PaginatedList<ScheduleDto>(query.Page, query.PageSize, schedulesDto.Count, schedulesDto));
        }
    }
}