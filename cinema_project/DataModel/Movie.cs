public class Movie
{
    public string filename { get; set; }
    public string movieTitle { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public DateTime displayTime { get; set; }
    public string auditorium { get; set; }
    public decimal LowPrice { get; set; }
    public decimal MediumPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal HandicapPrice { get; set; }
    public Movie()
    {
    }
    public Movie(string title, int year, string genre)
    {
        movieTitle = title;
        Year = year;
        Genre = genre;
    }

    public Movie(string title)
    {
        movieTitle = title;
        Year = 0;
        Genre = ""; 
    }
}
