static class Menu
{
    static public void Start()
    {
        CenterText.printart(TextArt.loginprint());
        Console.WriteLine();
        CenterText.print("=================================", "Cyan");
        CenterText.print("||                             ||", "Cyan");
        CenterText.print("|| 1. Login                    ||", "Cyan");
        CenterText.print("|| 2. Create an Account        ||", "Cyan");
        CenterText.print("|| 3. View Cinema Rules        ||", "Cyan");
        CenterText.print("|| 4. Exit                     ||", "Cyan");
        CenterText.print("||                             ||", "Cyan");
        CenterText.print("=================================", "Cyan");
        char input = Console.ReadKey().KeyChar;
        if (input == '1')
        {
            Console.WriteLine();
            User loggedInUser = UserLogin.Start();
            if (loggedInUser != null)
            {
                if (loggedInUser is Admin)
                {
                    AdminMenu.Start(ref loggedInUser);
                }
                else
                {
                    UserMenu.Start(ref loggedInUser);
                }
            }
            else
            {
                Start(); // Restart the menu if login failed
            }
        }
        else if (input == '2')
        {
            Console.WriteLine();
            UserLogic.Start();
        }
        else if (input == '3')
        {
            //Rules method call
            RulesLogic.ViewAllRules();
        }
        else if (input == '4')
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }
}
