using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class AuditoriumsDataAccess
{
    /*public static string jsonFolderPath = @"C:\Users\abdul\OneDrive\Documents\GitHub\Cinema-Project\cinema_project\DataSources\";
    public const string CinemaHallsFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json";
    private const string MovieScheduleFilePath = "C:\\Users\\abdul\\OneDrive\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";*/
    public static string jsonFolderPath = @"C:\Users\Gebruiker\OneDrive - Hogeschool Rotterdam\Github\Cinema-Project\cinema_project\DataSources";
    public const string CinemaHallsFilePath = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\CinemaHalls.json";
    private const string MovieScheduleFilePath = "C:\\Users\\Joseph\\Documents\\GitHub\\Cinema-Project\\cinema_project\\DataSources\\MovieSchedule.json";

    public static CinemaHalls GetAllAuditoriums()
    {
        string json = File.ReadAllText(CinemaHallsFilePath);
        return JsonConvert.DeserializeObject<CinemaHalls>(json);
    }

    public static JObject GetAuditoriumData(string fileName)
    {
        try
        {
            string jsonContent = File.ReadAllText(Path.Combine(jsonFolderPath, fileName));
            return JsonConvert.DeserializeObject<JObject>(jsonContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading auditorium data: {ex.Message}");
            return null;
        }
    }

    public static void UpdateAuditoriumLayoutFile(string movieTitle, DateTime displayDate, string auditorium, string seatNumber, bool reserved)
    {
        try
        {
            string fileName = $"{movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditorium}.json";
            string filePath = Path.Combine(jsonFolderPath, fileName);

            string jsonContent = File.ReadAllText(filePath);
            var auditoriumData = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var seatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == seatNumber);

            if (seatToUpdate != null)
            {
                seatToUpdate["reserved"] = reserved.ToString().ToLower();
                File.WriteAllText(filePath, JsonConvert.SerializeObject(auditoriumData));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating auditorium layout file: {ex.Message}");
        }
    }

    public static void UpdateAuditoriumLayoutFile(string movieTitle, DateTime displayDate, string auditoriumName, string oldSeatNumber, string newSeatNumber, bool reserved)
    {
        try
        {
            string fileName = $"{movieTitle}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json";
            string filePath = Path.Combine(jsonFolderPath, fileName);

            string jsonContent = File.ReadAllText(filePath);
            var auditoriumData = JsonConvert.DeserializeObject<JObject>(jsonContent);

            var oldSeatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == oldSeatNumber);

            if (oldSeatToUpdate != null)
            {
                oldSeatToUpdate["reserved"] = "false"; 
            }

            var newSeatToUpdate = auditoriumData["auditoriums"][0]["layout"]
                .SelectMany(row => row)
                .FirstOrDefault(seat => seat["seat"].ToString() == newSeatNumber);

            if (newSeatToUpdate != null)
            {
                newSeatToUpdate["reserved"] = "true"; 
            }

            string updatedJson = JsonConvert.SerializeObject(auditoriumData, Formatting.Indented);

            File.WriteAllText(filePath, updatedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating auditorium layout file: {ex.Message}");
        }
    }

    public static Movie GetMovieSeatPrices(string fileName)
    {
        try
        {
            string filenameOnly = Path.GetFileName(fileName);
            string json = File.ReadAllText(Path.Combine(jsonFolderPath, "MovieSchedule.json"));
            var movieSchedule = JsonConvert.DeserializeObject<List<Movie>>(json);

            return movieSchedule.FirstOrDefault(m => m.filename == filenameOnly);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading MovieSchedule.json file: " + ex.Message);
            return null;
        }
    }

    public static void ReserveSeatAndUpdateFile(JObject auditoriumData, string seatNumber, string fileName)
    {
        var auditorium = auditoriumData["auditoriums"][0];
        foreach (var row in auditorium["layout"])
        {
            foreach (var seat in row)
            {
                if (seat["seat"].ToString() == seatNumber)
                {
                    seat["reserved"] = "true";

                    string updatedJson = JsonConvert.SerializeObject(auditoriumData, Formatting.Indented);

                    try
                    {
                        File.WriteAllText(fileName, updatedJson);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating JSON file: {ex.Message}");
                    }

                    return;
                }
            }
        }
    }

    public static void CreateLayoutFile(string movieName, DateTime displayDate, string auditoriumName)
    {
        string fileName = Path.Combine(jsonFolderPath, $"{movieName}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json");

        if (!File.Exists(fileName))
        {
            using (FileStream fs = File.Create(fileName)) { }
        }

        string cinemaHallsJson = File.ReadAllText(CinemaHallsFilePath);
        CinemaHalls cinemaHalls = JsonConvert.DeserializeObject<CinemaHalls>(cinemaHallsJson);

        var auditorium = cinemaHalls.auditoriums.FirstOrDefault(a => a.name.Equals(auditoriumName, StringComparison.OrdinalIgnoreCase));
        if (auditorium != null)
        {
            CinemaHalls selectedAuditorium = new CinemaHalls
            {
                auditoriums = new[] { auditorium }
            };

            string selectedAuditoriumJson = JsonConvert.SerializeObject(selectedAuditorium, Formatting.Indented);

            File.WriteAllText(fileName, selectedAuditoriumJson);

            //Console.WriteLine($"Layout for {auditoriumName} copied successfully to {fileName}.");
        }
        else
        {
            Console.WriteLine("Auditorium not found.");
        }
    }

    public static void RemoveLayoutFile(string movieName, DateTime displayDate, string auditoriumName)
    {
        string fileName = Path.Combine(jsonFolderPath, $"{movieName}-{displayDate:yyyyMMdd-HHmm}-{auditoriumName}.json");

        if (File.Exists(fileName))
        {
            try
            {
                File.Delete(fileName);
                Console.WriteLine($"Layout file {fileName} removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing layout file {fileName}: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine($"Layout file {fileName} does not exist.");
        }
    }
}
