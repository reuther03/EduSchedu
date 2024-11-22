using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Modules.Schools.Domain.Schools.Enums;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Extensions;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Features.Queries.School.Class;

public record GetClassesBySearchValuesQuery : IQuery<PaginatedList<ClassDto>>
{
    private int Page { get; init; } = 1;
    private int PageSize { get; init; } = 10;
    public string? Name { get; init; }
    public Language? Language { get; init; }
    public Lvl? Lvl { get; init; }
    public DayOfWeek? Day { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }

    [property: JsonIgnore]
    public Guid SchoolId { get; init; }

    internal sealed class Handler : IQueryHandler<GetClassesBySearchValuesQuery, PaginatedList<ClassDto>>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<ClassDto>>> Handle(GetClassesBySearchValuesQuery request, CancellationToken cancellationToken)
        {
            //todo: pomyslec nad lekcjami w jakis lepszy sposob, sprobowac naprawic test, dodac brakujaca walidacje
            //todo: pomyslec nad przeniesieniem lekcji do osobnego query lub cos z getem na cala klase z detalami

            var user = await _context.SchoolUsers.FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var classes = await _context.Schools
                .Where(x => x.Id == Shared.Abstractions.Kernel.ValueObjects.SchoolId.From(request.SchoolId))
                .SelectMany(x => x.Classes)
                .Include(x => x.LanguageProficiency)
                .AsSplitQuery()
                .WhereIf(
                    !string.IsNullOrWhiteSpace(request.Name), x => EF.Functions.Like(x.Name, $"%{request.Name}%"))
                .WhereIf(request.Language.HasValue, x => x.LanguageProficiency!.Language == request.Language)
                .WhereIf(request.Lvl.HasValue, x => x.LanguageProficiency!.Lvl == request.Lvl)
                .Include(x => x.Lessons)
                .WhereIf(request.Day.HasValue, x => x.Lessons.Any(y => y.Day == request.Day))
                .WhereIf(request.StartTime.HasValue, x => x.Lessons.Any(y => y.StartTime == request.StartTime))
                .WhereIf(request.EndTime.HasValue, x => x.Lessons.Any(y => y.EndTime == request.EndTime))
                .Select(x => ClassDto.AsDto(x))
                .ToListAsync(cancellationToken);

            return Result.Ok(new PaginatedList<ClassDto>(request.Page, request.PageSize, classes.Count, classes));
        }
    }
}