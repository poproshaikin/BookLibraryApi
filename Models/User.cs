using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Models;

public class User
{
    public static readonly User ExampleUser;
    
    [Key] public int UserId { get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    static User()
    {
        ExampleUser = new User
        {
            UserId = 123123123,
            Name = "Stasik",
            Surname = "Motsnyi",
            Username = "poproshaikin",
            Email = "email@example.com",
            Password = "12345678"
        };
    }
}