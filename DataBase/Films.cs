using System.Collections.Generic;
using System.Linq;
using api.testing.Extensions;
using api.testing.Models;

namespace api.testing.DataBase
{
    public static class Films
    {
        private const string Columns = "ID, Year, Title";
        private const string Table = "Films";

        static Films() => DB.Execute($"CREATE TABLE IF NOT EXISTS {Table} (ID Number, Year Number, Title Text); ");

        public static void DeleteAll() => DB.Execute($"DELETE FROM {Table}");
        public static void Delete(Film film) => DB.Execute($"DELETE FROM {Table} WHERE ID = {film.ID}");

        public static void Add(Film film)
        {
            var id = DB.GetValue($"SELECT Max(ID) FROM {Table}").ToInt() + 1;
            DB.Execute($"INSERT INTO {Table} ({Columns}) VALUES ({id}, {film.Year}, '{film.Title.Trim()}')");
        }

        public static bool Exists(int id) => DB.GetRows($"SELECT ID FROM {Table} WHERE ID = {id}").Any();
        public static bool Exists(string title) => DB.GetRows($"SELECT ID FROM {Table} WHERE Title = '{title.Trim()}'").Any();


        public static Film Get(int id) => DB.GetRow($"SELECT {Columns} FROM {Table} WHERE ID = {id}").GetFilm();

        public static Film Get(string title) => DB.GetRow($"SELECT {Columns} FROM {Table} WHERE Title = '{title}'").GetFilm();

        private static Film GetFilm(this List<string> film) => new Film { ID = film[0].ToInt(), Year = film[1].ToInt(), Title = film[2] };

        public static List<Film> Items => DB.GetRows($"SELECT {Columns} FROM {Table}").Select(GetFilm).ToList();
    }
}
