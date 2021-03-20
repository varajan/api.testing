using System.Collections.Generic;
using api.testing.DataBase;
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
            Contacts.Add(contact);

            return Ok("Ok");
        }
    }
}
