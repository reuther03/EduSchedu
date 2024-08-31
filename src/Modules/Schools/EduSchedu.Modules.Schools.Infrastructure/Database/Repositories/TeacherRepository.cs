using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly ISchoolsDbContext _dbContext;
    private readonly DbSet<Teacher> _teachers;

    public TeacherRepository(ISchoolsDbContext dbContext)
    {
        _dbContext = dbContext;
        _teachers = dbContext.Teachers;
    }

    public async Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default)
        => await _teachers.AnyAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Teacher teacher, CancellationToken cancellationToken = default)
    {
        await _teachers.AddAsync(teacher, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}