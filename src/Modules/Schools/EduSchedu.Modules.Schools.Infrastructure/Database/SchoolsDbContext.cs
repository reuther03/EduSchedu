﻿using System.Runtime.CompilerServices;
using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("EduSchedu.Tests.Unit")]

namespace EduSchedu.Modules.Schools.Infrastructure.Database;

internal class SchoolsDbContext : DbContext, ISchoolsDbContext
{
    public DbSet<School> Schools => Set<School>();
    public DbSet<SchoolUser> SchoolUsers => Set<SchoolUser>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<LanguageProficiency> LanguageProficiencies => Set<LanguageProficiency>();

    public SchoolsDbContext(DbContextOptions<SchoolsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("schools");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}