using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibraryApi.Models;

public class Like
{
    [Key] public int LikeId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
}