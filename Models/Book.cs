using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Models;

public class Book
{
    [Key] public int BookId { get; set; }
    public int UserId { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string AuthorFullName { get; set; }
    public string Genre { get; set; }
    public int PageCount { get; set; }
    public int Price { get; set; }
    
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    
    [NotMapped]
    public User UploadedUser { get; set; }
}