# 🎬 Film of the Day

A social media app for film lovers — share daily picks, discover friends' posts and browse an Instagram-like home feed. The project is built with ASP.NET Core MVC and EF Core using Clean Architecture.

## Implemented (current) features

- User registration, login and cookie-based authentication
- Create and view film posts (image + description + date)
- Profile pages showing a user's posts
- Follow / connection requests (send / accept)
- Notifications plumbing (service + controller) for basic alerts
- Search page (query UI) and a vertical home feed that shows your and friends' posts sorted by newest
- Responsive UI using Tailwind utility classes (with some Bootstrap assets)

## Tech

- .NET 8 (ASP.NET Core MVC)
- Entity Framework Core (SQL Server by default)
- Tailwind CSS for utilities; minimal JS for interactivity

## Quick start

1. Clone and enter the repo
   ```bash
   git clone <repo-url>
   cd Film-Of-The-Day
   ```
2. Update the connection string in `FilmOfTheDay.Web/appsettings.json`.
3. Apply migrations and seed (dev):
   ```bash
   dotnet ef database update --project FilmOfTheDay.Infrastructure --startup-project FilmOfTheDay.Web
   ```
4. (Optional) Rebuild Tailwind if you edit styles:
   ```bash
   npx tailwindcss -i ./FilmOfTheDay.Web/wwwroot/css/input.css -o ./FilmOfTheDay.Web/wwwroot/css/site.css --minify
   ```
5. Run the web app:
   ```bash
   dotnet run --project FilmOfTheDay.Web
   ```

## Dev notes

- When you reset the DB, clear browser cookies or sign out — auth cookies are independent of DB state.
- Follow/connection endpoints are AJAX-friendly and expect antiforgery tokens for POSTs.
- Home feed service aggregates posts from the current user + accepted friends.

## Project layout

```
FilmOfTheDay/
├─ FilmOfTheDay.Core/             # Entities
├─ FilmOfTheDay.Infrastructure/   # DbContext, Migrations, seeders
└─ FilmOfTheDay.Web/              # Controllers, Views, Services, Assets
```


