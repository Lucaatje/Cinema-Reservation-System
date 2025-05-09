using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class UserLogic
{

    public static void Start()
    {
        Console.WriteLine("Create an Account");

        string newUsername;
        do
        {
            Console.WriteLine("Enter username:");
            newUsername = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                Console.WriteLine("Username cannot be empty. Please enter a valid username.");
            }
            else if (UserAccess.UsernameExists(newUsername))
            {
                Console.WriteLine("Username already exists. Please enter a different username.");
                newUsername = string.Empty;
            }
        } while (string.IsNullOrWhiteSpace(newUsername));

        string newPassword;
        do
        {
            Console.WriteLine("Enter password:");
            newPassword = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                Console.WriteLine("Password cannot be empty. Please enter a valid password.");
            }
        } while (string.IsNullOrWhiteSpace(newPassword));

        string name;
        do
        {
            Console.WriteLine("Enter name:");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty. Please enter a valid name.");
            }
        } while (string.IsNullOrWhiteSpace(name));

        string email;
        do
        {
            Console.WriteLine("Enter email:");
            email = Console.ReadLine();
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
            else if (UserAccess.EmailExists(email))
            {
                Console.WriteLine("Email already exists. Please enter a different email.");
                email = string.Empty;
            }
        } while (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email));

        string phoneNumber;
        do
        {
            Console.WriteLine("Enter phone number:");
            phoneNumber = Console.ReadLine();
            if (!IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid phone number format. Please enter a valid phone number.");
            }
            else if (UserAccess.PhoneNumberExists(phoneNumber))
            {
                Console.WriteLine("Phone number already exists. Please enter a different phone number.");
                phoneNumber = string.Empty;
            }
        } while (string.IsNullOrWhiteSpace(phoneNumber) || !IsValidPhoneNumber(phoneNumber));

        bool creationResult = UserAccess.CreateUser(newUsername, newPassword, name, email, phoneNumber);

        if (creationResult)
        {
            Console.WriteLine("Account created successfully!");
            UserLogin.Start();
        }
        else
        {
            Console.WriteLine("Failed to create account.");
            Start();
        }
    }

    public static UserData GetUserData(string username)
    {
        return UserAccess.GetUserData(username);
    }



    public static User Login(string username, string password)
    {
        return UserAccess.Login(username, password);
    }

    public static bool ChangeAccount(string newInfo, int choice, string userName)
    {
        return UserAccess.ChangeAccount(newInfo, choice, userName);
    }

    public static bool DeleteAccount(string username)
    {
        return UserAccess.DeleteAccount(username);
    }

    public static bool IsValidEmail(string email)
    {
        string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        return Regex.IsMatch(email, pattern);
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        string pattern = @"^\d{10}$";
        return Regex.IsMatch(phoneNumber, pattern);
    }

    public static bool UsernameExists(string username)
    {
        return UserAccess.UsernameExists(username);
    }

    public static bool EmailExists(string email)
    {
        return UserAccess.EmailExists(email);
    }

    public static bool PhoneNumberExists(string phoneNumber)
    {
        return UserAccess.PhoneNumberExists(phoneNumber);
    }

    public static void UpdateAccount(User loggedInUser)
    {

        UserMenu.DisplayCurrentUserInfo(loggedInUser);

        int choice;
        bool isValidChoice = false;
        do
        {
            Console.WriteLine("Enter your choice (1-4):");
            string choiceInput = Console.ReadLine();
            if (!int.TryParse(choiceInput, out choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
            }
            else
            {
                isValidChoice = true;
            }
        } while (!isValidChoice);

        Console.WriteLine("Enter the new information:");
        string newInfo = Console.ReadLine();

        if (string.IsNullOrEmpty(newInfo))
        {
            Console.WriteLine("Input cannot be empty. Please enter valid information.");
            return;
        }

        if (choice == 1)
        {
            if (!IsValidEmail(newInfo))
            {
                Console.WriteLine("Invalid email format. Please enter a valid email address.");
            }
            else if (EmailExists(newInfo))
            {
                Console.WriteLine("This email is already in use. Please enter a different email.");
            }
            else
            {
                UpdateAccountInfo(newInfo, choice, loggedInUser.Username);
            }
        }
        else if (choice == 2)
        {
            if (!IsValidPhoneNumber(newInfo))
            {
                Console.WriteLine("Invalid phone number format. Please enter a valid phone number.");
            }
            else if (PhoneNumberExists(newInfo))
            {
                Console.WriteLine("This phone number is already in use. Please enter a different phone number.");
            }
            else
            {
                UpdateAccountInfo(newInfo, choice, loggedInUser.Username);
            }
        }
        else
        {
            UpdateAccountInfo(newInfo, choice, loggedInUser.Username);
        }
    }

    public static void UpdateAccountInfo(string newInfo, int choice, string userName)
    {
        bool result = ChangeAccount(newInfo, choice, userName);
        if (result)
        {
            Console.WriteLine($"Your information has been updated successfully.\n");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine($"Failed to update information.\n");
        }
    }
}
