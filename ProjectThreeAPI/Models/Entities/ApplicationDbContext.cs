using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Npc> Npcs { get; set; }
    public DbSet<NpcsDeckEntry> NpcsDeckEntries { get; set; }
    public DbSet<PlayersSave> PlayersSaves { get; set; }
    public DbSet<PlayersCardEntry> PlayersCardEntries { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<PlayersDeckEntry> PlayersDeckEntries { get; set; }
    public DbSet<Battle> Battles { get; set; }
}