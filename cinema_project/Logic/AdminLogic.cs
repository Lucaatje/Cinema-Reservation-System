using System.Diagnostics;

public static class AdminLogic
{
    static private MoviesLogic movieManager = new MoviesLogic();
    static private RulesLogic rulesManager = new RulesLogic();

    public static void DisplayAllReservations()
    {
        List<Reservation> userReservations = ReservationAccess.LoadAllReservations();
        ReservationHistory.DisplayReservationHistory("", userReservations);
    }


    public static void RemoveMovie()
    {
        ViewMovies();
        Console.WriteLine();
        Console.WriteLine("Enter the title of the movie you want to remove:");
        string titleToRemove = Console.ReadLine();

        bool removed = movieManager.RemoveItem<string>(titleToRemove);
        if (removed)
        {
            Console.WriteLine($"Movie '{titleToRemove}' removed successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to remove movie '{titleToRemove}'. Movie not found.");
        }
    }

    public static void ViewMovies()
    {
        List<Movie> movies = MovieAccess.GetAllMovies();
        Console.WriteLine();
        Console.WriteLine(new string(' ', 40) + "Available Movies:");
        Console.WriteLine(" " + new string('-', 98)); // Top border

        Console.WriteLine("| {0,-60} | {1,-10} | {2,-20} |", "Title", "Year", "Genre"); // Table headers
        Console.WriteLine("|{0}|{1}|{2}|", new string('-', 62), new string('-', 12), new string('-', 22)); // Header separator
        foreach (var movie in movies)
        {
            Console.WriteLine("| {0,-60} | {1,-10} | {2,-20} |", movie.movieTitle, movie.Year, movie.Genre); // Movie details
        }
        Console.WriteLine(" " + new string('-', 98)); // Bottom border
        Console.WriteLine();
    }




