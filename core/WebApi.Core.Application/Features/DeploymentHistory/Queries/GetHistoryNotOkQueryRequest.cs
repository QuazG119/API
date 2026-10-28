using FluentValidation;
using MediatR;
using WebApi.Core.Application.Abstractions;
using WebApi.Core.Application.Abstractions.Models;
using WebApi.Infrastructure.Shared.Caching;
using WebApi.Infrastructure.Shared.Constants;
using WebApi.Infrastructure.Shared.Models;

namespace WebApi.Core.Application.Features.DeploymentHistory.Queries;

public class GetHistoryNotOkQueryRequest : IRequest<Response<IReadOnlyList<DeploymentHistoryDto>>>
{
    public long SupId { get; set; }

    public class Validator : AbstractValidator<GetHistoryNotOkQueryRequest>
    {
        public Validator()
        {
            RuleFor(x => x.SupId).GreaterThan(0);
        }
    }

    public class Handler : IRequestHandler<GetHistoryNotOkQueryRequest, Response<IReadOnlyList<DeploymentHistoryDto>>>
    {
        private readonly IReturnResultQueryRepository repository;
        private readonly ICacheService cache;

        public Handler(IReturnResultQueryRepository repository, ICacheService cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task<Response<IReadOnlyList<DeploymentHistoryDto>>> Handle(
            GetHistoryNotOkQueryRequest request, CancellationToken cancellationToken)
        {
            var key = $"deployment:history-not-ok:{request.SupId}";
            var data = await cache.GetOrCreateAsync(
                key,
                () => repository.GetHistoryNotOkAsync(request.SupId, cancellationToken),
                TimeSpan.FromMinutes(2));

            if (data is null || data.Count == 0)
                return Response<IReadOnlyList<DeploymentHistoryDto>>.Fail(AppMessageConstant.NotFound, 404);

            return Response<IReadOnlyList<DeploymentHistoryDto>>.Success(data);
        }
    }
}
