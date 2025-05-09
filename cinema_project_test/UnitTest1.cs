using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;






[TestClass]
public class UnitTest1
{

    public static (string MovieScheduleFilePath, string MoviesFilePath, string reservationHistoryPath, string cateringMenuPath) FilePathTuple =
    ("C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json",
     "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\movies.csv",
     "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\ReservationHistory.csv",
     "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\cateringmenu.json");

    private const string ConnectionString = "Data Source=ABDULRAHMAN;Initial Catalog=cinema_project;User ID=sa;Password=q1w2e3r4t5;";

    private SqlConnection OpenConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    // Testing DataBase Connection.
    [TestMethod]
    public void TestDataBaseConnection()
    {
        {
            bool connectionEstablished = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    {
                        connectionEstablished = true;
                        Assert.IsTrue(connectionEstablished);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    }

    // Testing Login With Right Credentials.
    [TestMethod]
    public void TestLogin_WithRightCredentials()
    {
        string username = "customer";
        string password = "customer";

        var result = UserAccess.Login(username, password);

        Assert.IsNotNull(result);
        Assert.AreEqual(username, result.Username);
        Assert.AreEqual(password, result.Password);
        Assert.IsTrue(result is Customer);
    }

    // Testing Login with wrong Credentials.
    [TestMethod]
    public void TestLogin_WithWrongCredentials()
    {
        string username = "wrongUsername";
        string password = "wrongPassword";

        var result = UserAccess.Login(username, password);

        Assert.IsNull(result);
    }

    // Testing User Creation And Check If User Added To DB. 
    [TestMethod]
    public void CreateUser_ShouldReturnTrue_WhenCredentialsAreValid()
    {
        string username = "testuser";
        string password = "testpassword";
        string name = "test User";
        string email = "testuser@example.com";
        string phoneNumber = "1234567890";

        bool result = UserAccess.CreateUser(username, password, name, email, phoneNumber);

        Assert.IsTrue(result);

        // Verify the user was actually created in the database.
        using (var connection = OpenConnection())
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT COUNT(*) FROM users WHERE user_name = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                int userCount = (int)command.ExecuteScalar();
                Assert.AreEqual(1, userCount);
            }
        }
    }

