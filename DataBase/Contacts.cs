using System.Collections.Generic;
using System.Linq;
using api.testing.Models;

namespace api.testing.DataBase
{
    public static class Contacts
    {
        public static string[] Columns = { "Name", "Phone", "Email" };
        private static string columns => string.Join(", ", Columns);
        private static string table = "Contacts";

        static Contacts() =>
            DB.Execute($"CREATE TABLE IF NOT EXISTS {table} ({string.Join(",", Columns.Select(x => $"{x} TEXT"))}); ");

        public static List<Contact> Items =>
                DB
                    .GetRows($"SELECT {columns} FROM {table}")
                    .Select(c => new Contact { Name = c[0], Phone = c[1], Email = c[2] })
                    .ToList();

        public static void DeleteAll() => DB.Execute($"DELETE FROM {table}");

        public static bool Exists(string name) => DB.GetRow($"SELECT Name FROM {table} WHERE Name = '{name}'").Count > 0;

        public static void Add(Contact contact) =>
            DB.Execute($"INSERT INTO {table} ({columns}) VALUES ('{contact.Name.Trim()}', '{contact.Phone.Trim()}', '{contact.Email.Trim()}')");
    }
}
