using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


public static class ReservationLogic
{

    public static void ViewReservationHistory(string username)
    {
        List<Reservation> userReservations = ReservationAccess.LoadReservationHistory(username);
        ReservationHistory.DisplayReservationHistory(username, userReservations);
    }

    public static void PrintMoviesForDay(DateTime date)
    {
        Console.WriteLine($"\nMovies for {date:yyyy-MM-dd}:");
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        int screeningNumber = 1;
        foreach (var movieInfo in movieSchedule)
        {
            if (DateTime.TryParseExact(movieInfo["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
            {
                if (displayTime.Date == date.Date)
                {
                    Console.WriteLine($"{screeningNumber} - {movieInfo["movieTitle"]} at {displayTime:HH:mm} in {movieInfo["auditorium"]}");
                    screeningNumber++;
                }
            }
            else
            {
                Console.WriteLine($"Error parsing display time for movie: {movieInfo["movieTitle"]}");
            }
        }
    }


    private static bool IsSeatAvailable(JObject auditoriumData, string seatNumber)
    {
        var auditorium = auditoriumData["auditoriums"][0];
        foreach (var row in auditorium["layout"])
        {
            foreach (var seat in row)
            {
                if (seat["seat"].ToString() == seatNumber && seat["reserved"].ToString() == "false")
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void MakeReservationFromSearch(string username, Dictionary<string, string> selectedScreening)
    {
        if (selectedScreening != null)
        {
            if (DateTime.TryParseExact(selectedScreening["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
            {
                string auditoriumFileName = selectedScreening["filename"].ToString();
                DisplayAuditoriumFromFile(auditoriumFileName);

                var auditoriumData = AuditoriumsDataAccess.GetAuditoriumData(auditoriumFileName);

                if (auditoriumData != null)
                {
                    bool reservationSuccessful = false;
                    while (!reservationSuccessful)
                    {
                        Console.Write("Enter seat number(s) to reserve (separated by commas): ");
                        string[] seatNumbers = Console.ReadLine().ToUpper().Split(',');

                        bool allSeatsAvailable = true;
                        foreach (string seatNumber in seatNumbers)
                        {
                            if (!IsSeatAvailable(auditoriumData, seatNumber.Trim()))
                            {
                                Console.WriteLine($"Seat {seatNumber} is not available.");
                                allSeatsAvailable = false;
                                break;
                            }
                        }

                        if (allSeatsAvailable)
                        {
                            foreach (string seatNumber in seatNumbers)
                            {
                                AuditoriumsDataAccess.ReserveSeatAndUpdateFile(auditoriumData, seatNumber.Trim(), Path.Combine(AuditoriumsDataAccess.jsonFolderPath, auditoriumFileName));
                                ReservationAccess.SaveReservationToCSV(username, selectedScreening["movieTitle"], selectedDate, selectedScreening["auditorium"].ToString(), seatNumber.Trim());
                            }
                            Console.WriteLine("Reservation successful!");
                            reservationSuccessful = true;
                        }
                        else
                        {
                            Console.WriteLine("Please choose different seat(s).");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error retrieving auditorium data.");
                }
            }
            else
            {
                Console.WriteLine("Error parsing display time for movie.");
            }
        }
        else
        {
            Console.WriteLine("Invalid selected screening.");
        }
    }



        public static void MakeReservation(string username)
        {
            MovieScheduleAccess.PrintMoviesWithAuditoriumAndDates();
            Console.Write("Enter date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime selectedDate))
            {
                Console.WriteLine($"\nMovies for {selectedDate:yyyy-MM-dd}:");
                var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

                int screeningIndex = 1;
                foreach (var movieInfo in movieSchedule)
                {
                    if (DateTime.TryParseExact(movieInfo["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
                    {
                        if (displayTime.Date == selectedDate.Date)
                        {
                            Console.WriteLine($"{screeningIndex} - {movieInfo["movieTitle"]} at {displayTime:HH:mm} in {movieInfo["auditorium"]}");
                            screeningIndex++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing display time for movie: {movieInfo["movieTitle"]}");
                    }
                }

                Console.Write("Enter the screening number you want to reserve: ");
                if (int.TryParse(Console.ReadLine(), out int selectedScreeningIndex))
                {
                    var selectedScreening = movieSchedule
                        .Where(schedule => DateTime.TryParseExact(schedule["displayTime"].ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime) && displayTime.Date == selectedDate.Date)
                        .ElementAtOrDefault(selectedScreeningIndex - 1);

                    if (selectedScreening != null)
                    {
                        string movieTitle = selectedScreening["movieTitle"].ToString();
                        string auditoriumFileName = selectedScreening["filename"].ToString();
                        DateTime displayDateTime = DateTime.ParseExact(selectedScreening["displayTime"].ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                        DisplayAuditoriumFromFile(auditoriumFileName);

                        var auditoriumData = AuditoriumsDataAccess.GetAuditoriumData(auditoriumFileName);

                        if (auditoriumData != null)
                        {
                            bool reservationSuccessful = false;
                            while (!reservationSuccessful)
                            {
                                Console.Write("Enter seat number(s) to reserve (separated by commas): ");
                                string[] seatNumbers = Console.ReadLine().ToUpper().Split(',');

                                bool allSeatsAvailable = true;
                                foreach (string seatNumber in seatNumbers)
                                {
                                    if (!IsSeatAvailable(auditoriumData, seatNumber.Trim()))
                                    {
                                        Console.WriteLine($"Seat {seatNumber} is not available.");
                                        allSeatsAvailable = false;
                                        break;
                                    }
                                }

                                if (allSeatsAvailable)
                                {
                                    foreach (string seatNumber in seatNumbers)
                                    {
                                        AuditoriumsDataAccess.ReserveSeatAndUpdateFile(auditoriumData, seatNumber.Trim(), Path.Combine(AuditoriumsDataAccess.jsonFolderPath, auditoriumFileName));
                                        ReservationAccess.SaveReservationToCSV(username, movieTitle, displayDateTime, selectedScreening["auditorium"].ToString(), seatNumber.Trim());
                                    }
                                    Console.WriteLine("Reservation successful!");
                                    reservationSuccessful = true;
                                }
                                else
                                {
                                    Console.WriteLine("Please choose different seat(s).");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error retrieving auditorium data.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid screening number or no screenings found for the selected date.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid screening number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }







    private static void DisplayAuditoriumFromFile(string fileName)
    {
        AuditoriumsPresentation.DisplayAuditoriumFromFile(Path.Combine(AuditoriumsDataAccess.jsonFolderPath, fileName));
    }

    public static void CancelReservation(string username)
    {
        List<Reservation> userReservations = ReservationAccess.LoadReservationHistory(username);

        if (userReservations.Count > 0)
        {
            Console.WriteLine("Select a reservation to cancel:");
            Console.WriteLine();
            Console.WriteLine("{0,-4} {1,-40} {2,-25} {3,-20} {4,-15}", "No.", "Movie Title", "Date", "Auditorium", "Seat");
            Console.WriteLine(new string('-', 100));
            for (int i = 0; i < userReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4} {userReservations[i].MovieTitle,-40} {userReservations[i].Date,-25} {userReservations[i].Auditorium,-20} {userReservations[i].SeatNumber,-15}");
                Console.WriteLine(new string('-', 100));
            }
            Console.WriteLine();
            Console.Write("Enter the number of the reservation to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= userReservations.Count)
            {
                Reservation reservationToCancel = userReservations[selection - 1];
                ReservationAccess.RemoveReservationFromCSV(username, reservationToCancel);
                AuditoriumsDataAccess.UpdateAuditoriumLayoutFile(reservationToCancel.MovieTitle, reservationToCancel.Date, reservationToCancel.Auditorium, reservationToCancel.SeatNumber, false);

                Console.WriteLine("Reservation canceled successfully.");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }

    public static void EditReservation(string username)
    {
        List<Reservation> userReservations = ReservationAccess.LoadReservationHistory(username);

        if (userReservations.Count > 0)
        {
            Console.WriteLine("Select a reservation to edit:");
            Console.WriteLine();
            Console.WriteLine("{0,-4} {1,-40} {2,-25} {3,-20} {4,-15}", "No.", "Movie Title", "Date", "Auditorium", "Seat");
            Console.WriteLine(new string('-', 100));
            for (int i = 0; i < userReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4} {userReservations[i].MovieTitle,-40} {userReservations[i].Date,-25} {userReservations[i].Auditorium,-20} {userReservations[i].SeatNumber,-15}");
                Console.WriteLine(new string('-', 100));
            }
            Console.WriteLine();
            Console.Write("Enter the number of the reservation to edit: ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= userReservations.Count)
            {
                Reservation reservationToEdit = userReservations[selection - 1];
                ReservationAccess.RemoveReservationFromCSV(username, reservationToEdit);

                ReservationAccess.DisplayAuditoriumForReservationEdit(reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium);

                Console.Write("Enter new seat number: ");
                string newSeatNumber = Console.ReadLine().ToUpper();

                string oldSeatNumber = reservationToEdit.SeatNumber;
                reservationToEdit.SeatNumber = newSeatNumber;

                ReservationAccess.SaveReservationToCSV(username, reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, reservationToEdit.SeatNumber); // Provide the reservation date

                AuditoriumsDataAccess.UpdateAuditoriumLayoutFile(reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, oldSeatNumber, reservationToEdit.SeatNumber, true);

                Console.WriteLine("Reservation edited successfully.");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
        else
        {
            Console.WriteLine("No reservations found for this user.");
        }
    }
}
