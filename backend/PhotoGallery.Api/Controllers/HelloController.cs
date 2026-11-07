using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Api.Contracts;

namespace PhotoGallery.Api.Controllers
{
    [Route("api/sayhello")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> index()
        {
            return Ok(new SayHelloResponse()
            {
                Message = "Hello World!"
            });
        }
    }
}
