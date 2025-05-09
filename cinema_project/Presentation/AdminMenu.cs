using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
static class AdminMenu
{
    static public void Start(ref User loggedInUser)
    {
        bool logoutRequested = false;
        Console.Clear();

        while (!logoutRequested)
        {
            Console.WriteLine();
            CenterText.print($"Welcome to the Cinema Application, {loggedInUser.Username}", "Cyan");
            Console.WriteLine();
            CenterText.print(" =================================", "Cyan");
            CenterText.print(" ||                             ||", "Cyan");
            CenterText.print(" || 1. Movie Management         ||", "Cyan");
            CenterText.print(" || 2. User Accounts            ||", "Cyan");
            CenterText.print(" || 3. Rules                    ||", "Cyan");
            CenterText.print(" || 4. Search And Filter Movies ||", "Cyan");
            CenterText.print(" || 5. Edit Catering Menu       ||", "Cyan");
            CenterText.print(" || 6. Reservations             ||", "Cyan");
            CenterText.print(" || 7. Logout                   ||", "Cyan");
            CenterText.print(" ||                             ||", "Cyan");
            CenterText.print(" =================================", "Cyan");
            char input = Console.ReadKey().KeyChar;
            switch (input)
            {
                case '1':
                    Console.Clear();
                    MoviesInterface();
                    break;
                case '2':
                    Console.Clear();
                    Users();
                    break;
                case '3':
                    Console.Clear();
                    Rules();
                    break;
                case '4':
                    Console.Clear();
                    SearchMovies(ref loggedInUser);
                    break;
                case '5':
                    Console.Clear();
                    cateringeditmenu();
                    break;
                case '6':
                    Console.Clear();
                    Reservations();
                    break;
                case '7':
                    Console.Clear();
                    Console.WriteLine("Logging out...");
                    Console.WriteLine("You have been logged out.");
                    Console.WriteLine("Press any key to continue..");
                    Console.ReadKey();
                    Console.Clear();
                    Menu.Start();
                    logoutRequested = true;
                    break; 
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    static private void Reservations()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            CenterText.print(" ==================================", "Cyan");
            CenterText.print(" ||    Reservations Management   ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" || 1. View reservations         ||", "Cyan");
            CenterText.print(" || 2. Edit reservation          ||", "Cyan");
            CenterText.print(" || 3. Remove reservation        ||", "Cyan");
            CenterText.print(" || 4. Update Movie Schedule     ||", "Cyan");
            CenterText.print(" || 5. Back to Main Menu         ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" ==================================", "Cyan");
            char choice = Console.ReadKey().KeyChar;
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    AdminLogic.DisplayAllReservations();
                    break;
                case '2':
                    Console.Clear();
                    AdminLogic.EditReservation();
                    break;
                case '3':
                    Console.Clear();
                    AdminLogic.CancelReservation();
                    break;
                case '4':
                    Console.Clear();
                    MovieScheduleAccess.CheckOutdatingMovieScreenings();
                    break;
                case '5':
                    Console.Clear();
                    exitRequested = true;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private static void SearchMovies(ref User loggedInUser)
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            UserMenu.ViewMovies();
            Console.WriteLine();
            CenterText.print(" ==================================", "Cyan");
            CenterText.print(" ||    Choose search criteria:   ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" || 1. Search by film            ||", "Cyan");
            CenterText.print(" || 2. Search by year            ||", "Cyan");
            CenterText.print(" || 3. Search by genre           ||", "Cyan");
            CenterText.print(" || 4. Back to Main Menu         ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" ==================================", "Cyan");

            char searchOption = Console.ReadKey().KeyChar;

            switch (searchOption)
            {
                case '1':
                    Console.WriteLine();
                    SearchLogic.SearchByFilmForAdmin();
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
                    Console.Clear();
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }


    static private void MoviesInterface()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            Console.WriteLine();
            CenterText.print(" =============================================", "Cyan");
            CenterText.print(" ||         Movie Management                ||", "Cyan");
            CenterText.print(" ||                                         ||", "Cyan");
            CenterText.print(" || 1. View All Movies                      ||", "Cyan");
            CenterText.print(" || 2. Add Movie                            ||", "Cyan");
            CenterText.print(" || 3. Edit Movie                           ||", "Cyan");
            CenterText.print(" || 4. Remove Movie                         ||", "Cyan");
            CenterText.print(" || 5. Add Time and Auditorium to a Movie   ||", "Cyan");
            CenterText.print(" || 6. Back to Main Menu                    ||", "Cyan");
            CenterText.print(" ||                                         ||", "Cyan");
            CenterText.print(" =============================================", "Cyan");

            char input = Console.ReadKey().KeyChar;
            switch (input)
            {
                case '1':
                    Console.Clear();
                    AdminLogic.ViewMovies();
                    Console.WriteLine("\nPress any key to continu....");
                    Console.ReadKey();
                    Console.Clear();
                    MoviesInterface();
                    break;
                case '2':
                    Console.Clear();
                    AdminLogic.AddMovie();
                    break;
                case '3':
                    Console.Clear();
                    EditMovieMenu();
                    break;
                case '4':
                    Console.Clear();
                    AdminLogic.RemoveMovie();
                    break;
                case '5':
                    Console.Clear();
                    AdminLogic.AddTimeAndAuditoriumToMovie();
                    break;
                case '6':
                    Console.Clear();
                    //AdminMenu.Start();
                    exitRequested = true;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }


    static private void Rules()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            CenterText.print(" ==================================", "Cyan");
            CenterText.print(" ||        Rule Management       ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" || 1. View rules                ||", "Cyan");
            CenterText.print(" || 2. Edit rules                ||", "Cyan");
            CenterText.print(" || 3. Add rule                  ||", "Cyan");
            CenterText.print(" || 4. Remove rule               ||", "Cyan");
            CenterText.print(" || 5. Back to Main Menu         ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" ==================================", "Cyan");
            char choice = Console.ReadKey().KeyChar;
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    RulesLogic.ViewAllRules();
                    break;
                case '2':
                    Console.Clear();
                    AdminLogic.EditRules();
                    break;
                case '3':
                    Console.Clear();
                    AdminLogic.AddRule();
                    break;
                case '4':
                    Console.Clear();
                    AdminLogic.RemoveRule();
                    break;
                case '5':
                    Console.Clear();
                    exitRequested = true;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    static private void Users()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            CenterText.print(" ==================================", "Cyan");
            CenterText.print(" ||    User Account Management   ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" || 1. View users                ||", "Cyan");
            CenterText.print(" || 2. Edit user account         ||", "Cyan");
            CenterText.print(" || 3. Remove user               ||", "Cyan");
            CenterText.print(" || 4. Back to Main Menu         ||", "Cyan");
            CenterText.print(" ||                              ||", "Cyan");
            CenterText.print(" ==================================", "Cyan");
            char choice = Console.ReadKey().KeyChar;
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    AdminLogic.ViewAllUsers();
                    break;
                case '2':
                    Console.Clear();
                    AdminLogic.ViewAllUserData();
                    AdminLogic.EditUserAccount();
                    break;
                case '3':
                    Console.Clear();
                    AdminLogic.ViewAllUsers();
                    AdminLogic.RemoveUser();
                    break;
                case '4':
                    Console.Clear();
                    exitRequested = true;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private static void cateringeditmenu()
    {
        CenterText.print(" ===========================================", "Cyan");
        CenterText.print(" ||         CateringMenu Management       ||", "Cyan");
        CenterText.print(" ||                                       ||", "Cyan");
        CenterText.print(" || 1. Add Items                          ||", "Cyan");
        CenterText.print(" || 2. View Items                         ||", "Cyan");
        CenterText.print(" || 3. Remove Items                       ||", "Cyan");
        CenterText.print(" || 4. Sort Items                         ||", "Cyan");
        CenterText.print(" || 5. Edit Items                         ||", "Cyan");
        CenterText.print(" || 6. Back to main menu                  ||", "Cyan");
        CenterText.print(" ||                                       ||", "Cyan");
        CenterText.print(" ===========================================", "Cyan");

        bool exitmenu = false;
        char cateringchoice = Console.ReadKey().KeyChar;

        while (!exitmenu)
            switch (cateringchoice)
            {
                case '1':
                    CateringLogic.AddCateringItem();
                    exitmenu = true;
                    break;
                case '2':
                    CateringLogic.ViewItems("all", CateringAccess.cateringmenu);
                    exitmenu = true;
                    break;
                case '3':
                    CateringLogic.RemoveItem();
                    exitmenu = true;
                    break;
                case '4':
                    CateringLogic.SortItems();
                    exitmenu = true;
                    break;
                case '5':
                    CateringLogic.ViewItems("all", CateringAccess.cateringmenu);
                    Console.WriteLine();
                    Console.WriteLine("Which Item would you like to edit? (by ID)");
                    int idinput = Convert.ToInt32(Console.ReadLine());
                    int? foundId = CateringLogic.IDFound(idinput);
                    if (foundId.HasValue)
                    {
                        Console.WriteLine("Which item would you like to edit?");
                        Console.WriteLine("Name (N)");
                        Console.WriteLine("Size (S)");
                        Console.WriteLine("Price (P)");
                        string cateringchoicen = Console.ReadLine().ToUpper();
                        string arg = "";
                        switch (cateringchoicen)
                        {
                            case "N":
                                arg = "product";
                                Console.WriteLine("What would you like to change the name to?");
                                string newname = Console.ReadLine();
                                CateringLogic.EditItem(idinput, arg, newname);
                                break;
                            case "S":
                                arg = "size";
                                Console.WriteLine("What would you like to change the size to?");
                                string newsize = Console.ReadLine();
                                CateringLogic.EditItem(idinput, arg, newsize);
                                break;
                            case "P":
                                arg = "price";
                                Console.WriteLine("What would you like to change the price to?");
                                double newprice = Convert.ToDouble(Console.ReadLine());
                                CateringLogic.EditItem(idinput, arg, newprice);
                                break;
                            default:
                                Console.WriteLine("Invalid Input");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find the ID, please try again");
                    }
                    exitmenu = true;
                    break;
                case '6':
                    Console.Clear();
                    exitmenu = true;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
    }

    static private void EditMovieMenu()
    {
        AdminLogic.ViewMovies();
        Console.WriteLine();
        Console.WriteLine("Enter the title of the movie you want to edit:\n");
        string titleToEdit = Console.ReadLine();

        CenterText.print(" ===========================================", "Cyan");
        CenterText.print(" ||     What would you like to edit?      ||", "Cyan");
        CenterText.print(" ||                                       ||", "Cyan");
        CenterText.print(" || 1. Movie Title                        ||", "Cyan");
        CenterText.print(" || 2. Movie Year                         ||", "Cyan");
        CenterText.print(" || 3. Movie Genre                        ||", "Cyan");
        CenterText.print(" || 4. Back to movies menu                ||", "Cyan");
        CenterText.print(" ||                                       ||", "Cyan");
        CenterText.print(" ===========================================", "Cyan");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the new title of the movie:");
                    string newTitle = Console.ReadLine();
                    MoviesLogic.EditTitleMovie(titleToEdit, newTitle);
                    break;
                case 2:
                    Console.WriteLine("Enter the new year of release:");
                    if (int.TryParse(Console.ReadLine(), out int newYear))
                    {
                        MoviesLogic.EditYearMovie(titleToEdit, newYear);
                    }
                    else
                    {
                        Console.WriteLine("Invalid year format. Please enter a valid year.");
                    }
                    break;
                case 3:
                    Console.WriteLine("Enter the new genre of the movie:");
                    string newGenre = Console.ReadLine();
                    MoviesLogic.EditGenreMovie(titleToEdit, newGenre);
                    break;
                case 4:
                    MoviesInterface();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}