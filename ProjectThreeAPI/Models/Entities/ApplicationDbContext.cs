using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Npc> Npcs { get; set; }
    public DbSet<DeckEntry> DeckEntries { get; set; }
}