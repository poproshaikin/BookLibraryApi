using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Models;

public class User
{
    [Key] public int UserId { get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}