    public static void AddMovie()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the year of release:");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("Invalid input for year. Please enter a valid integer.");
            return;
        }

        Console.WriteLine("Enter the genre of the movie:");
        string genre = Console.ReadLine();

        Movie newMovie = new Movie(title, year, genre);
        movieManager.AddItem(newMovie);
        Console.WriteLine("Movie added successfully.");
    }

    public static void EditRules()
    {
        RulesLogic.ViewAllRules();
        Console.WriteLine();
        Console.WriteLine("Which rule would you like to edit? (Insert rule number)");
        if (!int.TryParse(Console.ReadLine(), out int RuleNumber))
        {
            Console.WriteLine("Invalid input for rule number. Please enter a valid integer.");
            return;
        }
        RulesLogic.EditRules(RuleNumber);
    }

    public static void AddRule()
    {
        Console.WriteLine("Enter new rule:");
        string NewRule = Console.ReadLine();
        rulesManager.AddItem<string>(NewRule);
    }

    public static void RemoveRule()
    {
        RulesLogic.ViewAllRules();
        Console.WriteLine();
        Console.WriteLine("Which rule would you like to remove? (Insert rule number)");

        if (!int.TryParse(Console.ReadLine(), out int RuleNumber))
        {
            Console.WriteLine("Invalid input for rule number. Please enter a valid integer.");
            return;
        }

        List<string> Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);

        if (RuleNumber < 1 || RuleNumber > Rules.Count)
        {
            Console.WriteLine("Invalid rule number. Please enter a number within the range of the rules list.");
            return;
        }

        string RuleToRemove = Rules[RuleNumber - 1];
        RulesLogic rulesManager = new RulesLogic();

        if (rulesManager.RemoveItem(RuleToRemove))
        {
            Console.WriteLine("Rule removed successfully.");
        }
        else
        {
            Console.WriteLine("Failed to remove the rule.");
        }
    }




    public static void ViewAllUsers()
    {
        List<User> users = UserAccess.GetAllUsers();

        Console.WriteLine("\nAll Users:");
        Console.WriteLine();
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("{0,-30} | {1,-15}", "Username", "Role");
        Console.WriteLine(new string('-', 50));

        foreach (var user in users)
        {
            Console.WriteLine("{0,-30} | {1,-15}", user.Username, user.Role);
            Console.WriteLine(new string('-', 50));
        }

        Console.WriteLine();
    }


    public static void AddTimeAndAuditoriumToMovie()
    {
        ViewMovies();
        Console.WriteLine();

        Console.WriteLine("Enter movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter display date (yyyy-MM-dd HH:mm):");
        DateTime displayDate;
        while (true)
        {
            string input = Console.ReadLine();
            if (!DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out displayDate))
            {
                Console.WriteLine("Invalid date format. Please enter in the format: yyyy-MM-dd HH:mm");
                continue;
            }

            if (displayDate <= DateTime.Now)
            {
                Console.WriteLine("You cannot enter a date in the past or today. Please enter a future date.");
                continue;
            }

            break;
        }

        Console.WriteLine("Enter auditorium (input has to be Auditorium 1 or Auditorium 2 or Auditorium 3):");
        string auditorium;
        while (true)
        {
            auditorium = Console.ReadLine();
            if (auditorium == "Auditorium 1" || auditorium == "Auditorium 2" || auditorium == "Auditorium 3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid auditorium. Please enter one of the following: Auditorium 1, Auditorium 2, or Auditorium 3.");
            }
        }

        MoviesLogic.AddTimeAndAuditorium(title, displayDate, auditorium);
    }

    public static void RemoveUser()
    {
        Console.WriteLine("Enter the username of the user you want to remove:");
        string usernameToRemove = Console.ReadLine();

        bool removed = UserAccess.RemoveUser(usernameToRemove);
        if (removed)
        {
            Console.WriteLine($"User '{usernameToRemove}' removed successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to remove user '{usernameToRemove}'. User not found.");
        }
    }

    public static void ViewAllUserData()
    {
        List<UserData> users = UserAccess.GetAllUserData();

        Console.WriteLine("All Users:");
        Console.WriteLine();
        Console.WriteLine(new string('-', 120));
        Console.WriteLine("{0,-20} | {1,-20} | {2,-20} | {3,-30} | {4,-20}", "Username", "Name", "Password", "Email", "Phone Number");
        Console.WriteLine(new string('-', 120));

        foreach (var user in users)
        {
            Console.WriteLine("{0,-20} | {1,-20} | {2,-20} | {3,-30} | {4,-20}", user.UserName, user.Name, user.Password, user.Email, user.PhoneNumber);
        }

        Console.WriteLine(new string('-', 120));
    }

    public static void EditUserAccount()
    {
        Console.WriteLine("Enter the username of the user you want to edit:");
        string currentUsername = Console.ReadLine();

        UserData selectedUser = UserAccess.GetAllUserData().FirstOrDefault(u => u.UserName == currentUsername);

        if (selectedUser == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.WriteLine($"Selected User:");
        Console.WriteLine($"{selectedUser.UserName} | Name: {selectedUser.Name} |  Password: {selectedUser.Password} | Email: {selectedUser.Email} | Phone Number: {selectedUser.PhoneNumber}");
        Console.WriteLine();
        Console.WriteLine("Which field do you want to edit?");
        Console.WriteLine("1. Username");
        Console.WriteLine("2. Password");
        Console.WriteLine("3. Name");
        Console.WriteLine("4. Email");
        Console.WriteLine("5. Phone Number");

        string choice = Console.ReadLine();
        string newValue = string.Empty;

        switch (choice)
        {
            case "1":
                Console.WriteLine("Enter new username:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, newValue, selectedUser.Password, selectedUser.Name, selectedUser.Email, selectedUser.PhoneNumber);
                break;
            case "2":
                Console.WriteLine("Enter new password:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, newValue, selectedUser.Name, selectedUser.Email, selectedUser.PhoneNumber);
                break;
            case "3":
                Console.WriteLine("Enter new name:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, selectedUser.Password, newValue, selectedUser.Email, selectedUser.PhoneNumber);
                break;
            case "4":
                Console.WriteLine("Enter new email:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, selectedUser.Password, selectedUser.Name, newValue, selectedUser.PhoneNumber);
                break;
            case "5":
                Console.WriteLine("Enter new phone number:");
                newValue = Console.ReadLine();
                UserAccess.UpdateUser(currentUsername, selectedUser.UserName, selectedUser.Password, selectedUser.Name, selectedUser.Email, newValue);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Console.WriteLine("User updated successfully.");
    }

    public static void EditReservation()
    {
        List<Reservation> allReservations = ReservationAccess.LoadAllReservations();

        if (allReservations.Count > 0)
        {
            Console.WriteLine("Select a reservation to edit:");
            for (int i = 0; i < allReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allReservations[i].Username} - {allReservations[i].MovieTitle} - {allReservations[i].Date} - {allReservations[i].Auditorium} - {allReservations[i].SeatNumber}");
            }

            Console.Write("Enter the number of the reservation to edit: ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= allReservations.Count)
            {
                Reservation reservationToEdit = allReservations[selection - 1];
                string oldUsername = reservationToEdit.Username;
                ReservationAccess.RemoveReservationFromCSV(oldUsername, reservationToEdit);

                ReservationAccess.DisplayAuditoriumForReservationEdit(reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium);

                Console.Write("Enter new seat number: ");
                string newSeatNumber = Console.ReadLine();

                string oldSeatNumber = reservationToEdit.SeatNumber;
                reservationToEdit.SeatNumber = newSeatNumber;

                ReservationAccess.SaveReservationToCSV(oldUsername, reservationToEdit.MovieTitle, reservationToEdit.Date, reservationToEdit.Auditorium, reservationToEdit.SeatNumber);

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
            Console.WriteLine("No reservations found.");
        }
    }

    public static void CancelReservation()
    {
        List<Reservation> allReservations = ReservationAccess.LoadAllReservations();

        if (allReservations.Count > 0)
        {
            Console.WriteLine("Select a reservation to cancel:");
            for (int i = 0; i < allReservations.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allReservations[i].Username} - {allReservations[i].MovieTitle} - {allReservations[i].Date} - {allReservations[i].Auditorium} - {allReservations[i].SeatNumber}");
            }

            Console.Write("Enter the number of the reservation to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= allReservations.Count)
            {
                Reservation reservationToCancel = allReservations[selection - 1];
                string username = reservationToCancel.Username;
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
            Console.WriteLine("No reservations found.");
        }
    }
}