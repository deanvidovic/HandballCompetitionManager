# Admin Tournaments Dashboard

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Tournament Management page available at `/Admin/Tournaments`.

This page is only a visual/admin dashboard prototype for now. It should be accessible directly through the URL `/Admin/Tournaments`.

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

- Create a dedicated admin-style layout for this page.
- Do not reuse the guest/public page layout as-is.
- Use `ITournamentRepository`.
- Use dependency injection.
- Do not hardcode tournament data inside Controllers or Razor Views.
- Use mock repository data only.
- The page must be directly accessible at `/Admin/Tournaments` without authentication for now.
- This is only a UI/design step.

---

## Data Source Requirement

Use mock repositories only.

Required repository:

- `ITournamentRepository`

Controller flow:

```text
AdminController
        ↓
ITournamentRepository
        ↓
MockTournamentRepository
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

- `wwwroot/css/admin-tournaments.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `admin-tournaments.css` only from the Admin tournaments page.
- Keep global styles only in `wwwroot/css/site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Create or update `AdminController`.

Requirements:

- Add public `Tournaments()` action.
- Route: `/Admin/Tournaments`
- Return the Admin Tournament Management view.
- Retrieve tournament data from `ITournamentRepository`.
- Do not create sample data inside the controller.
- Do not add authentication or authorization checks.

---

## 2. Admin Tournament Details Navigation

The `View` action in the tournament table must navigate to the Admin Tournament Details page.

When the user clicks `View` on a tournament row, navigate to:

```text
/Admin/Tournaments/Details/{id}
```

Requirements:

- Use the tournament id from the mock repository data.
- The link must be generated for every tournament row.
- Do not point Admin View buttons to the public `/Tournaments/Details/{id}` page.
- Do not implement the details page in this task unless it already exists.
- The goal is only to make the `View` links target the correct future Admin details route.

Example:

```text
View → /Admin/Tournaments/Details/1
```

---

## 3. Admin Layout

Create a dedicated management layout for this page.

The layout should include:

- Left sidebar.
- Top header.
- Main content area.

Design goal:

The page should feel like a professional internal management dashboard, not a public guest page.

---

## 4. Sidebar

Display:

- Tournaments (active)
- Teams (placeholder)
- Matches (placeholder)
- Users (placeholder)
- Sign Out (placeholder)

Only `Tournaments` needs to be visually active.

Placeholder links may point to `#`.

Do not implement routing for the other sections yet.

---

## 5. Header

Title:

Tournament Management

Subtitle:

Manage competitions, schedules, and tournament progress.

Do not display notification or profile/avatar placeholders in this design-only step.

---

## 6. Summary Cards

Display four summary cards:

- Total Tournaments
- Active Tournaments
- Upcoming Tournaments
- Completed Tournaments

Use data from the mock repository.

---

## 7. Search Toolbar

Create a toolbar above the table.

Include:

- Search input

Do not implement search logic yet.

This is visual only.

---

## 8. Tournament Table

Create a responsive tournament management table.

Columns:

- Tournament
- Status
- Location
- Start
- End
- Teams
- Matches
- Actions

Actions:

- View → `/Admin/Tournaments/Details/{id}`
- Edit (disabled)
- Delete (disabled)

Do not implement Edit or Delete functionality in this task.

The View action must be a working link to the Admin tournament details route.

---

## 9. Create Tournament Action

Create a top-right action in the Tournaments section header.

Button:

- Create Tournament (disabled)

This button is visual only.

Do not implement the Create Tournament action.

Generate Bracket belongs on a future Admin Tournament Details page.

Export Report is not needed for this page.

---

## 10. Empty State

Prepare a hidden or commented empty state.

Title:

No tournaments found

Description:

Create your first tournament to get started.

---

## Check When Done

- `/Admin/Tournaments` renders successfully.
- Page is accessible directly through `/Admin/Tournaments` without sign-in.
- No authentication or authorization logic was added.
- No `[Authorize]` attributes were added.
- `AdminController` exists.
- `Tournaments()` action exists.
- Data is loaded from `ITournamentRepository`.
- Mock repository is used.
- No sample data exists inside Controllers.
- No sample data exists inside Razor Views.
- Dedicated admin layout exists.
- Sidebar exists.
- Sidebar does not include a Dashboard placeholder.
- Header exists.
- Header does not include notification or profile/avatar placeholders.
- Summary cards exist.
- Search toolbar exists.
- Tournament table exists.
- View links point to `/Admin/Tournaments/Details/{id}`.
- Clicking `View` from `/Admin/Tournaments` navigates to `/Admin/Tournaments/Details/{id}`.
- View links do not point to the public `/Tournaments/Details/{id}` route.
- Create Tournament button exists in the Tournaments section header.
- Quick actions panel does not exist on this page.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- `dotnet build` passes.
