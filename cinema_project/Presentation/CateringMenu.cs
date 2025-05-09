using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public static class CateringMenu
{

    public static string Choice { get; set; }
    public static void StartMenu(ref User loggedInUser)
    
    {
        bool browsemenu = false;
        while (!browsemenu)
        {
            CenterText.print("=================================", "Cyan");
            CenterText.print("||                             ||", "Cyan");
            CenterText.print("|| 1. Browse Food              ||", "Cyan");
            CenterText.print("|| 2. Browse Drinks            ||", "Cyan");
            CenterText.print("|| 3. Go back to user menu     ||", "Cyan");
            CenterText.print("||                             ||", "Cyan");
            CenterText.print("=================================", "Cyan");
            char userinput = Console.ReadKey().KeyChar;
            switch (userinput)
            {
                case '1':
                    Choice = "Food";
                    Console.Clear();
                    CateringLogic.ViewItems(Choice, CateringAccess.cateringmenu);
                    Console.WriteLine("Press any key to continue..");
                    Console.ReadKey();
                    Console.Clear();  
                    break;
                case '2':
                    Choice = "Drink";
                    Console.Clear();
                    CateringLogic.ViewItems(Choice, CateringAccess.cateringmenu);
                    Console.WriteLine("Press any key to continue..");
                    Console.ReadKey();
                    Console.Clear();  
                    break;
                case '3':
                    Console.Clear();
                    browsemenu = true;
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
    }
}