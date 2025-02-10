using MedievalAutoBattler.Service;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<AdminCardsService>();
builder.Services.AddScoped<AdminNpcsService>();
builder.Services.AddScoped<PlayerSavesService>();
builder.Services.AddScoped<PlayerStatsService>();
builder.Services.AddScoped<PlayerCardsService>();
builder.Services.AddScoped<PlayerDecksService>();
builder.Services.AddScoped<BattleSavesService>();
builder.Services.AddScoped<BattleNpcsService>();
builder.Services.AddScoped<BattlePlayersService>();
builder.Services.AddScoped<BattleResultsService>();
builder.Services.AddScoped<DeckBoostersService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => { options.CommandTimeout(120); }
        );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();