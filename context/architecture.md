# Architecture Context

## Stack

| Layer               | Technology                       | Role                                    |
| ------------------- | -------------------------------- | --------------------------------------- |
| Framework           | ASP.NET Core MVC                 | Web application framework               |
| Backend Language    | C#                               | Business logic and application services |
| Frontend            | Razor Views                      | Server-side rendered user interface     |
| Navigation          | Expo Router                      | File-based screen navigation            |
| Authentication      | ASP.NET Core Identity            | Authentication and authorization        |
| Authorization       | Role-Based Authorization         | Admin, Referee, Coach, and Guest access |
| Database            | Microsoft SQL Server             | Persistent data storage                 |
| ORM                 | Entity Framework Core            | Database access and object mapping      |
| Database Migrations | Entity Framework Core Migrations | Database schema management              |

## System Boundaries

- `Controllers/` — Handle HTTP requests and coordinate application flow.
- `Services/` — Business logic and use case implementations.
- `Interfaces/` — Contracts for services and repositories.
- `Models/` — Domain entities and database models.
- `DTOs/` — Data Transfer Objects used for data exchange between layers.
- `ViewModels/` — Presentation-specific models for Views.
- `Data/` — DbContext and Entity Framework Core configuration.
- `Migrations/` — Database migration files only.
- `Views/` — Razor views and UI rendering only.
- `wwwroot/` — Static assets (CSS, JavaScript, images).
- `Areas/Identity/` — Authentication and authorization functionality.
- `Helpers/` — Utility classes and common helpers.
- `context/` — Project specification and documentation files only.

## Storage Model

- **Microsoft SQL Server** — Main persistent storage for users, roles, competitions, teams, players, matches, results, events, and statistics.
- **Entity Framework Core** — ORM used for database access and object mapping.
- **ASP.NET Core Identity** — Storage for authentication data, users, roles, claims, and login information.
- **wwwroot/** — Static files such as CSS, JavaScript, images, and client-side libraries.

## Auth and Access Model

- Authenticated users sign in using ASP.NET Core Identity.
- Users can authenticate using either email/password credentials or Google OAuth.
- ASP.NET Core Identity manages user accounts, roles, and authentication sessions.
- Role-based authorization is used for Admin, Referee, and Coach.
- Guests can access only public pages without signing in.
- Admin has access to system management, users, roles, competitions, teams, matches, and statistics.
- Referee can only access matches assigned to them.
- Coach can only access teams assigned to them.
- Guests can only view public standings, brackets, schedules, results, teams, and player statistics.
- Protected controller actions are guarded using authorization attributes.
- Users can edit only their own profile information unless they are Admin.

## Invariants

1. Controllers must not contain business logic.
2. Views must not access the database directly.
3. Database access must go through Services or Repositories.
4. DTOs are used for data exchange between layers.
5. ViewModels are used for data displayed in Razor Views.
6. Unauthenticated users can never access protected pages.
7. A Referee can only enter results and statistics for matches assigned to them.
8. A Coach can only manage and view data for their assigned team.
9. Guests can never create, modify, or delete any data.
10. Confirmed match results must not be changed without Admin permission.
11. The tournament bracket must update only after a match result is confirmed.
12. No user can edit another user's profile unless they are Admin.
13. Entity models should not be used directly in Views when a ViewModel is required.
