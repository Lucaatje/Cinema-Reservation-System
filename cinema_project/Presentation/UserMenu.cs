public static class UserMenu
{
    public static void Start(ref User loggedInUser)
    {
        bool exitRequested = false;
        Console.Clear();

        while (!exitRequested)
        {
            Console.WriteLine();
            CenterText.print($"Welcome to the Cinema Application, {loggedInUser?.Username}", "Cyan");
            CenterText.printart(TextArt.welcomescreen());
            CenterText.print("=================================", "Cyan");
            CenterText.print("||                             ||", "Cyan");
            CenterText.print("|| 1. Change Account Info      ||", "Cyan");
            CenterText.print("|| 2. Delete Account           ||", "Cyan");
            CenterText.print("|| 3. View All Movies          ||", "Cyan");
            CenterText.print("|| 4. Catering Menu Info       ||", "Cyan");
            CenterText.print("|| 5. Display Auditoriums      ||", "Cyan");
            CenterText.print("|| 6. Search And Filter Movies ||", "Cyan");
            CenterText.print("|| 7. Reservation              ||", "Cyan");
            CenterText.print("|| 8. Logout                   ||", "Cyan");
            CenterText.print("||                             ||", "Cyan");
            CenterText.print("=================================", "Cyan");
            char option = Console.ReadKey().KeyChar;

            switch (option)
            {
                case '1':
                    Console.Clear();
                    DisplayCurrentUserInfo(loggedInUser);
                    UserLogic.UpdateAccount(loggedInUser);
                    break;
                case '2':
                    Console.Clear();
                    bool status = UserLogic.DeleteAccount(loggedInUser.Username);
                    if (status)
                    {
                        Console.WriteLine("Account deleted successfully");
                        Menu.Start();
                    }
                    break;
                case '3':
                    Console.Clear();
                    ViewMovies();
                    break;
                case '4':
                    Console.Clear();
                    CateringMenu.StartMenu(ref loggedInUser);
                    break;
                case '5':
                    Console.Clear();
                    AuditoriumsLogic.ShowAllAuditoriums();
                    break;
                case '6':
                    Console.Clear();
                    SearchMovies(ref loggedInUser);
                    break;
                case '7':
                    Console.Clear();
                    ReservationMenu.Start(ref loggedInUser);
                    break;
                case '8':
                    Console.Clear();
                    Logout(ref loggedInUser);
                    exitRequested = true; // Set exitRequested after logout
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select again.");
                    break;
            }
        }
    }



    private static void SearchMovies(ref User loggedInUser)
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            ViewMovies();
            Console.WriteLine();
            CenterText.print(" ===============================================", "Cyan");
            CenterText.print(" ||              Choose search criteria:      ||", "Cyan");
            CenterText.print(" ||                                           ||", "Cyan");
            CenterText.print(" || 1. Search by film                         ||", "Cyan");
            CenterText.print(" || 2. Search by year                         ||", "Cyan");
            CenterText.print(" || 3. Search by genre                        ||", "Cyan");
            CenterText.print(" || 4. Back to menu                           ||", "Cyan");
            CenterText.print(" ||                                           ||", "Cyan");
            CenterText.print(" ===============================================", "Cyan");

            char searchOption = Console.ReadKey().KeyChar;

            switch (searchOption)
            {
                case '1':
                    Console.WriteLine();
                    SearchLogic.SearchByFilm(ref loggedInUser);
                    break;
                case '2':
                    Console.WriteLine();
                    SearchLogic.SearchByYear();
                    break;
                case '3':
                    Console.WriteLine();
                    SearchLogic.SearchByGenre();
                    break;
                case '4':
                    Console.Clear();
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    public static void DisplayCurrentUserInfo(User loggedInUser)
    {
        UserData userData = UserLogic.GetUserData(loggedInUser.Username);

        Console.Clear();
        Console.WriteLine("Your current information:");
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("{0,-20} | {1,-30}", "Field", "Value");
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("{0,-20} | {1,-30}", "Username", userData.UserName);
        Console.WriteLine("{0,-20} | {1,-30}", "Password", userData.Password);
        Console.WriteLine("{0,-20} | {1,-30}", "Name", userData.Name);
        Console.WriteLine("{0,-20} | {1,-30}", "Email", userData.Email);
        Console.WriteLine("{0,-20} | {1,-30}", "Phone Number", userData.PhoneNumber);
        Console.WriteLine(new string('-', 50));
        Console.WriteLine();

        CenterText.print(" ============================================", "Cyan");
        CenterText.print(" ||                                        ||", "Cyan");
        CenterText.print(" ||  Which info would you like to change?  ||", "Cyan");
        CenterText.print(" || 1. Email                               ||", "Cyan");
        CenterText.print(" || 2. Phone                               ||", "Cyan");
        CenterText.print(" || 3. Name                                ||", "Cyan");
        CenterText.print(" || 4. Password                            ||", "Cyan");
        CenterText.print(" ||                                        ||", "Cyan");
        CenterText.print(" ============================================", "Cyan");
    }




    public static void ViewMovies()
    {
        List<Movie> movies = MovieAccess.GetAllMovies();

        Console.WriteLine(new string(' ', 40) +"Available Movies:");
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


    private static void Logout(ref User loggedInUser)
    {
        Console.WriteLine("Logging out...");
        loggedInUser = null;
        Console.WriteLine("You have been logged out.");
    }
}