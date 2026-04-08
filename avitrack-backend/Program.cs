using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AviTrack.Api.Data;
using AviTrack.Api.Services;
using AviTrack.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AirportService>();
builder.Services.AddScoped<FlightService>();

var openSkySettings = builder.Configuration
    .GetSection("OpenSky")
    .Get<OpenSkySettings>() ?? new OpenSkySettings();

builder.Services.AddSingleton(openSkySettings);
builder.Services.AddSingleton<OpenSkyTokenService>();
builder.Services.AddHttpClient<OpenSkyTokenService>();
builder.Services.AddHttpClient<OpenSkyService>();

builder.Services.AddHttpClient<AirportDataService>();
builder.Services.AddScoped<DashboardService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();