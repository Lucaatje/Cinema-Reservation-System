using Newtonsoft.Json;

public static class CateringAccess
{
    static public string cateringmenu = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\cateringmenu.json";


    public static void SaveMenuToJson(List<Dictionary<string, object>> menu, string file)
    {
        try
        {
            string json = JsonConvert.SerializeObject(menu, Formatting.Indented);
            File.WriteAllText(file, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the menu to JSON file: {ex.Message}");
        }
    }

    public static List<Dictionary<string, object>> LoadMenuFromJson(string file)
    {
        if (!File.Exists(file))
        {
            Console.WriteLine("Menu file does not exist.");
            return new List<Dictionary<string, object>>();
        }

        string json = File.ReadAllText(file);
        return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json) ?? new List<Dictionary<string, object>>();
    }


}