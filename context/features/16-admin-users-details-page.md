# Admin User Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin User Details page available at `/Admin/Users/Details/{id}`.

This page is opened when an Administrator clicks **View** from `/Admin/Users`.

It should provide a complete overview of a user account, including profile information, assigned role, team or referee assignments, account status, activity summary, and related entities.

Use mock repository data only.

Do not:

- Implement authentication.
- Implement authorization.
- Require the user to be signed in.
- Add `[Authorize]` attributes.
- Redirect unauthenticated users.
- Create or modify any model classes.
- Create Entity Framework Core migrations or touch the database.
- Implement Create, Edit, Delete, or Admin functionality.
- Connect the page to the database.
- Use Bootstrap or any other CSS framework.
- Use JavaScript.
- Implement AJAX.

---

## Requirements

- Reuse the existing Admin layout.
- Use dependency injection.
- Use mock repositories only.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible through `/Admin/Users/Details/{id}`.
- This is a UI/design step only.

---

## Data Source Requirement

Required repositories:

- `IUserRepository`
- `ITeamRepository`
- `IMatchRepository`

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
- No EF Core.
- No database access.
- No sample data inside controllers or views.

---

## Styling Requirement

Create:

- `wwwroot/css/admin-user-details.css`

Rules:

- Do not use inline styles.
- Link CSS only from this page.
- Keep global styles in `wwwroot/css/site.css`.
- Follow `ui-context.md`.
- Keep the design consistent with all Admin pages.

---

## 1. Controller and Route

Update `AdminController`.

Requirements:

- Add `UserDetails(int id)` action.
- Route:

`/Admin/Users/Details/{id}`

- Load all data from repositories.
- Return `NotFound()` for invalid ids.

---

## 2. Admin Layout

Reuse the existing Admin layout.

Sidebar active item:

- Users

---

## 3. Breadcrumbs

Display:

```text
Admin → Users → User Details
```

Links:

- Admin → `/Admin/Tournaments`
- Users → `/Admin/Users`
- User Details → current page

---

## 4. User Profile Header

Display:

- Full Name
- Email Address
- Role
- Account Status
- Date Created (placeholder)
- Last Login (placeholder)

Display a role badge for:

- Administrator
- Coach
- Referee

Buttons (disabled):

- Edit User
- Change Role
- Disable Account

Secondary link:

- Back to Users

---

## 5. Summary Cards

Display:

- Assigned Teams
- Assigned Matches
- Account Status
- Role

Use repository data.

---

## 6. Profile Information

Display:

- First Name
- Last Name
- Email
- Phone (placeholder)
- Role
- Status

Read-only information.

---

## 7. Assignments Section

The content depends on the user's role.

### If Coach

Display:

- Assigned Team
- Tournament
- Number of Players

Link:

`/Admin/Teams/Details/{id}`

### If Referee

Display:

- Assigned Matches
- Upcoming Matches
- Completed Matches

Each match links to:

`/Admin/Matches/Details/{id}`

### If Administrator

Display:

- System Access Level
- Total Managed Tournaments (placeholder)

Use mock data.

---

## 8. Activity Overview

Display a simple timeline or table.

Examples:

- User created.
- Assigned to team.
- Assigned to match.
- Last sign in (placeholder).

Timeline is informational only.

---

## 9. Related Entities

Display quick navigation cards.

Possible cards:

- Assigned Team
- Assigned Tournament
- Assigned Matches

Only show relevant cards depending on role.

Links:

- `/Admin/Teams/Details/{id}`
- `/Admin/Tournaments/Details/{id}`
- `/Admin/Matches/Details/{id}`

---

## 10. Administration Panel

Display badges:

- Account Active
- Email Verified (placeholder)
- Profile Complete (placeholder)

Below the badges add disabled buttons:

- Edit User
- Reset Password
- Change Role
- Delete User

Visual only.

---

## 11. Empty States

Prepare hidden/commented states:

- No assigned team.
- No assigned matches.
- No recent activity.

---

## Check When Done

- `/Admin/Users/Details/{id}` renders successfully.
- Accessible directly without sign-in.
- No authentication or authorization added.
- `AdminController` contains `UserDetails(int id)`.
- Data comes from repositories.
- No sample data inside controllers.
- No sample data inside views.
- `wwwroot/css/admin-user-details.css` exists.
- Breadcrumbs exist.
- User profile header exists.
- Summary cards exist.
- Profile information exists.
- Assignments section exists.
- Activity overview exists.
- Related entities exist.
- Administration panel exists.
- Team links navigate to `/Admin/Teams/Details/{id}`.
- Match links navigate to `/Admin/Matches/Details/{id}`.
- Tournament links navigate to `/Admin/Tournaments/Details/{id}`.
- Responsive below 768px.
- Design matches `ui-context.md`.
- No CRUD functionality implemented.
- `dotnet build` passes.
