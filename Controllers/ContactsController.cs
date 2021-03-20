using System.Collections.Generic;
using api.testing.DataBase;
using api.testing.Extensions;
using api.testing.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.testing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        [HttpGet]
        [Route("all")]
        public IEnumerable<Contact> Get() => Contacts.Items;

        [HttpPost]
        [Route("deleteAll")]
        public ActionResult DeleteAll()
        {
            Contacts.DeleteAll();

            return Ok("Ok");
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add(Contact contact)
        {
            if (contact.Name.Trim().Length == 0) return BadRequest("Name cannot be empty");
            if (!contact.Email.IsValidEmail()) return BadRequest("Invalid email");
            if (Contacts.Exists(contact.Name)) return Conflict($"Contact with '{contact.Name}' already exists");

            Contacts.Add(contact);

            return Ok("Ok");
        }
    }
}
