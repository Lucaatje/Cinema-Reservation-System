public static class UserLogin
{
    public static User Start()
    {
        Console.WriteLine("Login");

        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();

        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        User user = UserLogic.Login(username, password);

        if (user == null)
        {
            Console.Clear();
            Console.WriteLine("Invalid username or password.");
            return null;
        }
        else
        {
            Console.WriteLine($"Welcome, {user.Username}!");
            if (user is Admin)
            {
                AdminMenu.Start(ref user);
            }
            else if (user is Customer)
            {
                UserMenu.Start(ref user);
            }
            return user; 
        }
    }
}
