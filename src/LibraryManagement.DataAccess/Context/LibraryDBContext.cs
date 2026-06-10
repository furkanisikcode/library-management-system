using LibraryManagement.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Context;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    // DbSet<T>: Her entity için bir tablo
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Penalty> Penalties { get; set; }
}