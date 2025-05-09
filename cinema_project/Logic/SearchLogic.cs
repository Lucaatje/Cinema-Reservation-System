using System.Globalization;

static class SearchLogic
{
    public static void SearchByFilm(ref User loggedInUser)
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        List<Movie> movies = MovieAccess.GetAllMovies();
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        var filteredMovies = movies.Where(movie => movie.movieTitle.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            Console.WriteLine(new string('-', 90));
            Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", "Title", "Year", "Genre");
            Console.WriteLine(new string('-', 90));
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", movie.movieTitle, movie.Year, movie.Genre);
                Console.WriteLine(new string('-', 90));
            }

            Console.WriteLine("\nAvailable screenings for this movie:");
            Console.WriteLine(new string('-', 90));
            Console.WriteLine("{0,-5} | {1,-40} | {2,-20} | {3,-20}", "No.", "Title", "Time", "Auditorium");
            Console.WriteLine(new string('-', 90));
            int ScreeningNumber = 1;
            foreach (var screening in movieSchedule)
            {
                if (screening["movieTitle"] == title)
                {
                    if (DateTime.TryParseExact(screening["displayTime"], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
                    {
                        Console.WriteLine("{0,-5} | {1,-40} | {2,-20} | {3,-20}", ScreeningNumber, screening["movieTitle"], displayTime.ToString("yyyy-MM-dd HH:mm"), screening["auditorium"]);
                        Console.WriteLine(new string('-', 90));
                        ScreeningNumber++;
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing display time for movie: {screening["movieTitle"]}");
                    }
                }
            }
            Console.WriteLine("\nWould you like to make a reservation? (Y/N)");
            string choice = Console.ReadLine();
            if (choice == "Y" || choice == "y")
            {
                Console.Write("\nEnter the screening number you want to reserve: ");
                if (int.TryParse(Console.ReadLine(), out int selectedScreeningNumber))
                {
                    selectedScreeningNumber--;

                    if (selectedScreeningNumber >= 0 && selectedScreeningNumber < ScreeningNumber - 1)
                    {
                        var selectedScreening = movieSchedule
                            .Where(screening => screening["movieTitle"] == title)
                            .ElementAt(selectedScreeningNumber);

                        ReservationLogic.MakeReservationFromSearch(loggedInUser.Username, selectedScreening);
                        ReservationMenu.Start(ref loggedInUser);
                    }
                    else
                    {
                        Console.WriteLine("Invalid screening number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid screening number.");
                }
            }
        }
        else
        {
            Console.WriteLine("No movies found with the given title.");
        }
    }

    public static void SearchByFilmForAdmin()
    {
        Console.WriteLine("Enter the title of the movie:");
        string title = Console.ReadLine();

        List<Movie> movies = MovieAccess.GetAllMovies();
        var movieSchedule = MovieScheduleAccess.GetMovieSchedule();

        var filteredMovies = movies.Where(movie => movie.movieTitle.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredMovies.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            Console.WriteLine(new string('-', 90));
            Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", "Title", "Year", "Genre");
            Console.WriteLine(new string('-', 90));
            foreach (var movie in filteredMovies)
            {
                Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", movie.movieTitle, movie.Year, movie.Genre);
                Console.WriteLine(new string('-', 90));
            }

            Console.WriteLine("\nAvailable screenings for this movie:");
            Console.WriteLine(new string('-', 90));
            Console.WriteLine("{0,-5} | {1,-40} | {2,-20} | {3,-20}", "No.", "Title", "Time", "Auditorium");
            Console.WriteLine(new string('-', 90));
            int ScreeningNumber = 1;
            bool screeningsFound = false;
            foreach (var screening in movieSchedule)
            {
                if (screening["movieTitle"].ToString().Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    screeningsFound = true;
                    if (DateTime.TryParseExact(screening["displayTime"].ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime displayTime))
                    {
                        Console.WriteLine("{0,-5} | {1,-40} | {2,-20} | {3,-20}", ScreeningNumber, screening["movieTitle"], displayTime.ToString("yyyy-MM-dd HH:mm"), screening["auditorium"]);
                        Console.WriteLine(new string('-', 90));
                        ScreeningNumber++;
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing display time for movie: {screening["movieTitle"]}");
                    }
                }
            }
            if (!screeningsFound)
            {
                Console.WriteLine("No screenings found for this movie.");
            }
        }
        else
        {
            Console.WriteLine("No movies found with the given title.");
        }
    }

    public static void SearchByYear()
    {
        Console.WriteLine("Enter the year:");
        if (int.TryParse(Console.ReadLine(), out int year))
        {
            List<Movie> movies = MovieAccess.GetAllMovies();

            var filteredMovies = movies.Where(movie => movie.Year == year).ToList();

            if (filteredMovies.Count > 0)
            {
                Console.WriteLine("\nSearch Results:");
                Console.WriteLine(new string('-', 90));
                Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", "Title", "Year", "Genre");
                Console.WriteLine(new string('-', 90));
                foreach (var movie in filteredMovies)
                {
                    Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", movie.movieTitle, movie.Year, movie.Genre);
                    Console.WriteLine(new string('-', 90));
                }
            }
            else
            {
                Console.WriteLine("No movies found for the given year.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input for year.");
        }
    }

    public static void SearchByGenre()
    {
        List<Movie> movies = MovieAccess.GetAllMovies();
        List<string> uniqueGenres = MovieAccess.GetUniqueGenres(movies);

        if (uniqueGenres.Count > 0)
        {
            Console.WriteLine("\nAvailable Genres:");
            for (int i = 0; i < uniqueGenres.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {uniqueGenres[i]}");
            }

            int genreChoice;
            bool isValidChoice = false;
            do
            {
                Console.WriteLine("Enter the number corresponding to the genre you want to filter by:");
                string choiceInput = Console.ReadLine();
                if (!int.TryParse(choiceInput, out genreChoice) || genreChoice < 1 || genreChoice > uniqueGenres.Count)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
                else
                {
                    isValidChoice = true;
                }
            } while (!isValidChoice);

            string selectedGenre = uniqueGenres[genreChoice - 1];

            var filteredMovies = movies.Where(movie => movie.Genre.Equals(selectedGenre, StringComparison.OrdinalIgnoreCase)).ToList();

            if (filteredMovies.Count > 0)
            {
                Console.WriteLine("\nSearch Results:");
                Console.WriteLine(new string('-', 90));
                Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", "Title", "Year", "Genre");
                Console.WriteLine(new string('-', 90));
                foreach (var movie in filteredMovies)
                {
                    Console.WriteLine("{0,-40} | {1,-10} | {2,-20}", movie.movieTitle, movie.Year, movie.Genre);
                    Console.WriteLine(new string('-', 90));
                }
            }
            else
            {
                Console.WriteLine("No movies found for the selected genre.");
            }
        }
        else
        {
            Console.WriteLine("No genres found in the movie list.");
        }
    }
}
