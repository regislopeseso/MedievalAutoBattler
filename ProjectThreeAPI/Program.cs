using MedievalAutoBattler.Service.Admin;
using MedievalAutoBattler.Service.Battles;
using MedievalAutoBattler.Service.Players;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<AdminCardsService>();
builder.Services.AddScoped<AdminNpcsService>();
builder.Services.AddScoped<PlayersSavesService>();
builder.Services.AddScoped<PlayersStatsService>();
builder.Services.AddScoped<PlayersCardsService>();
builder.Services.AddScoped<PlayersDecksService>();
builder.Services.AddScoped<BattlesNewBattlesService>();
builder.Services.AddScoped<BattlesPlaysService>();
builder.Services.AddScoped<BattlesResultsService>();
builder.Services.AddScoped<PlayersBoostersService>();

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