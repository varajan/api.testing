using System.Collections.Generic;
using api.testing.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace api.testing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmsController : ControllerBase
    {
        /// <summary>
        /// Add new film
        /// </summary>
        /// <response code="409">Already exists</response>
        [HttpPost]
        [Route("add")]
        public ActionResult Add(string title)
        {
            if (Films.Exists(title)) Conflict($"'{title}' film already exists");

            Films.Add(title);

            return Ok("Ok");
        }

        [HttpGet]
        [Route("getAll")]
        public ActionResult<IEnumerable<Films>> GetAll()
        {
            return Ok(Films.Items);
        }
    }
}
