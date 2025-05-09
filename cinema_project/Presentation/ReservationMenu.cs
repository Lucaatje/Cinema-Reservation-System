public static class ReservationMenu
{
    public static void Start(ref User loggedInUser)
    {
        bool logoutRequested = false;

        while (!logoutRequested)
        {
            Console.WriteLine();
            CenterText.print("=================================", "Cyan");
            CenterText.print("||                             ||", "Cyan");           
            CenterText.print("|| 1. Make reservation         ||", "Cyan");
            CenterText.print("|| 2. View reservation history ||", "Cyan");
            CenterText.print("|| 3. Cancel reservation       ||", "Cyan");
            CenterText.print("|| 4. Edit reservation         ||", "Cyan");
            CenterText.print("|| 5. Go back to user menu     ||", "Cyan");
            CenterText.print("||                             ||", "Cyan"); 
            CenterText.print("=================================", "Cyan");
            char input = Console.ReadKey().KeyChar;
            switch (input)
            {
                case '1':
                    Console.Clear();
                    ReservationLogic.MakeReservation(loggedInUser.Username);
                    break;
                case '2':
                    Console.Clear();
                    ReservationLogic.ViewReservationHistory(loggedInUser.Username);
                    break;
                case '3':
                    Console.Clear();
                    ReservationLogic.CancelReservation(loggedInUser.Username);
                    break;
                case '4':
                    Console.Clear();
                    ReservationLogic.EditReservation(loggedInUser.Username);
                    break;
                case '5':
                    Console.Clear();
                    logoutRequested = true;
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }
}