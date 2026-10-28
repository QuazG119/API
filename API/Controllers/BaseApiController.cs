using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Shared.Models;
namespace APITest.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult ToResult<T>(Response<T> response) => StatusCode(response.Code, response);
}
