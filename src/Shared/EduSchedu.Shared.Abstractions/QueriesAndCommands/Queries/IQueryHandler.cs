using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using MediatR;

namespace EduSchedu.Shared.Abstractions.QueriesAndCommands.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;