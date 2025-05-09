public static class ReservationHistory
{
    public static void DisplayReservationHistory(string username, List<Reservation> reservations)
    {
        if (reservations.Count > 0)
        {
            if (!string.IsNullOrEmpty(username))
            {
                Console.WriteLine($"Reservation History for {username}:");
            }

            Console.WriteLine();
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("{0,-30} {1,-30} {2,-20} {3,-20}", "Movie", "Date", "Auditorium", "Seat");
            Console.WriteLine(new string('-', 100));

            foreach (var reservation in reservations)
            {
                Console.WriteLine("{0,-30} {1,-30} {2,-20} {3,-20}", reservation.MovieTitle, reservation.Date, reservation.Auditorium, reservation.SeatNumber);
                Console.WriteLine(new string('-', 100));
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }
}
