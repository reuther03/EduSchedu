using EduSchedu.Shared.Abstractions.Kernel.Database;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Shared.Infrastructure.Postgres;

public class Repository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public Repository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Remove(TEntity entity)
        => _dbContext.Set<TEntity>().Remove(entity);
}