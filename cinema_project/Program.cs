class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        bool exitRequested = false;

        while (!exitRequested)
        {
            CenterText.print("Welcome to the Cinema Application!", "Cyan");
            Console.WriteLine();
            Menu.Start();
        }
    }
}