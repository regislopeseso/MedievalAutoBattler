using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }
}