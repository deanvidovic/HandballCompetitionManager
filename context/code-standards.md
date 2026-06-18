# Code Standards

## General

- Keep classes small and single-purpose.
- Fix root causes, do not layer workarounds.
- Do not mix unrelated concerns in one Controller, Service, or View.
- Use clear and descriptive names for classes, methods, properties, and variables.
- Keep code readable and consistent across the project.

## C# / .NET

- Use strongly typed models, DTOs, and ViewModels.
- Avoid using `dynamic` unless absolutely necessary.
- Use nullable reference types where possible.
- Validate external input before using it.
- Use async/await for database and service operations.
- Do not ignore exceptions silently.
- Use dependency injection instead of manually creating service instances.

## ASP.NET Core MVC

- Controllers should only handle HTTP requests and coordinate application flow.
- Controllers must not contain business logic.
- Business logic belongs in Services.
- Database access belongs in Repositories or the Data layer.
- Views should only render UI and should not contain business logic.
- Use ViewModels for data displayed in Razor Views.
- Use DTOs for data transfer between layers.
- Use ModelState validation for form submissions.
- Protect actions using `[Authorize]` and role-based authorization attributes.
- Use `[ValidateAntiForgeryToken]` on POST actions.

## Entity Framework Core

- Use Entity Framework Core for database access.
- Keep database entities in the `Models/` folder.
- Keep `DbContext` and EF configuration in the `Data/` folder.
- Use migrations for database schema changes.
- Do not query the database directly from Controllers or Views.
- Use LINQ queries clearly and avoid unnecessary eager loading.
- Use `Include` only when related data is needed.

## Authentication and Authorization

- Use ASP.NET Core Identity for authentication.
- Support email/password login and Google OAuth login.
- Use role-based authorization for Admin, Referee, and Coach.
- Guests should only access public pages.
- Users may edit only their own profile unless they are Admin.
- Referees may access only matches assigned to them.
- Coaches may access only teams assigned to them.

## Styling

- Avoid inline styles when possible.
- Keep custom CSS in `wwwroot/css/`.
- Keep custom JavaScript in `wwwroot/js/`.
- Do not modify third-party library files in `wwwroot/lib/`.
- Keep UI consistent across all pages.

## Services

- Put business rules and application logic in `Services/`.
- Services should communicate with the Data layer, not with Views.
- Services should return DTOs or ViewModels where appropriate.
- Services should handle validation that is not part of simple form validation.
- Keep service methods focused on one use case.

## Data and Storage

- Use Microsoft SQL Server as the main database.
- Use Entity Framework Core for persistence.
- Store users and roles using ASP.NET Core Identity tables.
- Do not store sensitive data in plain text.
- Use configuration files or user secrets for connection strings and external login secrets.
- Do not hardcode sensitive values in source code.

## File Organization

- `Controllers/` — MVC controllers and request handling.
- `Models/` — Domain entities and database models.
- `DTOs/` — Data Transfer Objects.
- `ViewModels/` — Models used by Razor Views.
- `Views/` — Razor views and UI rendering.
- `Services/` — Business logic and use case logic.
- `Interfaces/` — Service and repository contracts.
- `Data/` — DbContext, repositories, and EF Core configuration.
- `Migrations/` — Entity Framework Core migration files.
- `wwwroot/` — Static files such as CSS, JavaScript, images, and libraries.
- `Areas/Identity/` — Identity pages and authentication UI.
- `Helpers/` — Utility classes and helper methods.
- `context/` — Project specification and documentation files.
