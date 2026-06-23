# Admin Users Dashboard

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin User Management page available at `/Admin/Users`.

This page is the central place where an Administrator can review all users, manage roles, and monitor account status.

Use mock repository data only.

Do not:

- Implement authentication or authorization.
- Add `[Authorize]` attributes.
- Create or modify model classes.
- Create EF Core migrations or access the database.
- Implement Create/Edit/Delete functionality.
- Use Bootstrap or another CSS framework.

---

## Requirements

- Reuse the existing Admin layout.
- Use dependency injection.
- Use `IUserRepository`.
- Use mock repositories only.
- No hardcoded data inside Controllers or Razor Views.
- Accessible directly through `/Admin/Users`.
- UI/design only.

---

## Data Source Requirement

Required repository:

- `IUserRepository`

Controller flow:

```text
AdminController
        ↓
IUserRepository
        ↓
MockUserRepository
```

Requirements:

- Controllers depend only on interfaces.
- Mock data lives inside repositories.
- No EF Core or database access.
- No sample data inside controllers or views.

---

## Styling Requirement

Create:

- `wwwroot/css/admin-users.css`

Rules:

- Link CSS only from this page.
- No inline styles.
- Keep global styles in `site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Update `AdminController`.

Requirements:

- Add `Users()` action.
- Route: `/Admin/Users`
- Load users from `IUserRepository`.
- No authentication logic.

---

## 2. User Details Navigation

The **View** action must navigate to:

`/Admin/Users/Details/{id}`

Do not implement the details page in this task.

---

## 3. Admin Layout

Reuse the existing Admin layout.

Sidebar:

- Tournaments
- Teams
- Players
- Matches
- Users (active)
- Sign Out

---

## 4. Header

Title:

User Management

Subtitle:

Manage user accounts, roles, and assignments.

---

## 5. Summary Cards

Display:

- Total Users
- Administrators
- Coaches
- Referees

Use repository data.

---

## 6. Search & Filter Toolbar

Include:

- Search input

Placeholder:

Search by name, email, or role...

Visual only.

---

## 7. Users Table

Columns:

- Name
- Email
- Role
- Status
- Assigned Team
- Assigned Matches
- Last Login (placeholder)
- Actions

Actions:

- View → `/Admin/Users/Details/{id}`
- Edit (disabled)
- Delete (disabled)

Display colored role badges for Admin, Coach, and Referee.

---

## 8. Create User Action

Place a button in the page header.

Button:

- Create User (disabled)

Visual only.

---

## 9. Role Distribution Panel

Display a small chart-like card or summary showing:

- Admins
- Coaches
- Referees

Use mock repository counts.

---

## 10. Empty State

Hidden/commented state:

Title:

No users found

Description:

Users will appear here once accounts are created.

---

## Check When Done

- `/Admin/Users` renders successfully.
- Accessible directly without sign-in.
- No authentication or authorization added.
- `AdminController` contains `Users()`.
- Data comes from `IUserRepository`.
- Mock repository is used.
- No sample data inside controllers or views.
- Sidebar marks Users as active.
- Header exists.
- Summary cards exist.
- Full-width search toolbar exists.
- Users table exists.
- View links navigate to `/Admin/Users/Details/{id}`.
- Create User button exists.
- Role distribution panel exists.
- Responsive below 768px.
- Design matches `ui-context.md`.
- No CRUD functionality implemented.
- `dotnet build` passes.
