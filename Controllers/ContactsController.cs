using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        [HttpGet]
        [Route("filter")]
        public IEnumerable<Contact> Filter(string filter) =>
            Contacts.Items.Where(i =>
                   i.Name.ContainsIgnoreCase(filter)
                || i.Email.ContainsIgnoreCase(filter)
                || i.Phone.Replace("-", "").Contains(filter.Replace("-", "")));

        [HttpPost]
        [Route("deleteAll")]
        public ActionResult DeleteAll()
        {
            Contacts.DeleteAll();

            return Ok("Ok");
        }

        [HttpGet]
        [Route("get")]
        public ActionResult Get(string name)
        {
            if (!Contacts.Exists(name)) NotFound($"Contact with '{name}' was not found");

            return Ok(Contacts.Items.First(x => x.Name == name.Trim()));
        }

        [HttpPost]
        [Route("edit")]
        public ActionResult Edit(ContactEdit contact)
        {
            if (!Contacts.Exists(contact.Name)) NotFound($"Contact with '{contact.Name}' was not found");
            if (Contacts.Exists(contact.NewName)) Conflict($"Contact with '{contact.NewName}' was found");

            Contacts.Delete(contact.Name);

            return Add(contact.Contact);
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name)) BadRequest("Name is required");
            if (!Contacts.Exists(name)) NotFound($"Contact with '{name}' was not found");

            Contacts.Delete(name);

            return Ok("Ok");
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add(Contact contact)
        {
            if (contact.Name.Trim().Length == 0) return BadRequest("Name cannot be empty");
            if (!contact.Email.IsValidEmail()) return BadRequest("Invalid email");
            if (!Regex.IsMatch(contact.Phone, Settings.PhonePattern)) return BadRequest("Invalid phone number");
            if (Contacts.Exists(contact.Name)) return Conflict($"Contact with '{contact.Name}' already exists");

            Contacts.Add(contact);

            return Ok("Ok");
        }
    }
}
