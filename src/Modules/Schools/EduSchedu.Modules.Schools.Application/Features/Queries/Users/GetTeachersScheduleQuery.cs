using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Features.Queries.Users;

public class GetTeachersScheduleQuery : IQuery<ScheduleItemDateDto>
{
    internal sealed class Handler : IQueryHandler<GetTeachersScheduleQuery, ScheduleItemDateDto>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<ScheduleItemDateDto>> Handle(GetTeachersScheduleQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _context.SchoolUsers.OfType<Teacher>().FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            var scheduleItems = await _context.Schedules
                .Include(x => x.ScheduleItems)
                .Where(x => x.TeacherId == teacher.Id)
                .SelectMany(x => x.ScheduleItems).ToListAsync(cancellationToken);

            var groupedScheduleItems = scheduleItems.GroupBy(x => x.Day).Select(group => new
            {
                Day = group.Key,
                Items = group.Select(x => new ScheduleItemDateDto
                {
                    Start = x.Start,
                    End = x.End
                }).ToList()
            }).ToList();



            return Result.Ok(groupedScheduleItems);
        }
    }
}