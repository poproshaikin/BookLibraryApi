const string corsOptionsName = "basicCorsOptions";

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