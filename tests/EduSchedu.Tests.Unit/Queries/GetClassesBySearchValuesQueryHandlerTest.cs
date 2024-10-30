using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Features.Queries.School.Class;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Modules.Schools.Infrastructure.Database;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EduSchedu.Tests.Unit.Commands.Queries;

public class GetClassesBySearchValuesQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    [Fact]
    public async Task Should_Return_Classes_By_Search_Values()
    {
        // Arrange
        var dbContext = InMemorySchoolsDbContext.Create();
        var handler = new GetClassesBySearchValuesQuery.Handler(dbContext, new MockUserService());

        var school = School.Create("Test School", new Address("test", "test", "test", "test"), "test", "test@test.com", UserId.New());
        var @class = Class.Create("Test Class");
        var language = LanguageProficiency.Create(Guid.NewGuid(), Language.English, Lvl.B2);
        @class.SetLanguageProficiency(language);
        school.AddClass(@class);
        var lesson = Lesson.Create(DayOfWeek.Monday, new TimeOnly(15, 00, 00), new TimeOnly(17, 00, 00));
        @class.AddLesson(lesson);
        dbContext.Schools.Add(school);
        dbContext.Classes.Add(@class);
        dbContext.Lessons.Add(lesson);

        await dbContext.SaveChangesAsync();

        var result = await handler
    }