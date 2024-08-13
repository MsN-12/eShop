using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.APi.Controllers;
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    
}