# Referee My Matches Dashboard

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Referee dashboard available at `/Referee/MyMatches`.

This page is the default Referee landing page. It should use the same overall dashboard style as the Admin pages, but the content and navigation must be focused on the Referee workflow.

The main purpose of this page is to show matches assigned to the Referee.

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

- Create a dedicated Referee dashboard layout.
- Keep the same design language as the Admin dashboard.
- Do not reuse the Guest/Public layout.
- Use dependency injection.
- Use mock repositories only.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible through `/Referee/MyMatches`.
- This is a UI/design step only.

---

## Data Source Requirement

Required repositories:

- `IMatchRepository`
- `ITournamentRepository`
- `ITeamRepository`
- `IUserRepository`

Controller flow:

```text
RefereeController
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
- No sample data inside controllers.
- No sample data inside views.

---

## Styling Requirement

Create:

- `wwwroot/css/referee-my-matches.css`

Rules:

- Do not use inline styles.
- Link CSS only from this page.
- Keep global styles in `wwwroot/css/site.css`.
- Follow `ui-context.md`.
- Keep the design visually consistent with the Admin dashboard, but adjust labels and navigation for Referee use.

---

## 1. Controller and Route

Create or update `RefereeController`.

Requirements:

- Add `MyMatches()` action.
- Route:

`/Referee/MyMatches`

- Load assigned match data from repositories.
- Do not create sample data inside the controller.
- Do not implement authentication or authorization logic.

---

## 2. Referee Layout

Create a dedicated Referee dashboard layout.

The layout should include:

- Left sidebar.
- Top header.
- Main content area.

Design goal:

The page should feel like an internal match management workspace for Referees.

---

## 3. Sidebar

Display:

- My Matches (active)
- Profile (placeholder)
- Sign Out (placeholder)

Only `My Matches` should be visually active.

Links:

- My Matches → `/Referee/MyMatches`
- Other placeholder links may point to `#`.

Do not implement routing for the other sections yet.

---

## 4. Header

Title:

My Matches

Subtitle:

Review assigned matches, prepare team sheets, and record match results.

Do not display notification or profile/avatar placeholders in this design-only step.

---

## 5. Summary Cards

Display four summary cards:

- Assigned Matches
- Today
- Upcoming
- Completed

Use data from mock repositories.

---

## 6. Match Status Sections

Do not display separate Today, Upcoming, or Completed match sections. Their counts remain available in the summary cards, and all fixtures remain available in the Assigned Matches table.

---

## 7. Assigned Matches Table

Create a responsive table for all assigned matches.

Columns:

- Match
- Tournament
- Phase
- Date
- Status
- Venue
- Actions

Actions:

- View → `/Referee/Matches/Details/{id}`
- Report → `/Referee/Matches/Report/{id}`

Do not implement the details or report pages in this task.

---

## 8. Match Cards

Do not display a separate priority or featured match cards section on this page.

---

## 9. Search Toolbar

Create a toolbar above the table.

Include:

- Search input

Placeholder:

Search by team, tournament, or match status...

Do not implement search logic.

This is visual only.

---

## 10. Workflow Help Panel

Do not display a Referee Workflow help panel on this page.

---

## 11. Empty State

Prepare hidden/commented empty state.

Title:

No assigned matches

Description:

Assigned matches will appear here when the schedule is published.

---

## Check When Done

- `/Referee/MyMatches` renders successfully.
- Page is accessible directly without sign-in.
- No authentication or authorization added.
- No `[Authorize]` attributes were added.
- `RefereeController` exists.
- `MyMatches()` action exists.
- Data comes from repositories.
- Mock repository is used.
- No sample data inside controllers.
- No sample data inside views.
- Dedicated Referee dashboard layout exists.
- Sidebar exists.
- Sidebar marks My Matches as active.
- Header exists.
- Summary cards exist.
- Separate Today, Upcoming, and Completed match sections are not displayed.
- Assigned matches table exists.
- Search toolbar exists.
- Search input spans the full assigned matches toolbar width.
- Status and tournament filters are not displayed.
- Priority match cards are not displayed.
- Referee Workflow help panel is not displayed.
- View links navigate to `/Referee/Matches/Details/{id}`.
- Report links navigate to `/Referee/Matches/Report/{id}`.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- `dotnet build` passes.
