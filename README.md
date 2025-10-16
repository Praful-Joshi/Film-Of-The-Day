# 🎬 Film of the Day

A social platform for film enthusiasts to share and discover their daily movie picks. Built with ASP.NET Core MVC and Entity Framework Core.

## 🌟 Features

- **User Authentication**: Secure login and registration system
- **Movie Search**: Search and select movies from a vast database
- **Daily Posts**: Share your film of the day with descriptions and images
- **User Profiles**: View user profiles and their film posts
- **Responsive Design**: Works seamlessly on both desktop and mobile devices

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: Entity Framework Core with SQL Server
- **Frontend**: 
  - Razor Views for server-side rendering of HTML pages
  - Tailwind CSS for styling
- **Authentication**: Custom authentication using BCrypt for password hashing and browser-cookies based authorization

## 🚀 Project Structure

```
FilmOfTheDay/
├── FilmOfTheDay.Core/           # Domain entities and interfaces
├── FilmOfTheDay.Infrastructure/ # Data access and database context
└── FilmOfTheDay.Web/           # Web application and UI
```

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server
- Node.js (for frontend asset management)

## ⚙️ Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/Praful-Joshi/Film-Of-The-Day.git
   cd Film-Of-The-Day
   ```

2. Update the database connection string in `FilmOfTheDay.Web/appsettings.json`

3. Apply database migrations:
   ```bash
   dotnet ef database update --project FilmOfTheDay.Infrastructure --startup-project FilmOfTheDay.Web
   ```

4. Run the application:
   ```bash
   dotnet run --project FilmOfTheDay.Web
   ```

## 🎯 Future Enhancements

- Friend system for following other users
- Comments and reactions on posts
- Personal watchlist management
- Movie recommendations based on user preferences
- Advanced search filters

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

[Praful Joshi](https://github.com/Praful-Joshi)
