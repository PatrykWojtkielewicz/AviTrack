using Microsoft.EntityFrameworkCore;
using AviTrack.Api.Data;
using AviTrack.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.MapControllers();

app.Run();