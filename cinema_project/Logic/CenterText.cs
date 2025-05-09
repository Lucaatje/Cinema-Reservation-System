public static class CenterText
{
    public static void print(string text, string color)
    {
        switch (color)
        {
            case "Red":
            Console.ForegroundColor = ConsoleColor.Red;
            break;
            case "Yellow":
            Console.ForegroundColor = ConsoleColor.Yellow;
            break;            
            case "Green":
            Console.ForegroundColor = ConsoleColor.Green;
            break;
            case "Cyan":
            Console.ForegroundColor = ConsoleColor.Cyan;
            break;
            default:
            Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
            break;   
        }
        int screenWidth = Console.WindowWidth;
        int stringWidth = text.Length;
        int spaces = (screenWidth - stringWidth) / 2;
        Console.WriteLine(new string(' ', Math.Max(0, spaces)) + text);
    }

    public static void printart(string art)
    {
        int screenWidth = Console.WindowWidth;
        string[] lines = art.Split('\n');
        foreach (string line in lines)
        {
            int stringWidth = line.Length;
            int spaces = (screenWidth - stringWidth) / 2;
            Console.WriteLine(new string(' ', Math.Max(0, spaces)) + line);
        }
    }
}