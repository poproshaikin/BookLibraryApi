using BookLibraryApi.Models;
using BookLibraryApi.Models.Database;
using Microsoft.AspNetCore.Identity;

const string corsOptionsName = "basicCorsOptions";

_ = new JwtService("8wzTHNp3j9QY1X+0/WfO8iMsmC+oSQ21oYg5DCln2tI=");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsOptionsName, corsOptions =>
    {
        corsOptions.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapControllers();
app.UseCors(corsOptionsName);

app.Run();