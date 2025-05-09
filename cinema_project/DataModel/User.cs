public abstract class User
{
    public string Username { get; }
    public string Password { get; }
    public string Role { get; }

    protected User(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
    }
}

