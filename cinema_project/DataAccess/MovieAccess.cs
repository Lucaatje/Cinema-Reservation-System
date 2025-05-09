using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class MovieAccess
{
    /*private const string MoviesFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";
    private const string CinemaHallsFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json";
    public const string DataSourcesFolder = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources";*/
    private const string MoviesFilePath = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";
    public const string DataSourcesFolder = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources";
    /*private const string MoviesFilePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\movies.csv";
    public const string DataSourcesFolder = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources";*/

    public static List<Movie> GetAllMovies()
    {
        List<Movie> movies = new List<Movie>();
        try
        {
            using (var reader = new StreamReader(MoviesFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    movies.Add(new Movie(values[0], int.Parse(values[1]), values[2]));

                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Movies file not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading movies file: " + ex.Message);
        }

        return movies;
    }

    public static List<string> GetUniqueGenres(List<Movie> movies)
    {
        return movies.Select(m => m.Genre).Distinct().ToList();
    }

    /*public static void WriteMoviesToCSV(List<Movie> movies)
    {
        try
        {
            using (var writer = new StreamWriter(MoviesFilePath, false))
            {
                foreach (var movie in movies)
                {
                    string displayDate = movie.displayTime != default(DateTime) ? movie.displayTime.ToString("yyyy-MM-dd HH:mm") : "";

                    writer.WriteLine($"{movie.movieTitle},{movie.Year},{movie.Genre},{displayDate},{movie.auditorium}");
                }
            }
            //Console.WriteLine("Movies written to file successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing movies to file: " + ex.Message);
        }
    }*/
    public static void WriteItemsToCSV<T>(IEnumerable<T> items, string filePath, Action<StreamWriter, T> writeAction)
    {
        try
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var item in items)
                {
                    writeAction(writer, item);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing items to file: " + ex.Message);
        }
    }

    public static void WriteMoviesToCSV(IEnumerable<Movie> movies)
    {
        WriteItemsToCSV(movies, MoviesFilePath, (writer, movie) =>
        {
            string displayDate = movie.displayTime != default(DateTime) ? movie.displayTime.ToString("yyyy-MM-dd HH:mm") : "";
            writer.WriteLine($"{movie.movieTitle},{movie.Year},{movie.Genre},{displayDate},{movie.auditorium}");
        });
    }

}
