using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Pagination;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Features.Queries.Users;

public record GetTeacherLessonsQuery(
    [property: JsonIgnore]
    Guid SchoolId,
    int Page = 1,
    int PageSize = 10) : IQuery<PaginatedList<LessonDto>>
{
    internal sealed class Handler : IQueryHandler<GetTeacherLessonsQuery, PaginatedList<LessonDto>>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<PaginatedList<LessonDto>>> Handle(GetTeacherLessonsQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _context.SchoolUsers.FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            if (teacher.Role is not Role.Teacher)
                return Result<PaginatedList<LessonDto>>.BadRequest("You are not allowed to get lessons");

            var school = await _context.Schools.FirstOrDefaultAsync(x => x.Id == Domain.Schools.Ids.SchoolId.From(request.SchoolId), cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (!school.TeacherIds.Contains(teacher.Id))
                return Result<PaginatedList<LessonDto>>.BadRequest("You are not allowed to get lessons");

            var lessons = await _context.Lessons
                .Where(x => x.AssignedTeacher == teacher.Id)
                .OrderBy(x => x.Day)
                .ThenBy(x => x.StartTime)
                .Skip((request.Page - 1) * request.PageSize)
                .ToListAsync(cancellationToken);

            var lessonsDto = lessons.Select(LessonDto.AsDto).ToList();

            return Result.Ok(new PaginatedList<LessonDto>(request.Page, request.PageSize, lessons.Count, lessonsDto));
        }
    }
}