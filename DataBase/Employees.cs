using System.Collections.Generic;
using System.Linq;
using api.testing.Extensions;
using api.testing.Models;

namespace api.testing.DataBase
{
    public static class Employees
    {
        private const string Columns = "ID, Name, Role, Salary";
        private const string Table = "Films";

        static Employees() => DB.Execute($"CREATE TABLE IF NOT EXISTS {Table} (ID Number, Name Text, Role Number, Salary Number); ");

        public static void DeleteAll() => DB.Execute($"DELETE FROM {Table}");
        public static void Delete(Employee employee) => DB.Execute($"DELETE FROM {Table} WHERE ID = {employee.ID}");

        public static void Add(Employee employee)
        {
            var id = DB.GetValue($"SELECT Max(ID) FROM {Table}").ToInt() + 1;

            DB.Execute($"INSERT INTO {Table} ({Columns}) VALUES ({id}, '{employee.Name.Trim()}', {(int) employee.Role}, {employee.Salary})");
        }

        public static bool Exists(int id) => DB.GetRows($"SELECT ID FROM {Table} WHERE ID = {id}").Any();
        public static bool Exists(string name) => DB.GetRows($"SELECT ID FROM {Table} WHERE Name = '{name.Trim()}'").Any();


        public static Employee Get(int id) => DB.GetRow($"SELECT {Columns} FROM {Table} WHERE ID = {id}").GetEmployee();

        public static Employee Get(string title) => DB.GetRow($"SELECT {Columns} FROM {Table} WHERE Title = '{title}'").GetEmployee();

        private static Employee GetEmployee(this List<string> employee) => new Employee
        {
            ID = employee[0].ToInt(),
            Name = employee[1],
            Role = employee[2].ParseEnum<Roles>(),
            Salary = employee[3].ToInt()
        };

        public static List<Employee> Items => DB.GetRows($"SELECT {Columns} FROM {Table}").Select(GetEmployee).ToList();
    }
}
