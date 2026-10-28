# 🎬 FilmOfTheDay

**FilmOfTheDay** is a full-featured ASP.NET Core MVC web application built using **Clean Architecture** principles.  
It’s a social platform for film enthusiasts to share, discover, and connect through posts, profiles, and film-based interactions. [Try it yourself!](https://film-of-the-day.fly.dev)

---

## 🏗️ Clean Architecture Overview

This project follows a **modular Clean Architecture** design pattern to ensure scalability, testability, and maintainability.  
Each layer has a single responsibility, allowing the app to grow easily without coupling business logic to UI or data layers.

### **Architecture Layers**

1. **Core Layer (Domain Entities)**  
   - Contains the fundamental business entities like `User`, `FilmPost`, `Friendship`, and `Notification`.
   - Completely independent of frameworks — pure C# logic.

2. **Infrastructure Layer (Data Access & Persistence)**  
   - Uses **Entity Framework Core** for database operations.
   - Contains the `ApplicationDbContext` which manages entities and database migrations.
   - Uses **PostgreSQL** (via Npgsql) in both Development and Production.

3. **Web Layer (Presentation & Services)**  
   - Implements MVC pattern (`Controllers`, `Views`, `ViewModels`).
   - Contains all UI logic, route handling, and view rendering.
   - Depends only on service interfaces, never on data access directly.

4. **Services Layer (Business Logic)**  
   - All controllers depend on service interfaces (like `IConnectionService`, `IHomeFeedService`, etc.).  
   - Each service handles one domain responsibility (Connections, Notifications, Home Feed, Posts, etc.).
   - All services are injected via **Dependency Injection (DI)** for clean separation of concerns.

---

## ⚙️ Dependency Injection (DI)

FOTD heavily uses **Dependency Injection**, a built-in ASP.NET Core feature, to achieve loose coupling.  
All major services (`HomeFeedService`, `ConnectionService`, `NotificationService`, etc.) are registered in the DI container and automatically provided to controllers.
This ensures controllers are only responsible for request handling — not object creation or business logic.

---

## 🌍 Environment Configuration

The project uses **two environments**:

- **Development (Dev):**
  - Uses a separate **PostgreSQL** dev database hosted on **Fly.io**.
  - Connection string is read from `ConnectionStrings:LOCAL_DEV_DB` in `appsettings.Development.json`.

- **Production (Prod):**
  - Uses **PostgreSQL** hosted on **Fly.io**.
  - Connection string is read from the `FLY_PROD_DB` environment variable (set as a Fly secret).
  - The live app runs on **Fly.io** at [film-of-the-day.fly.dev](https://film-of-the-day.fly.dev).

Environment configuration is handled using the built-in `DOTNET_ENVIRONMENT` variable:
```bash
export DOTNET_ENVIRONMENT=Development
# or
export DOTNET_ENVIRONMENT=Production
```

Each environment uses its own settings, defined in:
- `appsettings.Development.json`
- `appsettings.Production.json`

---

## 🚀 Continuous Integration & Deployment (CI/CD)

The project deploys to **Fly.io** via **GitHub Actions** (`.github/workflows/fly-deploy.yml`):

- Every push to `main` triggers `flyctl deploy --remote-only`.
- Fly builds the app from the `Dockerfile` and rolls out the new image.
- App settings live in `fly.toml`; machines auto-stop when idle and auto-start on request.
- This ensures the production site is always up to date with the latest tested code.

---

## 💡 Implemented Features

### 👤 User System
- Secure authentication & authorization with ASP.NET Identity.
- Individual user profiles showing posts and friend stats.

### 🏠 Home Feed
- Displays posts from the logged-in user and their friends.
- Automatically updates when new posts are created.

### 📝 Film Posts
- Users can create posts about movies.
- Integrates with **TMDb API** for movie search and poster retrieval.

### 🔍 Search
- Allows searching for other users by username.

### 🤝 Friend System (Connections)
- Send, accept, or receive friend (follow) requests.
- Friendship status and counts displayed dynamically.

### 🔔 Notifications
- In-app notification system for new friend requests or accepted requests.
- Managed through the `NotificationService` with read/unread states.

---

## 🧩 Scalability & Maintainability

The Clean Architecture combined with service-oriented design ensures the project can scale easily:

- Each feature is **self-contained** with its own controller, service, and view models.
- Database and infrastructure concerns are isolated from business logic.
- Adding new features (like comments, likes, or direct messages) only requires extending the service layer and creating new UI components.
- EF Core ensures data access is optimized and manageable even with large user and post counts.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core MVC 8.0  
- **ORM:** Entity Framework Core  
- **Frontend:** Razor Views + TailwindCSS  
- **Database:** PostgreSQL (Dev & Prod, hosted on Fly.io)  
- **Hosting:** Fly.io (Docker)  
- **External API:** The Movie Database (TMDb) API  

---

## 🧠 Key Takeaways

- Built using modern **Clean Architecture** principles.  
- Fully **asynchronous** service layer with **Dependency Injection**.  
- **Scalable**, **testable**, and **deployment-ready** architecture.  
- Clean separation between data, business, and UI layers.

---

## 🧪 Future Enhancements

- 💬 Commenting and likes on posts  
- 📨 Real-time notifications (SignalR)  
- 🖼️ Profile photo uploads  
- 🧩 Modular API endpoints for mobile apps  
- 🧠 Caching and query optimization  

---

## 📄 License

This project is open-source under the **MIT License**.
