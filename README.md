# 🎬 FilmOfTheDay

**FilmOfTheDay** is a modern social web application built with **ASP.NET Core MVC** and **Entity Framework Core**, designed around a **clean, scalable, and modular architecture**.  
It enables users to share film posts, follow friends, and stay updated through personalized feeds and notifications — all while maintaining clear separation between application layers for long-term scalability and maintainability.

---

## 🏗️ Architecture Overview

FilmOfTheDay follows **Clean Architecture (a.k.a. Onion / Hexagonal Architecture)** principles — ensuring that the system is modular, testable, and easy to scale.

### 📚 Layered Design

```
FilmOfTheDay
├── Core/               → Domain Entities & Enums (pure business logic)
├── Infrastructure/     → Data layer (EF Core DbContext, repositories)
├── Web/                → MVC layer (Controllers, Views, Services, ViewModels)
```

### 🧩 Layer Responsibilities

| Layer | Description |
|-------|--------------|
| **Core** | Contains all **entities**, **value objects**, and **enums** (e.g., `User`, `FilmPost`, `Friendship`, `NotificationType`). This layer has **no dependencies** on others — it represents pure business logic. |
| **Infrastructure** | Handles **data persistence** using **Entity Framework Core**. It defines the `ApplicationDbContext` and manages all database access. This layer depends only on the Core layer. |
| **Web** | The presentation and API layer built with **ASP.NET Core MVC**. It contains **controllers**, **services**, and **view models**, ensuring that business logic stays out of controllers. |

This separation allows independent evolution of each layer — for example, you could replace the MVC frontend with a Web API or Blazor app without touching the domain or database code.

---

## ⚙️ Key Design Principles

- **Dependency Inversion:** Controllers depend on interfaces (e.g., `IHomeFeedService`, `INotificationService`) — never on direct implementations.
- **Single Responsibility:** Each service handles one concern: profile data, feed aggregation, notifications, etc.
- **Separation of Concerns:** Controllers only coordinate; business logic lives in services; data logic stays in the infrastructure layer.
- **Scalability:** The async EF Core queries, lightweight service boundaries, and clean structure make horizontal scaling (or microservice extraction) straightforward.

---

## 🧠 Implemented Features

### 👤 User Profiles
- View any user’s profile with their film posts.
- Display username, email, number of posts, and friend count.
- Follow/unfollow functionality via `ConnectionService`.

### 🏠 Home Feed
- Personalized feed showing posts from friends and self.
- `HomeFeedService` aggregates posts efficiently using EF Core projections.
- Sorted by creation date (latest first).

### 🔍 Search
- Search users by username (case-insensitive).
- Clean, async, server-side search using EF Core and `LIKE` expressions.
- Displays basic profile info with placeholder images.

### 🔔 Notifications
- `NotificationService` manages creation, retrieval, and marking notifications as read.
- Each notification includes message, timestamp, link, and type.
- Clean separation between database entities and view models.

### 👥 Friendships
- Managed through a `Friendship` table with `Pending`, `Accepted`, and `Rejected` states.
- Encapsulated by `ConnectionService` for cleaner business logic.

---

## 🧰 Technologies Used

| Technology | Purpose |
|-------------|----------|
| **ASP.NET Core MVC 8.0** | Web framework |
| **Entity Framework Core** | ORM for data access |
| **SQL Server / SQLite** | Database provider |
| **TailwindCSS** | Modern styling (for views) |
| **Dependency Injection** | Built-in DI container |
| **Identity / Claims** | Authentication and user management |

---

## 🚀 Scalability & Extensibility

Because of its **clean separation**, FilmOfTheDay can be scaled or extended with minimal effort:

- ✅ Add an **API layer** → expose the same services as REST endpoints.
- ✅ Replace EF Core with another data source → only `Infrastructure` changes.
- ✅ Add caching (e.g., Redis) → inject into service layer.
- ✅ Deploy to cloud → independent horizontal scaling of web & database layers.

---

## 🧪 Future Enhancements

- 💬 Commenting and likes on posts  
- 📨 Real-time notifications (SignalR)  
- 🖼️ Profile photo uploads  
- 🧩 Modular API endpoints for mobile apps  
- 🧠 Caching and query optimization  

---

## 🧾 License

This project is licensed under the **MIT License**.  
You’re free to use, modify, and distribute it with attribution.