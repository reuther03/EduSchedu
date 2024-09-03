using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class SchoolModuleSeeder : IModuleSeeder
{
    private readonly SchoolsDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public SchoolModuleSeeder(SchoolsDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!_dbContext.LanguageProficiencies.Any())
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
                await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}