    // Testing Delete User Account By Removing  The Created Test User.
    [TestMethod]
    public void DeleteAccount_ShouldReturnTrue_WhenUserExists()
    {
        string username = "testuser";
        string password = "testpassword";
        string name = "test User";
        string email = "testuser@example.com";
        string phoneNumber = "1234567890";

        UserAccess.CreateUser(username, password, name, email, phoneNumber);


        bool result = UserAccess.DeleteAccount(username);

        Assert.IsTrue(result);

        // Verify the user was actually deleted from the database.
        using (var connection = OpenConnection())
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT COUNT(*) FROM users WHERE user_name = @username", connection))
            {
                command.Parameters.AddWithValue("@username", username);
                int userCount = (int)command.ExecuteScalar();
                Assert.AreEqual(0, userCount);
            }
        }
    }

    // Test Change Account Information.
    [TestMethod]
    public void ChangeAccount_ShouldReturnTrue_WhenInformationIsUpdated()
    {
        string username = "testuser";
        string password = "testpassword";
        string name = "test User";
        string email = "testuser@example.com";
        string phoneNumber = "1234567890";

        UserAccess.CreateUser(username, password, name, email, phoneNumber);

        try
        {
            string newEmail = "newemail@example.com";
            bool result = UserAccess.ChangeAccount(newEmail, 1, username); // Choice 1: Update email

            Assert.IsTrue(result);

            // Verify the user's email was updated in the database.
            using (var connection = OpenConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT email FROM users WHERE user_name = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    string updatedEmail = (string)command.ExecuteScalar();
                    Assert.AreEqual(newEmail, updatedEmail);
                }
            }
        }
        finally
        {
            // Ensure the user is deleted from the database.
            using (var connection = OpenConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM users WHERE user_name = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    // Testing Add Movie To Schedule In Json File Methode.
    [TestMethod]
    public void WriteToMovieSchedule_ShouldAddMovieEntry_WhenDataIsValid()
    {
        string filename = "TestMovie.mp4";
        string movieTitle = "Test Movie";
        DateTime displayTime = DateTime.Now;
        string auditorium = "Auditorium 1";
        decimal lowPrice = 5.0m;
        decimal mediumPrice = 7.5m;
        decimal highPrice = 10.0m;
        decimal handiPrice = 4.0m;

        try
        {
            MovieScheduleAccess.WriteToMovieSchedule(filename, movieTitle, displayTime, auditorium, lowPrice, mediumPrice, highPrice, handiPrice);

            string json = File.ReadAllText(FilePathTuple.MovieScheduleFilePath);
            var movieSchedule = JsonConvert.DeserializeObject<List<Movie>>(json);

            Assert.IsNotNull(movieSchedule);
            Assert.IsTrue(movieSchedule.Exists(m => m.movieTitle == movieTitle && m.auditorium == auditorium && m.displayTime == displayTime));

            var addedMovie = movieSchedule.Find(m => m.movieTitle == movieTitle && m.auditorium == auditorium && m.displayTime == displayTime);
            Assert.AreEqual(filename, addedMovie.filename);
            Assert.AreEqual(lowPrice, addedMovie.LowPrice);
            Assert.AreEqual(mediumPrice, addedMovie.MediumPrice);
            Assert.AreEqual(highPrice, addedMovie.HighPrice);
            Assert.AreEqual(handiPrice, addedMovie.HandicapPrice);
        }
        finally
        {
            // Remove the movie entry added by the test.
            var movieSchedule = JsonConvert.DeserializeObject<List<Movie>>(File.ReadAllText(FilePathTuple.MovieScheduleFilePath));

            if (movieSchedule != null)
            {
                movieSchedule.RemoveAll(m => m.movieTitle == movieTitle && m.auditorium == auditorium && m.displayTime == displayTime);

                string updatedJson = JsonConvert.SerializeObject(movieSchedule, Formatting.Indented);
                File.WriteAllText(FilePathTuple.MovieScheduleFilePath, updatedJson);
            }
        }
    }

    // Testing Write Movies To CSV Methode.
    [TestMethod]
    public void WriteMoviesToCSV_ShouldAddMovieEntry_WhenDataIsValid()
    {
        var movies = new List<Movie>
            {
                new Movie("Test Movie")
                {
                    Year = 2024,
                    Genre = "Action",
                    displayTime = DateTime.Now,
                    auditorium = "Auditorium 1"
                }
            };

        var originalLines = File.ReadAllLines(FilePathTuple.MoviesFilePath).ToList();

        try
        {
            MovieAccess.WriteMoviesToCSV(movies);

            string[] lines = File.ReadAllLines(FilePathTuple.MoviesFilePath);
            string lastLine = lines.Last();

            Assert.AreEqual("Test Movie,2024,Action," + movies[0].displayTime.ToString("yyyy-MM-dd HH:mm") + ",Auditorium 1", lastLine);
        }
        finally
        {
            //Restore the original contents excluding the test movie
            File.WriteAllLines(FilePathTuple.MoviesFilePath, originalLines.Where(line => !line.Contains("Test Movie,2024,Action,")));
        }
    }

    // Testing save reservation to csv file.
    [TestMethod]
    public void SaveReservationToCSV_ShouldAppendReservationDetails()
    {
        string username = "testuser";
        string movieTitle = "Test Movie";
        DateTime date = DateTime.Now;
        string auditoriumName = "Auditorium 1";
        string seatNumber = "A1";

        var originalLines = File.Exists(FilePathTuple.reservationHistoryPath) ? File.ReadAllLines(FilePathTuple.reservationHistoryPath).ToList() : new List<string>();

        try
        {
            ReservationAccess.SaveReservationToCSV(username, movieTitle, date, auditoriumName, seatNumber);

            string[] lines = File.ReadAllLines(FilePathTuple.reservationHistoryPath);
            string lastLine = lines.Last();

            string expectedLine = $"{username},{movieTitle},{date:yyyy-MM-dd HH:mm},{auditoriumName},{seatNumber}";
            Assert.AreEqual(expectedLine, lastLine);
        }
        finally
        {
            // Clean up by restoring original content
            File.WriteAllLines(FilePathTuple.reservationHistoryPath, originalLines);
        }
    }

    // Testing remove reservation from csv file.
    [TestMethod]
    public void RemoveReservationFromCSV_ShouldRemoveReservationDetails_WhenReservationExists()
    {
        string username = "testuser";
        string movieTitle = "Test Movie";
        DateTime date = DateTime.Now;
        string auditoriumName = "Auditorium 1";
        string seatNumber = "A1";


        var originalLines = File.Exists(FilePathTuple.reservationHistoryPath) ? File.ReadAllLines(FilePathTuple.reservationHistoryPath).ToList() : new List<string>();

        string reservationDetails = $"{username},{movieTitle},{date:yyyy-MM-dd HH:mm},{auditoriumName},{seatNumber}";

        // Add the reservation to be removed
        File.AppendAllText(FilePathTuple.reservationHistoryPath, reservationDetails + Environment.NewLine);

        // Assuming the Reservation class has a constructor that matches these parameters
        var reservationToRemove = new Reservation(username, movieTitle, date, auditoriumName, seatNumber);

        try
        {
            ReservationAccess.RemoveReservationFromCSV(username, reservationToRemove);

            string[] lines = File.ReadAllLines(FilePathTuple.reservationHistoryPath);
            Assert.IsFalse(lines.Contains(reservationDetails));
        }
        finally
        {
            // Clean up by restoring original content
            File.WriteAllLines(FilePathTuple.reservationHistoryPath, originalLines);
        }
    }

    // Testing save catering menu to json file.
    [TestMethod]
    public void SaveMenuToJson_ShouldAppendMenuToJsonFile_WhenDataIsValid()
    {
        // Step 1: Read and store the original content
        var originalContent = File.Exists(FilePathTuple.cateringMenuPath) ? File.ReadAllText(FilePathTuple.cateringMenuPath) : string.Empty;

        var newMenuItems = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            { "Product", "Ice Cream" },
            { "Category", "Dessert" },
            { "Size", "Medium" },
            { "Price", 3.50 }
        },
        new Dictionary<string, object>
        {
            { "Product", "Nachos" },
            { "Category", "Snack" },
            { "Size", "Large" },
            { "Price", 4.00 }
        }
    };

        try
        {
            // Step 2: Append new items to the existing menu
            var existingMenu = File.Exists(FilePathTuple.cateringMenuPath) ? JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(originalContent) : new List<Dictionary<string, object>>();
            existingMenu.AddRange(newMenuItems);

            CateringAccess.SaveMenuToJson(existingMenu, FilePathTuple.cateringMenuPath);

            // Step 3: Verify the new items have been added correctly
            string updatedContent = File.ReadAllText(FilePathTuple.cateringMenuPath);
            var deserializedMenu = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(updatedContent);

            Assert.IsNotNull(deserializedMenu);
            Assert.IsTrue(deserializedMenu.Count >= newMenuItems.Count);

            foreach (var newItem in newMenuItems)
            {
                Assert.IsTrue(deserializedMenu.Any(item =>
                    item.ContainsKey("Product") && item["Product"].Equals(newItem["Product"]) &&
                    item.ContainsKey("Category") && item["Category"].Equals(newItem["Category"]) &&
                    item.ContainsKey("Size") && item["Size"].Equals(newItem["Size"]) &&
                    item.ContainsKey("Price") && item["Price"].Equals(newItem["Price"])));
            }
        }
        finally
        {
            // Step 4: Restore the original content
            if (!string.IsNullOrEmpty(originalContent))
            {
                File.WriteAllText(FilePathTuple.cateringMenuPath, originalContent);
            }
            else
            {
                File.Delete(FilePathTuple.cateringMenuPath);
            }
        }
    }

    // Testing create auditorium layout file.
    [TestMethod]
    public void TestCreateLayoutFile()
    {
        var movieName = "TestMovie";
        var displayDate = new DateTime(2023, 6, 7);
        var auditoriumName = "Auditorium 1";
        var fileName = Path.Combine(AuditoriumsDataAccess.jsonFolderPath, $"{movieName}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json");
        var cinemaHallsJson = JsonConvert.SerializeObject(new CinemaHalls
        {
            auditoriums = new[]
            {
                    new Auditorium
                    {
                        name = auditoriumName,
                        layout = new Seat[][]
                        {
                            new Seat[]
                            {
                                new Seat { seat = "A1", reserved = "false", PriceRange = "Low" }
                            }
                        }
                    }
                }
        });

        var backupFilePath = AuditoriumsDataAccess.CinemaHallsFilePath + ".bak";
        File.Copy(AuditoriumsDataAccess.CinemaHallsFilePath, backupFilePath, true);

        try
        {
            File.WriteAllText(AuditoriumsDataAccess.CinemaHallsFilePath, cinemaHallsJson);

            AuditoriumsDataAccess.CreateLayoutFile(movieName, displayDate, auditoriumName);

            Assert.IsTrue(File.Exists(fileName));
            var createdFileContent = File.ReadAllText(fileName);
            var createdCinemaHalls = JsonConvert.DeserializeObject<CinemaHalls>(createdFileContent);
            Assert.IsNotNull(createdCinemaHalls);
            Assert.AreEqual(1, createdCinemaHalls.auditoriums.Length);
            Assert.AreEqual(auditoriumName, createdCinemaHalls.auditoriums[0].name);
        }
        finally
        {
            // Clean up
            File.Delete(fileName);
            File.Copy(backupFilePath, AuditoriumsDataAccess.CinemaHallsFilePath, true);
            File.Delete(backupFilePath);
        }
    }

    // Testing remove auditorium layout file.
    [TestMethod]
    public void TestRemoveLayoutFile()
    {
        var movieName = "TestMovie";
        var displayDate = new DateTime(2023, 6, 7);
        var auditoriumName = "Auditorium 1";
        var fileName = Path.Combine(AuditoriumsDataAccess.jsonFolderPath, $"{movieName}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json");

        File.WriteAllText(fileName, "Temporary file content");

        AuditoriumsDataAccess.RemoveLayoutFile(movieName, displayDate, auditoriumName);

        Assert.IsFalse(File.Exists(fileName));
    }
}