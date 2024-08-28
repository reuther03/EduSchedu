using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using MediatR;

namespace EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;