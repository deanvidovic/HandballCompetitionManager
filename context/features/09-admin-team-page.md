# Admin Teams Dashboard

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Teams Management page available at `/Admin/Teams`.

This page is an admin dashboard prototype for managing and reviewing teams. It should be accessible directly through the URL `/Admin/Teams`.

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

- Create a dedicated admin-style Teams page.
- Reuse the same admin layout style from `/Admin/Tournaments`.
- Do not reuse the guest/public page layout as-is.
- Use `ITeamRepository`.
- Use dependency injection.
- Do not hardcode team data inside Controllers or Razor Views.
- Use mock repository data only.
- The page must be directly accessible at `/Admin/Teams` without authentication for now.
- This is only a UI/design step.

---

## Data Source Requirement

Use mock repositories only.

Required repository:

- `ITeamRepository`

Optional repositories if needed for summary counts:

- `IPlayerRepository`
- `ITournamentRepository`

Controller flow:

```text
AdminController
        ↓
ITeamRepository
        ↓
MockTeamRepository
```

Requirements:

- Controllers must depend on repository interfaces only.
- Mock data must live inside the mock repository.
- Razor Views must receive data from the controller.
- No database access.
- No EF Core queries.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/admin-teams.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `admin-teams.css` only from the Admin teams page.
- Keep global styles only in `wwwroot/css/site.css`.
- Follow `ui-context.md`.
- Keep the design consistent with `/Admin/Tournaments`.

---

## 1. Controller and Route

Create or update `AdminController`.

Requirements:

- Add public `Teams()` action.
- Route: `/Admin/Teams`
- Return the Admin Teams Management view.
- Retrieve team data from `ITeamRepository`.
- Do not create sample data inside the controller.
- Do not add authentication or authorization checks.

---

## 2. Admin Team Details Navigation

The `View` action in the teams table must navigate to the Admin Team Details page.

When the user clicks `View` on a team row, navigate to:

```text
/Admin/Teams/Details/{id}
```

Requirements:

- Use the team id from the mock repository data.
- The link must be generated for every team row.
- Do not point Admin View buttons to the public `/Teams/Details/{id}` page.
- Do not implement the details page in this task unless it already exists.
- The goal is only to make the `View` links target the correct future Admin details route.

---

## 3. Admin Layout

Use the same dedicated management layout style as `/Admin/Tournaments`.

The layout should include:

- Left sidebar.
- Top header.
- Main content area.

Sidebar active item:

- Teams

Design goal:

The page should feel like a professional internal management dashboard, not a public guest page.

---

## 4. Sidebar

Display:

- Tournaments
- Teams (active)
- Matches (placeholder)
- Users (placeholder)
- Sign Out (placeholder)

Only `Teams` needs to be visually active.

Links:

- Tournaments → `/Admin/Tournaments`
- Teams → `/Admin/Teams`
- Other placeholder links may point to `#`.

Do not implement routing for the other sections yet.

---

## 5. Header

Title:

Team Management

Subtitle:

Review teams, coaches, rosters, and tournament participation.

Do not display notification or profile/avatar placeholders in this design-only step.

---

## 6. Summary Cards

Display four summary cards:

- Total Teams
- Active Teams
- Total Players
- Teams in Tournaments

Use data from the mock repository or related mock repositories.

---

## 7. Search Toolbar

Create a toolbar above the table.

Include:

- Search input

Placeholder:

Search teams by name, city, coach, or tournament...

Do not implement search logic yet.

This is visual only.

---

## 8. Teams Table

Create a responsive team management table.

Columns:

- Team
- City
- Coach
- Players
- Tournament
- Record
- Goals
- Actions

Actions:

- View → `/Admin/Teams/Details/{id}`
- Edit (disabled)
- Delete (disabled)

Do not implement Edit or Delete functionality in this task.

The View action must be a working link to the Admin team details route.

---

## 9. Create Team Action

Create a top-right action in the Teams section header.

Button:

- Create Team (disabled)

This button is visual only.

Do not implement the Create Team action.

---

## 10. Empty State

Prepare a hidden or commented empty state.

Title:

No teams found

Description:

Teams will appear here once they are added to tournaments.

---

## Check When Done

- `/Admin/Teams` renders successfully.
- Page is accessible directly through `/Admin/Teams` without sign-in.
- No authentication or authorization logic was added.
- No `[Authorize]` attributes were added.
- `AdminController` exists.
- `Teams()` action exists.
- Data is loaded from `ITeamRepository`.
- Mock repository is used.
- No sample data exists inside Controllers.
- No sample data exists inside Razor Views.
- Dedicated admin layout exists.
- Sidebar exists.
- Sidebar marks Teams as active.
- Header exists.
- Header does not include notification or profile/avatar placeholders.
- Summary cards exist.
- Search toolbar exists.
- Teams table exists.
- View links point to `/Admin/Teams/Details/{id}`.
- Clicking `View` from `/Admin/Teams` navigates to `/Admin/Teams/Details/{id}`.
- View links do not point to the public `/Teams/Details/{id}` route.
- Create Team button exists in the Teams section header.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- `dotnet build` passes.
