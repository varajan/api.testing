using System.Collections.Generic;
using api.testing.DataBase;
using api.testing.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.testing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// Add new employee
        /// </summary>
        /// <response code="409">Already exists</response>
        [HttpPost]
        [Route("add")]
        public ActionResult Add(Employee employee)
        {
            if (Employees.Exists(employee.Name)) return Conflict($"'{employee.Name}' employee already exists");

            Employees.Add(employee);

            return Ok("Ok");
        }

        /// <summary>
        /// Delete all employees
        /// </summary>
        [HttpDelete]
        [Route("deleteAll")]
        public ActionResult Delete()
        {
            Employees.DeleteAll();

            return Ok("Ok");
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <response code="404">Employee not found</response>
        [HttpDelete]
        [Route("delete")]
        public ActionResult Delete(Employee employee)
        {
            if (!Employees.Exists(employee.ID)) return NotFound("Employee not fond");
            if (!Employees.Exists(employee.Name)) return NotFound("Employee not fond");

            Employees.Delete(employee);

            return Ok("Ok");
        }

        /// <summary>
        /// Get all available employees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public ActionResult<IEnumerable<Employee>> GetAll()
        {
            return Ok(Employees.Items);
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <response code="404">Employee not found</response>
        [HttpGet]
        [Route("get")]
        public ActionResult<Employee> Get(int id)
        {
            if (!Employees.Exists(id)) return NotFound($"Employee '{id}' not found");

            return Ok(Employees.Get(id));
        }

        /// <summary>
        /// Find employee by name
        /// </summary>
        /// <response code="404">Employee not found</response>
        [HttpGet]
        [Route("find")]
        public ActionResult<Employee> Find(string name)
        {
            if (!Employees.Exists(name)) return NotFound($"Employee '{name}' not found");

            return Ok(Employees.Get(name));
        }

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

            Assignments.Add(model);

            return Ok("Ok");
        }
    }
}
