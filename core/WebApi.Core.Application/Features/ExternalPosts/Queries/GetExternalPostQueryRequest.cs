using FluentValidation;
using MediatR;
using WebApi.Core.Application.Abstractions;
using WebApi.Core.Application.Abstractions.Models;
using WebApi.Infrastructure.Shared.Caching;
using WebApi.Infrastructure.Shared.Constants;
using WebApi.Infrastructure.Shared.Models;

namespace WebApi.Core.Application.Features.ExternalPosts.Queries;

public class GetExternalPostQueryRequest : IRequest<Response<ExternalPostDto>>
{
    public int Id { get; set; }

    public class Validator : AbstractValidator<GetExternalPostQueryRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
    public class Handler : IRequestHandler<GetExternalPostQueryRequest, Response<ExternalPostDto>>
    {
        private readonly IExternalPostService service;
        private readonly ICacheService cache;

        public Handler(IExternalPostService service, ICacheService cache)
        {
            this.service = service;
            this.cache = cache;
        }
        public async Task<Response<ExternalPostDto>> Handle(
            GetExternalPostQueryRequest request, CancellationToken cancellationToken)
        {
            var post = await cache.GetOrCreateAsync(
                $"external:post:{request.Id}",
                () => service.GetPostByIdAsync(request.Id, cancellationToken),
                TimeSpan.FromMinutes(5));

            return post is null
                ? Response<ExternalPostDto>.Fail(AppMessageConstant.NotFound, 404)
                : Response<ExternalPostDto>.Success(post);
        }
    }
}
