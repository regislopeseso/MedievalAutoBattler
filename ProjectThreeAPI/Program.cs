using MedievalAutoBattler.Service;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<AdminsService>();
builder.Services.AddScoped<AdminsDeleteDbDataService>();
builder.Services.AddScoped<PlayersService>();

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