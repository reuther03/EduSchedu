using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Features.Queries.School;

public record GetUserSchoolsQuery(int Page = 1, int PageSize = 10) : IQuery<PaginatedList<SchoolDto>>
{
    internal sealed class Handler : IQueryHandler<GetUserSchoolsQuery, PaginatedList<SchoolDto>>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<SchoolDto>>> Handle(GetUserSchoolsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.SchoolUsers.FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var schools = await _context.Schools
                .Include(x => x.Classes)
                .Where(x => x.TeacherIds.Any(y => y.Value == user.Id.Value))
                .ToListAsync(cancellationToken);

            var schoolsDto = schools.Select(SchoolDto.AsDto).ToList();

            return Result.Ok(new PaginatedList<SchoolDto>(request.Page, request.PageSize, schoolsDto.Count, schoolsDto));
        }
    }
}