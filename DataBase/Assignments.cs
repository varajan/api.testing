using System.Collections.Generic;
using System.Linq;
using api.testing.Extensions;
using api.testing.Models;

namespace api.testing.DataBase
{
    public class Assignments
    {
        private const string Columns = "Film, Employee";
        private const string Table = "Films";

        static Assignments() => DB.Execute($"CREATE TABLE IF NOT EXISTS {Table} (Film Number, Employee Number); ");

        public static void DeleteAll() => DB.Execute($"DELETE FROM {Table}");
        public static void Delete(Assignment assignment) =>
            DB.Execute($"DELETE FROM {Table} WHERE Film = {assignment.FilmId} AND Employee = {assignment.EmployeeId}");

        public static void Add(Assignment assignment) =>
            DB.Execute($"INSERT INTO {Table} ({Columns}) VALUES ({assignment.FilmId}, {assignment.EmployeeId})");

        public static bool Exists(Assignment assignment) =>
            Items.Any(x => x.FilmId == assignment.FilmId && x.EmployeeId == assignment.EmployeeId);

        public static List<Assignment> ByFilm(int id) => Items.Where(x => x.FilmId == id).ToList();

        public static List<Assignment> Items =>
                DB
                    .GetRows($"SELECT {Columns} FROM {Table}")
                    .Select(x => new Assignment { FilmId = x[0].ToInt(), EmployeeId = x[1].ToInt() }).ToList();
    }
}
