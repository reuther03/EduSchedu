using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;

namespace EduSchedu.Shared.Abstractions.Kernel.Database;

public interface IBaseUnitOfWork
{
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}