public class Reservation
{
    public string Username { get; set; }
    public string MovieTitle { get; set; }
    public DateTime Date { get; set; }
    public string Auditorium { get; set; }
    public string SeatNumber { get; set; }

    public Reservation(string username, string movieTitle, DateTime date, string auditorium, string seatNumber)
    {
        Username = username;
        MovieTitle = movieTitle;
        Date = date;
        Auditorium = auditorium;
        SeatNumber = seatNumber;
    }
}
