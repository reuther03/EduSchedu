using System.Text.Json.Serialization;
using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schedules.Application.Features.Queries;

public record GetTeachersSchedulesQuery(
    [property: JsonIgnore]
    Guid SchoolIdQuery,
    int Page = 1,
    int PageSize = 10) : IQuery<PaginatedList<ScheduleDto>>
{
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
                .ToListAsync(cancellationToken);
            NullValidator.ValidateNotNull(schedules);

            var schedulesDto = schedules.Select(ScheduleDto.AsDto).ToList();

            return Result.Ok(new PaginatedList<ScheduleDto>(query.Page, query.PageSize, schedulesDto.Count, schedulesDto));
        }
    }
}