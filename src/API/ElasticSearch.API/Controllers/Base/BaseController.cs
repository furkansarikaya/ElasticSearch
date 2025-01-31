using System.Net;
using ElasticSearch.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public class BaseController:ControllerBase
{
    [NonAction]
    protected static IActionResult CreateActionResult<T>(ResponseDto<T> response)
    {
        return response.Status == HttpStatusCode.NoContent 
            ? new ObjectResult(null) { StatusCode = response.Status.GetHashCode() } 
            : new ObjectResult(response) { StatusCode = response.Status.GetHashCode() };
    }
}