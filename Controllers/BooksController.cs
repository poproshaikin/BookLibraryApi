using BookLibraryApi.Models;
using BookLibraryApi.Models.Database;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryApi.Controllers;

[ApiController]
[Route("/[controller]")]
public class BooksController : Controller
{
    [HttpGet]
    public IActionResult GetAllBooks()
    {
        var context = new DataContext();

        var books = context.Books.ToList();

        foreach (var book in books)
        {
            book.UploadedUser = context.Users.FirstOrDefault(u => u.UserId == book.UserId);

            book.UploadedUser.Password = null!; 
            book.UploadedUser.Email = null!;
        }

        Console.WriteLine("Requested books list");
        
        return Ok(books);
    }

    [Route("/[controller]/book")]
    public IActionResult GetBookById([FromQuery]int id)
    {
        var context = new DataContext();

        var book = context.Books.FirstOrDefault(book => book.BookId == id);

        if (book == null)
        {
            return NotFound();
        }

        book.UploadedUser = context.Users.FirstOrDefault(user => user.UserId == book.UserId)!;

        book.UploadedUser.Email = null!;
        book.UploadedUser.Password = null!;

        Console.WriteLine($"Requested book: {book.BookId}.{book.Name}");

        return Ok(book);
    }
}