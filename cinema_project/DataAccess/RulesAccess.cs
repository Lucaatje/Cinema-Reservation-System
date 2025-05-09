public static class RulesAccess
{
    public const string RulesCSVFile = "C:\\Users\\Gebruiker\\OneDrive - Hogeschool Rotterdam\\Github\\Cinema-Project\\cinema_project\\DataSources\\cinemarules.csv";
    public static List<string> ReadRulesFromCSV(string RulesCSVFile)
    {
        List<string> rules = new List<string>();

        try
        {
            using (StreamReader reader = new StreamReader(RulesCSVFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    rules.Add(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }
        return rules;
    }

    public static void WriteRulesToCSV(List<string> rules, string rulesCSVFile)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(rulesCSVFile))
            {
                foreach (string rule in rules)
                {
                    writer.WriteLine(rule);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to CSV file: {ex.Message}");
        }
    }


}