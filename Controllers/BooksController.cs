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
        DataContext context = new DataContext();

        List<Book> books = context.Books.ToList();

        foreach (var book in books)
        {
            book.UploadedUser = context.Users.FirstOrDefault(u => u.UserId == book.UserId);

            book.UploadedUser.Password = null!;
            book.UploadedUser.Email = null!;
        }

        return Ok(books);
    }
}