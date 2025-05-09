public class MoviesLogic : IItem
{

    public void AddItem<T>(T item)
    {
        if (item is Movie movie)
        {
            List<Movie> movies = MovieAccess.GetAllMovies();
            movies.Add(movie);
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Movie added successfully.");
        }
        else
        {
            throw new ArgumentException("Item must be of type Movie.");
        }
    }

    public static void EditTitleMovie(string oldTitle, string newTitle)
    {
        Movie[] movies = MovieAccess.GetAllMovies().ToArray();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(oldTitle, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.movieTitle = newTitle;
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Title edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public static void EditYearMovie(string title, int newYear)
    {
        Movie[] movies = MovieAccess.GetAllMovies().ToArray();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.Year = newYear;
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Year edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public static void EditGenreMovie(string title, string newGenre)
    {
        Movie[] movies = MovieAccess.GetAllMovies().ToArray();
        var movieToEdit = movies.FirstOrDefault(m => m.movieTitle.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (movieToEdit != null)
        {
            movieToEdit.Genre = newGenre;
            MovieAccess.WriteMoviesToCSV(movies);
            Console.WriteLine("Genre edited successfully.");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    public bool RemoveItem<T>(T item)
    {
        if (item is Movie movie)
        {
            List<Movie> movies = MovieAccess.GetAllMovies();
            if (movies.Remove(movie))
            {
                MovieAccess.WriteMoviesToCSV(movies);
                Console.WriteLine("Movie successfully removed.");
                return true;
            }
            else
            {
                Console.WriteLine("Movie not found.");
                return false;
            }
        }
        else if (item is string title)
        {
            List<Movie> movies = MovieAccess.GetAllMovies();
            var movieToRemove = movies.FirstOrDefault(m => m.movieTitle.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movieToRemove != null)
            {
                movies.Remove(movieToRemove);
                MovieAccess.WriteMoviesToCSV(movies);
                Console.WriteLine("Movie successfully removed.");
                return true;
            }
            else
            {
                Console.WriteLine("Movie not found.");
                return false;
            }
        }
        else
        {
            throw new ArgumentException("Item must be of type Movie or string.");
        }
    }

    public static void AddTimeAndAuditorium(string movieTitle, DateTime displayDate, string auditorium)
    {
        if (MovieScheduleAccess.HasConflict(movieTitle, displayDate, auditorium))
        {
            Console.WriteLine("Conflict: Another movie screening exists in the same auditorium within 4 hours of this time.");
            return;
        }

        List<Movie> movies = MovieAccess.GetAllMovies();
        var movie = movies.FirstOrDefault(m => m.movieTitle.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
        if (movie != null)
        {
            movie.displayTime = displayDate;
            movie.auditorium = auditorium;
            movie.movieTitle = movieTitle;

            movie.LowPrice = GetValidDecimalInput("Enter low seat price:");
            movie.MediumPrice = GetValidDecimalInput("Enter medium seat price:");
            movie.HighPrice = GetValidDecimalInput("Enter high seat price:");
            movie.HandicapPrice = GetValidDecimalInput("Enter handicap seat price:");

            try
            {
                MovieAccess.WriteMoviesToCSV(movies);
                AuditoriumsDataAccess.CreateLayoutFile(movie.movieTitle, displayDate, auditorium);

                string FileName = $"{movie.movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
                MovieScheduleAccess.WriteToMovieSchedule(FileName, movie.movieTitle, displayDate, auditorium, movie.LowPrice, movie.MediumPrice, movie.HighPrice, movie.HandicapPrice);

                Console.WriteLine("Time, date, auditorium, and seat prices added successfully for the movie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }

    private static decimal GetValidDecimalInput(string prompt)
    {
        decimal value;
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            if (decimal.TryParse(input, out value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            }
        }
    }
}


