using System.Net;
using System.Text.Json.Nodes;
using BookLibraryApi.Models;
using BookLibraryApi.Models.Database;
using BookLibraryApi.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryApi.Controllers;

[ApiController]
[Route("/[controller]")]
public class UsersController : Controller
{
    [HttpGet("/[controller]/user")]
    public IActionResult GetUserById([FromQuery] int id)
    {
        var user = DataContext.Context.Users.FirstOrDefault(user => user.UserId == id);

        if (user == null)
        {
            return NotFound();
        }

        Console.WriteLine($"Requested user: {user.UserId}.{user.Username}");
        
        return Ok(user);
    }

    [HttpPost("/[controller]/signUp")]
    public IActionResult SignUpUser([FromBody] User userData)
    {
        var context = new DataContext();
        
        if (context.Users.FirstOrDefault(user => user.Username == userData.Username) != null)
        {
            return Conflict("Exists username");
        }

        if (context.Users.FirstOrDefault(user => user.Email == userData.Email) != null)
        {
            return Conflict("Exists email");
        }

        context.Users.Add(userData);
        context.SaveChanges();
        
        Console.WriteLine($"Registered up new account: {userData.UserId}.{userData.Username}");

        string token = JwtService.Service.UserToken(userData);
        
        Console.WriteLine($"Token: {token}");
        
        return Ok(token);
    }

    [HttpPost("/[controller]/login")]
    public IActionResult Login([FromBody] LoginDTO data)
    {
        var context = new DataContext();
        
        User user = null!;

        if (data.Username != null)
        {
            user = context.Users.FirstOrDefault(u => u.Username == data.Username);
        }
        else if (data.Email != null)
        {
            user = context.Users.FirstOrDefault(u => u.Email == data.Email);
        }

        if (user == null)
        {
            return NotFound("Not found");
        }

        if (user.Password == data.Password)
        {
            Console.WriteLine($"Authorized user: {user.UserId}.{user.Username}");
            
            var token = JwtService.Service.UserToken(user);
            
            Console.WriteLine($"Token: {token}");
            
            return Ok(token);
        }
        else
        {
            Console.WriteLine($"Authorization failed: {user.UserId}.{user.Username} : wrong password");
            
            return Unauthorized("Unauthorized");
        }
    }

    [HttpPost("/[controller]/userByToken")] 
    public async Task<IActionResult> GetUserByToken()
    {
        string token = "";

        using (var reader = new StreamReader(HttpContext.Request.Body))
        {
            token = await reader.ReadToEndAsync();
        }

        token = token.Replace("\"", "");
            
        
        Console.WriteLine($"Registered attempt of getting user by token: {token}");
        
        if (JwtService.Service.IsTokenValid(token))
        {
            int userId = JwtService.Service.GetUserIdByToken(token);

            var context = new DataContext();
            
            User user = context.Users.FirstOrDefault(u => u.UserId == userId);

            user.Password = null;

            Console.WriteLine($"Requested user: {user.UserId}.{user.Username}");

            return Ok(user);
        }
        else
        {
            return Unauthorized("Unauthorized");
        }
    }

    [HttpPatch("/[controller]/changeName")]
    public IActionResult ChangeName([FromBody] string newName)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        if (!JwtService.Service.IsTokenValid(token))
        {
            return Unauthorized("Unauthorized");
        }

        var userId = JwtService.Service.GetUserIdByToken(token);

        var context = new DataContext();
        
        User user = context.Users.FirstOrDefault(u => u.UserId == userId)!;
        user.Name = newName;
        
        context.SaveChanges();
        
        Console.WriteLine($"{user.UserId}.{user.Username} changed name: {newName}");

        return Ok(JwtService.Service.UserToken(user));
    }
}