using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database;

public interface ISchoolsDbContext
{
    DbSet<Class> Classes { get; }
    DbSet<School> Schools { get; }
    DbSet<SchoolUser> SchoolUsers { get; }
    DbSet<Student> Students { get; }
    DbSet<Schedule> Schedules { get; }
    DbSet<Lesson> Lessons { get; }
    DbSet<LanguageProficiency> LanguageProficiencies { get; }
}