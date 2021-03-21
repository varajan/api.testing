namespace api.testing.Models
{
    public class FilmsFullReport
    {
        public int Count { get; set; }
        public decimal Budget { get; set; }
        public FilmFullData[] Data { get; set; }
    }

    public class FilmFullData : Film
    {
        public decimal Budget { get; set; }
        public Stuff[] Stuff { get; set; }
    }

    public class Stuff
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public decimal Costs { get; set; }
    }
}
