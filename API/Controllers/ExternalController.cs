using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Application.Abstractions.Models;
using WebApi.Core.Application.Features.ExternalPosts.Queries;
using WebApi.Infrastructure.Shared.Models;

namespace APITest.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ExternalController : BaseApiController
{
    private readonly IMediator mediator;

    public ExternalController(IMediator mediator) => this.mediator = mediator;
    [HttpGet("posts/{id:int}")]

    [ProducesResponseType(typeof(Response<ExternalPostDto>), StatusCodes.Status200OK)]

    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]

    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]

    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetPost([FromRoute] int id, CancellationToken cancellationToken)

        => ToResult(await mediator.Send(new GetExternalPostQueryRequest { Id = id }, cancellationToken));
}
