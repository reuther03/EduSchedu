using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using SchoolIdEntityId = EduSchedu.Shared.Abstractions.Kernel.ValueObjects.SchoolId;


namespace EduSchedu.Modules.Schools.Application.Features.Queries.School;

public record GetSchoolCommand(
    [property: JsonIgnore]
    Guid SchoolId) : IQuery<SchoolDetailsDto>
{
    internal sealed class Handler : IQueryHandler<GetSchoolCommand, SchoolDetailsDto>
    {
        private readonly ISchoolsDbContext _context;
        private readonly IUserService _userService;

        public Handler(ISchoolsDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Result<SchoolDetailsDto>> Handle(GetSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolId = SchoolIdEntityId.From(request.SchoolId);

            var user = await _context.SchoolUsers.FirstOrDefaultAsync(x => x.Id == _userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var school = await _context.Schools
                .Where(x => x.TeacherIds.Any(y => y.Value == user.Id.Value) && x.Id == schoolId)
                .Select(x => SchoolDetailsDto.AsDto(x))
                .FirstOrDefaultAsync(cancellationToken);

            NullValidator.ValidateNotNull(school);

            return Result.Ok(school);
        }
    }
}