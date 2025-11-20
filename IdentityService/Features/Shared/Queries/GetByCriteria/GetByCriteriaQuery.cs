using MediatR;
using System.Linq.Expressions;

namespace IdentityService.Features.Shared.Queries.GetByCriteria
{
    public class GetByCriteriaQuery<TRequest,TResponse> : IRequest<Result<TResponse>> where TRequest : class
    {
        public Expression<Func<TRequest, bool>> Criteria { get; set; }
        public Expression<Func<TRequest, TResponse>> Selector { get; set; }

    }
}
