using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Enums;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class SchoolModuleSeeder : IModuleSeeder
{
    private readonly SchoolsDbContext _dbContext;
    private readonly ISchoolUnitOfWork _schoolUnitOfWork;

    public SchoolModuleSeeder(SchoolsDbContext dbContext, ISchoolUnitOfWork schoolUnitOfWork)
    {
        _dbContext = dbContext;
        _schoolUnitOfWork = schoolUnitOfWork;
    }

    public async Task SeedAsync(IConfiguration configuration, CancellationToken cancellationToken)
    {
        if (!await _dbContext.LanguageProficiencies.AnyAsync(cancellationToken))
        {
            await _dbContext.LanguageProficiencies.AddRangeAsync(
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.English, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.English, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.English, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.English, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.English, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.English, Lvl.C2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Polish, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Polish, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Polish, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Polish, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Polish, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Polish, Lvl.C2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.German, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.German, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.German, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.German, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.German, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.German, Lvl.C2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.French, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.French, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.French, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.French, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.French, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.French, Lvl.C2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Spanish, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Spanish, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Spanish, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Spanish, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Spanish, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Spanish, Lvl.C2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Italian, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Italian, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Italian, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Italian, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Italian, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Italian, Lvl.C2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Portuguese, Lvl.A1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Portuguese, Lvl.A2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Portuguese, Lvl.B1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Portuguese, Lvl.B2),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Portuguese, Lvl.C1),
                LanguageProficiency.Create(LanguageProficiencyId.New(), Language.Portuguese, Lvl.C2)
            );
        }

        await _schoolUnitOfWork.CommitAsync(cancellationToken);
    }
}