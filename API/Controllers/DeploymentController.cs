using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Application.Abstractions.Models;
using WebApi.Core.Application.Features.DeploymentHistory.Queries;
using WebApi.Infrastructure.Shared.Models;

namespace APITest.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DeploymentController : BaseApiController
{
    private readonly IMediator mediator;

    public DeploymentController(IMediator mediator) => this.mediator = mediator;

    [HttpGet("history/{supId:long}")]
    [ProducesResponseType(typeof(Response<IReadOnlyList<DeploymentHistoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHistory([FromRoute] long supId, CancellationToken cancellationToken)
        => ToResult(await mediator.Send(new GetHistoryNotOkQueryRequest { SupId = supId }, cancellationToken));
}
