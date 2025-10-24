using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Services.Interfaces;
using FilmOfTheDay.Web.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var env = builder.Environment.EnvironmentName;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("DefaultConnection");

if (env == "Development")
{
    //print env to console
    Console.WriteLine("Environment: Development");
    builder.Services.AddDbContext<SqlServerApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddScoped<ApplicationDbContext, SqlServerApplicationDbContext>();
}
else
{
    //print env to console
    Console.WriteLine("Environment: Production");
    builder.Services.AddDbContext<PostgresApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
    builder.Services.AddScoped<ApplicationDbContext, PostgresApplicationDbContext>();
}


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IHomeFeedService, HomeFeedService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    if (env == "Development")
    {
        var devDb = services.GetRequiredService<SqlServerApplicationDbContext>();
        devDb.Database.Migrate(); // applies pending migrations to local dev DB
    }
    else
    {
        var prodDb = services.GetRequiredService<PostgresApplicationDbContext>();
        prodDb.Database.Migrate(); // applies pending migrations to production DB
    }
}


//seed data
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     // Run all seeders
//     await DBSeeder.SeedDataToDB(dbContext);
//     //delete last 5 posts from the filmposts table
//     // var postsToDelete = dbContext.FilmPosts.OrderByDescending(p => p.Id).Take(5).ToList();
//     // dbContext.FilmPosts.RemoveRange(postsToDelete);
//     // await dbContext.SaveChangesAsync();
// }


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
