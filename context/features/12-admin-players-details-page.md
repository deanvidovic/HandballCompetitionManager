# Admin Player Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Player Details page available at `/Admin/Players/Details/{id}`.

This page is opened when an Admin clicks **View** from `/Admin/Players`.

It should provide a complete management overview of a player, including personal information, team assignment, tournament participation, statistics, disciplinary record, and recent matches.

Use mock repository data only.

Do not:

- Implement authentication or authorization.
- Require the user to be signed in.
- Add `[Authorize]` attributes.
- Create or modify model classes.
- Create EF Core migrations or access the database.
- Implement Create/Edit/Delete functionality.
- Use Bootstrap or any other CSS framework.
- Use JavaScript or AJAX.

---

## Requirements

- Reuse the existing Admin layout.
- Use dependency injection.
- Use mock repositories only.
- No hardcoded data inside Controllers or Razor Views.
- Page must be accessible directly through `/Admin/Players/Details/{id}`.

---

## Data Source Requirement

Required repositories:

- `IPlayerRepository`
- `ITeamRepository`
- `ITournamentRepository`
- `IMatchRepository`

Controller flow:

```text
AdminController
        ↓
IPlayerRepository
        ↓
MockPlayerRepository
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

- `wwwroot/css/admin-player-details.css`

Rules:

- No inline styles.
- Link CSS only from this page.
- Keep global styles in `wwwroot/css/site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Update `AdminController`.

Requirements:

- Add `PlayerDetails(int id)` action.
- Route: `/Admin/Players/Details/{id}`
- Load all data from repositories.
- Return `NotFound()` for invalid ids.

---

## 2. Admin Layout

Reuse the existing Admin layout.

Sidebar active item:

- Players

---

## 3. Breadcrumbs

Display:

```text
Admin → Players → Player Details
```

Links:

- Admin → `/Admin/Tournaments`
- Players → `/Admin/Players`
- Player Details → current page

---

## 4. Player Header

Display:

- Full name
- Jersey number
- Position
- Age
- Team
- Tournament
- Status (Active/Inactive)

Buttons (disabled):

- Edit Player
- Transfer Player

Secondary link:

- Back to Players

---

## 5. Summary Cards

Display:

- Goals
- Matches Played
- Yellow Cards
- Red Cards
- Two-Minute Suspensions
- Average Goals per Match

---

## 6. Team Information

Display:

- Team name
- Coach
- Tournament
- Team record

Link:

`/Admin/Teams/Details/{id}`

---

## 7. Tournament Statistics

Display a table with:

- Competition
- Matches
- Goals
- Assists (placeholder if model doesn't support it)
- Yellow Cards
- Red Cards
- Two-Minute Suspensions

---

## 8. Recent Matches

Display only matches this player participated in.

Columns:

- Date
- Opponent
- Phase
- Result
- Goals
- Cards
- Actions

Actions:

- View → `/Matches/Details/{id}`

---

## 9. Disciplinary Record

Display:

- Total Yellow Cards
- Total Red Cards
- Total Two-Minute Suspensions

Include a simple timeline/table of disciplinary events.

---

## 10. Performance Overview

Display compact statistic cards:

- Best Match
- Highest Goals in One Match
- Current Goal Ranking
- Fair Play Rating (placeholder)

---

## 11. Empty States

Prepare hidden/commented states for:

- No tournament statistics.
- No recent matches.
- No disciplinary events.

---

## Check When Done

- `/Admin/Players/Details/{id}` renders successfully.
- Accessible without sign-in.
- No authentication or authorization added.
- `AdminController` contains `PlayerDetails(int id)`.
- Data comes from repositories.
- No sample data in controllers or views.
- `wwwroot/css/admin-player-details.css` exists.
- Breadcrumbs exist.
- Player header exists.
- Summary cards exist.
- Team information exists.
- Tournament statistics table exists.
- Recent matches exist.
- Disciplinary record exists.
- Performance overview exists.
- Team link points to `/Admin/Teams/Details/{id}`.
- Match links point to `/Matches/Details/{id}`.
- Responsive below 768px.
- Design matches `ui-context.md`.
- No CRUD functionality implemented.
- `dotnet build` passes.
