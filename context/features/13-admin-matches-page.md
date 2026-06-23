# Admin Matches Dashboard

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Match Management page available at `/Admin/Matches`.

This page is the central place where an Administrator can review all tournament matches, monitor their status, and navigate to individual match details.

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

---

## Requirements

- Reuse the existing Admin layout from other Admin pages.
- Do not reuse the Guest layout.
- Use dependency injection.
- Use `IMatchRepository`.
- Use mock repository data only.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible through `/Admin/Matches`.
- This is a UI/design step only.

---

## Data Source Requirement

Required repositories:

- `IMatchRepository`

Optional repositories:

- `ITournamentRepository`
- `ITeamRepository`
- `IUserRepository`

Controller flow:

```text
AdminController
        ↓
IMatchRepository
        ↓
MockMatchRepository
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

- `wwwroot/css/admin-matches.css`

Rules:

- No inline styles.
- Link CSS only from this page.
- Keep global styles in `wwwroot/css/site.css`.
- Follow `ui-context.md`.
- Keep the design consistent with all existing Admin pages.

---

## 1. Controller and Route

Update `AdminController`.

Requirements:

- Add `Matches()` action.
- Route:

`/Admin/Matches`

- Retrieve all data from repositories.
- Do not create sample data inside the controller.
- Do not implement authentication or authorization.

---

## 2. Match Details Navigation

The `View` action in the table must navigate to:

```text
/Admin/Matches/Details/{id}
```

Requirements:

- Generate a View link for every match.
- Do not point to the public `/Matches/Details/{id}` page.
- Do not implement the details page in this task.

---

## 3. Admin Layout

Reuse the existing Admin layout.

Sidebar active item:

- Matches

Sidebar:

- Tournaments
- Teams
- Players
- Matches (active)
- Users
- Sign Out

---

## 4. Header

Title:

Match Management

Subtitle:

Manage tournament matches, referees, schedules, and match progress.

---

## 5. Summary Cards

Display:

- Total Matches
- Upcoming Matches
- In Progress Matches
- Completed Matches

Use repository data.

---

## 6. Search & Filter Toolbar

Create a management toolbar.

Include:

- Search input
- Tournament filter
- Match status filter
- Match phase filter

Placeholder text:

Search by team, tournament, referee...

Do not implement filtering logic.

This is visual only.

---

## 7. Matches Table

Create a responsive management table.

Columns:

- Match
- Tournament
- Phase
- Date
- Referee
- Status
- Score
- Actions

Actions:

- View → `/Admin/Matches/Details/{id}`
- Edit (disabled)
- Delete (disabled)

Requirements:

- Clearly distinguish completed and upcoming matches.
- Use status badges.
- Keep the table responsive.

Do not implement Edit or Delete.

---

## 8. Create Match Action

Place a button in the page header.

Button:

- Create Match (disabled)

Visual only.

---

## 9. Scheduling Overview

Create a compact section below the table.

Display:

- Upcoming matches today
- Upcoming matches this week
- Matches without assigned referee
- Completed matches awaiting confirmation

Use summary cards or small panels.

Data comes from repositories.

---

## 10. Empty State

Prepare hidden/commented state.

Title:

No matches found

Description:

Matches will appear here once tournaments generate fixtures.

---

## Check When Done

- `/Admin/Matches` renders successfully.
- Accessible directly without sign-in.
- No authentication or authorization added.
- `AdminController` contains `Matches()`.
- Data comes from `IMatchRepository`.
- Mock repository is used.
- No sample data exists in controllers.
- No sample data exists in views.
- Dedicated admin layout exists.
- Sidebar marks Matches as active.
- Header exists.
- Summary cards exist.
- Search and filter toolbar exists.
- Matches table exists.
- View links navigate to `/Admin/Matches/Details/{id}`.
- Create Match button exists.
- Scheduling overview exists.
- Responsive below 768px.
- Design matches `ui-context.md`.
- No CRUD functionality implemented.
- `dotnet build` passes.
