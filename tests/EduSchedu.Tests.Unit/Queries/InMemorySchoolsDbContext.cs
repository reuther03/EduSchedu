using EduSchedu.Modules.Schools.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Tests.Unit.Commands.Queries;

public class InMemorySchoolsDbContext
{
    internal static SchoolsDbContext Create()
    {
        var options = new DbContextOptionsBuilder<SchoolsDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemorySchoolsDb")
            .Options;

        var context = new SchoolsDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }

    private void Seed(SchoolsDbContext context)
    {
        // Seed data
    }
}