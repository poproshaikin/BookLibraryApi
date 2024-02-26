using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Models.Database;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Like> Likes { get; set; }

    public static DataContext Context;
    
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

        modelBuilder.Entity<Like>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<Like>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(l => l.BookId);
    }
}