using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationFP2.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            return NotFound();
        }
    }
}