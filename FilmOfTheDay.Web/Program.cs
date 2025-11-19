using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Services.Interfaces;
using FilmOfTheDay.Web.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var env = builder.Environment.EnvironmentName;

// Decide which DB connection string to use
string? connectionString = env switch
{
    "Development" =>
        Environment.GetEnvironmentVariable("FLY_DEV_DB")
        ?? builder.Configuration.GetConnectionString("DefaultConnection"),

    "Production" =>
        Environment.GetEnvironmentVariable("FLY_PROD_DB")
        ?? builder.Configuration.GetConnectionString("DefaultConnection"),

    _ => throw new Exception($"Unknown environment: {env}")
};
Console.WriteLine($"[DEBUG] ENV = {env}");
Console.WriteLine($"[DEBUG] Conn = '{connectionString}'");

// If connection string still ended up null → fail fast with a clear message
if (string.IsNullOrWhiteSpace(connectionString))
    throw new Exception($"Database connection string NOT found for environment '{env}'. " +
                        $"Expected env var FLY_DEV_DB or FLY_PROD_DB.");

// Register Postgres DB context
builder.Services.AddDbContext<PostgresApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ApplicationDbContext, PostgresApplicationDbContext>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IHomeFeedService, HomeFeedService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddHttpContextAccessor();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // required for Fly.io
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<PostgresApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
