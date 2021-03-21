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
        public Employee[] Stuff { get; set; }
    }
}
