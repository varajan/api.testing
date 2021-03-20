namespace api.testing.Models
{
    public class ContactEdit
    {
        public string Name { get; set; }
        public string NewName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Contact Contact => new Contact {Email = Email, Name = NewName, Phone = Phone};
    }
}
