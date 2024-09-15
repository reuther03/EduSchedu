using EduSchedu.Modules.Schools.Application.Features.Dtos;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;

namespace EduSchedu.Modules.Schools.Application.Features.Queries;

public record GetUserSchools : IQuery<SchoolDto>
{

}