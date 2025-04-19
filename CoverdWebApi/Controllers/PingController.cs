using Microsoft.AspNetCore.Mvc;

namespace CoverdWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            Console.Out.WriteLine("pong");

            return Ok("pong");
        }
    }
}