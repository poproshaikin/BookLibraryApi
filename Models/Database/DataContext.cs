using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Models.Database;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    
    public string DbPath { get; } = "database.db";

    public DataContext() : base()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite($"Data Source = {DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId);
    }
}