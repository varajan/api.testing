using System.Collections.Generic;
using System.Linq;
using api.testing.Models;

namespace api.testing.DataBase
{
    public static class Contacts
    {
        private const string Columns = "Name, Phone, Email";
        private const string Table = "Contacts";

        static Contacts() =>
            DB.Execute($"CREATE TABLE IF NOT EXISTS {Table} (Name Text, Phone Text, Email Text); ");

        public static List<Contact> Items =>
                DB
                    .GetRows($"SELECT {Columns} FROM {Table}")
                    .Select(c => new Contact { Name = c[0], Phone = c[1], Email = c[2] })
                    .ToList();

        public static void DeleteAll() => DB.Execute($"DELETE FROM {Table}");

        public static void Delete(string name) => DB.Execute($"DELETE FROM {Table} WHERE Name = '{name.Trim()}'");

        public static bool Exists(string name) => DB.GetRow($"SELECT Name FROM {Table} WHERE Name = '{name.Trim()}'").Count > 0;

        public static void Add(Contact contact) =>
            DB.Execute($"INSERT INTO {Table} ({Columns}) VALUES ('{contact.Name.Trim()}', '{contact.Phone.Trim()}', '{contact.Email.Trim()}')");
    }
}
