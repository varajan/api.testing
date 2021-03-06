using Microsoft.AspNetCore.Mvc;

namespace api.testing.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class WebController : ControllerBase
    {
        [HttpGet]
        public ContentResult Index()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = System.IO.File.ReadAllText("HTML/page.html")
            };
        }
    }
}
