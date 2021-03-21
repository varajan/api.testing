using System.Collections.Generic;
using System.Linq;
using api.testing.Extensions;
using api.testing.Models;

namespace api.testing.DataBase
{
    public class Films
    {
        public static string[] Columns = { "ID", "Title" };
        private static string columns => string.Join(", ", Columns);
        private static string table = "Films";

        static Films() => DB.Execute($"CREATE TABLE IF NOT EXISTS {table} (ID Number, Title Text); ");

        public static void DeleteAll() => DB.Execute($"DELETE FROM {table}");
        public static void Delete(Film film) => DB.Execute($"DELETE FROM {table} WHERE ID = {film.ID}");

        public static void Add(string title)
        {
            var id = DB.GetValue($"SELECT Max(ID) FROM {table}") + 1;
            DB.Execute($"INSERT INTO {table} (ID, Title) VALUES ({id}, '{title.Trim()}')");
        }

        public static bool Exists(int id) => DB.GetRows($"SELECT ID FROM {table} WHERE ID = {id}").Any();
        public static bool Exists(string title) => DB.GetRows($"SELECT ID FROM {table} WHERE Title = '{title.Trim()}'").Any();


        public static Film Get(int id)
        {
            var film = DB.GetRow($"SELECT {columns} FROM {table} WHERE ID = {id}");

            return new Film { ID = film[0].ToInt(), Title = film[1] };
        }

        public static Film Get(string title)
        {
            var film = DB.GetRow($"SELECT {columns} FROM {table} WHERE Title = '{title}'");

            return new Film { ID = film[0].ToInt(), Title = film[1] };
        }

        public static List<Film> Items =>
                DB
                    .GetRows($"SELECT {columns} FROM {table}")
                    .Select(film => new Film { ID = film[0].ToInt(), Title = film[1] }).ToList();
    }
}
