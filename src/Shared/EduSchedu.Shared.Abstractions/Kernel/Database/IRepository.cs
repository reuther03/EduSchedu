using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Shared.Abstractions.Kernel.Database;

public interface IRepository<in TEntity> where TEntity : class, IEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
}