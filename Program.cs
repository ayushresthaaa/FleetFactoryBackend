using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using FleetFactory.Infrastructure.Persistence;
using FleetFactory.Infrastructure.Identity; 

//register services and repositories
using FleetFactory.API.Extensions;
using FleetFactory.Infrastructure.Services; // for cache service and low stock

//middleware
using FleetFactory.API.Middleware;
//seeders 
using FleetFactory.Infrastructure.Seeders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


//configure DI for the services registration
builder.Services.AddProjectServicesAndRepositories();


//application user setup for identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//validate JWT tokens for authentication
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(
        options =>
        {
            var jwt = builder.Configuration.GetSection("Jwt");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwt["Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt["SecretKey"]!)
                ),

                ValidateLifetime = true,

                RoleClaimType = ClaimTypes.Role,
            };
        }
    );
//hosted service means it will run in the background and check for low stock every hour and send notifications to the users
builder.Services.AddHostedService<LowStockBackgroundService>();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

//scheme means auth types, we will use JWT for our API
//challenge -> if fails respond with rules of JWT

app.UseCors("AllowFrontend");

app.UseAuthentication();
   
app.UseAuthorization();


app.MapControllers();

app.MapGet("/health", () =>
    Results.Ok(new { status = "FleetFactory API is running" })
);




//use this to seed roles and admin user on startup

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    await RoleSeeder.SeedAsync(roleManager);
}
app.Run();