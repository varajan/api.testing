using System;
using System.Collections.Generic;
using System.Linq;
using api.testing.DataBase;
using api.testing.Extensions;
using api.testing.Models;
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
        public ActionResult Add(Film film)
        {
            if (Films.Exists(film.Title)) return Conflict($"'{film.Title}' film already exists");
            if (film.Year < 1900) return BadRequest("Invalid year");
            if (film.Year > DateTime.Today.Year) return BadRequest("Invalid year");

            Films.Add(film);

            return Ok("Ok");
        }

        /// <summary>
        /// Delete all films
        /// </summary>
        [HttpDelete]
        [Route("deleteAll")]
        public ActionResult Delete()
        {
            Films.DeleteAll();

            return Ok("Ok");
        }

        /// <summary>
        /// Get all available films
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public ActionResult<IEnumerable<Film>> GetAll() => Ok(Films.Items);

        /// <summary>
        /// Get film by id
        /// </summary>
        /// <response code="404">Film not found</response>
        [HttpGet]
        [Route("get")]
        public ActionResult<Film> Get(int id)
        {
            if (!Films.Exists(id)) return NotFound($"Film '{id}' not found");

            return Ok(Films.Get(id));
        }

        /// <summary>
        /// Find films by title
        /// </summary>
        [HttpGet]
        [Route("find")]
        public ActionResult<IEnumerable<Film>> Find(string title) =>
            Ok(Films.Items.Where(x => x.Title.ContainsIgnoreCase(title)));

        /// <summary>
        /// Assign employee to film
        /// </summary>
        /// <response code="404">Employee not found, Film not found</response>
        /// <response code="409">Assignment already exists</response>
        [HttpPost]
        [Route("assign")]
        public ActionResult Assign(Assignment model)
        {
            if (!Films.Exists(model.FilmId)) return NotFound($"Film with {model.FilmId} not found");
            if (!Films.Exists(model.EmployeeId)) return NotFound($"Employee with {model.EmployeeId} not found");
            if (Assignments.Exists(model)) return Conflict("Assignment already exists");

            return Ok("Ok");
        }

        /// <summary>
        /// Get full films report
        /// </summary>
        /// <response code="400">Invalid filter</response>
        [HttpPost]
        [Route("full")]
        public ActionResult<FilmsFullReport> GetFullReport(FilmsFilter filter)
        {
            if (filter.FromYear.HasValue && filter.ToYear.HasValue && filter.FromYear > filter.ToYear) return BadRequest("FromYear is greater than ToYear");

            var films = Films.Items
                .Where(film => film.Title.ContainsIgnoreCase(filter.Title))
                .Where(film => film.Year > (filter.FromYear ?? 0))
                .Where(film => film.Year < (filter.ToYear ?? 3000))
                .Select(film => new FilmFullData
                {
                    ID = film.ID,
                    Title = film.Title,
                    Year = film.Year,
                    Budget = Assignments.ByFilm(film.ID).Select(x => Employees.Get(x.EmployeeId)).Sum(x => x.Salary),
                    Stuff = Assignments.ByFilm(film.ID).Select(x => Employees.Get(x.EmployeeId)).ToArray()
                }).ToArray();

            var result = new FilmsFullReport
            {
                Count = films.Length,
                Budget = films.Sum(film => film.Budget),
                Data = films
            };

            return Ok(result);
        }
    }
}
