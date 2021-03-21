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
        /// <summary>
        /// Retrieve contacts (all, or filtered)
        /// </summary>
        [HttpGet]
        [Route("filter")]
        public IEnumerable<Contact> Filter(string filter) =>
            Contacts.Items.Where(i =>
                   i.Name.ContainsIgnoreCase(filter)
                || i.Email.ContainsIgnoreCase(filter)
                || i.Phone.Replace("-", "").Contains(filter.Replace("-", "")));

        /// <summary>
        /// Delete all contacts
        /// </summary>
        [HttpDelete]
        [Route("deleteAll")]
        public ActionResult DeleteAll()
        {
            Contacts.DeleteAll();

            return Ok("Ok");
        }

        /// <summary>
        /// Retrieve contact by name
        /// </summary>
        /// <response code="404">Contact not found</response>
        [HttpGet]
        [Route("get")]
        public ActionResult Get(string name)
        {
            if (!Contacts.Exists(name)) NotFound($"Contact with '{name}' was not found");

            return Ok(Contacts.Items.First(x => x.Name == name.Trim()));
        }

        /// <summary>
        /// Update contact details
        /// </summary>
        /// <response code="400">Name length is not in range: 3-30 symbols, Invalid email address, Invalid phone number</response>
        /// <response code="404">Contact not found</response>
        /// <response code="409">Contact with new name already exists</response>
        [HttpPut]
        [Route("edit")]
        public ActionResult Edit(ContactEdit contact)
        {
            if (!Contacts.Exists(contact.Name)) NotFound($"Contact with '{contact.Name}' was not found");
            if (Contacts.Exists(contact.NewName)) Conflict($"Contact with '{contact.NewName}' was found");

            Contacts.Delete(contact.Name);

            return Add(new Contact { Email = contact.Email, Name = contact.NewName, Phone = contact.Phone });
        }

        /// <summary>
        /// Delete contact by name
        /// </summary>
        /// <response code="404">Contact not found</response>
        /// <response code="400">Contact name was not provided</response>
        [HttpDelete]
        [Route("delete")]
        public ActionResult Delete([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name)) BadRequest("Name is required");
            if (!Contacts.Exists(name)) NotFound($"Contact with '{name}' was not found");

            Contacts.Delete(name);

            return Ok("Ok");
        }

        /// <summary>
        /// Add contact
        /// </summary>
        /// <response code="400">Name length is not in range: 3-30 symbols, Invalid email address, Invalid phone number</response>
        /// <response code="404">Contact not found</response>
        /// <response code="409">Contact with provided name already exists</response>
        [HttpPost]
        [Route("add")]
        public ActionResult Add(Contact contact)
        {
            if (contact.Name.Trim().Length < 3) return BadRequest("Name should be at least 3 symbols");
            if (contact.Name.Trim().Length > 30) return BadRequest("Name should be maximum 30 symbols");
            if (!contact.Email.IsValidEmail()) return BadRequest("Invalid email");
            if (!Regex.IsMatch(contact.Phone, Settings.PhonePattern)) return BadRequest("Invalid phone number");
            if (Contacts.Exists(contact.Name)) return Conflict($"Contact with '{contact.Name}' already exists");

            Contacts.Add(contact);

            return Ok("Ok");
        }
    }
}
