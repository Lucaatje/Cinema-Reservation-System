public class Seat
{
    public string seat { get; set; }
    public string reserved { get; set; }
    public string PriceRange { get; set; }
}

public class Auditorium
{
    public string name { get; set; }
    public Seat[][] layout { get; set; }
}

public class CinemaHalls
{
    public Auditorium[] auditoriums { get; set; }
}

public class SeatPrices
{
    public decimal Low { get; set; }
    public decimal Medium { get; set; }
    public decimal High { get; set; }

    public SeatPrices(decimal low, decimal medium, decimal high)
    {
        Low = low;
        Medium = medium;
        High = high;
    }
}