# Admin Team Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Team Details page available at `/Admin/Teams/Details/{id}`.

This page is opened when an Admin clicks `View` from `/Admin/Teams`.

It should provide a complete management overview of a team, including team information, coach, tournament participation, roster, recent matches, and team statistics.

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

- Use the same dedicated Admin layout from `/Admin/Tournaments` and `/Admin/Teams`.
- Use dependency injection.
- Use mock repositories only.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible through `/Admin/Teams/Details/{id}`.
- This is a design/UI step only.

---

## Data Source Requirement

Required repositories:

- `ITeamRepository`
- `IPlayerRepository`
- `IMatchRepository`
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

- Controllers depend only on interfaces.
- Mock data lives inside repositories.
- No EF Core.
- No database access.
- No sample data inside controllers or views.

---

## Styling Requirement

Create:

- `wwwroot/css/admin-team-details.css`

Rules:

- No inline styles.
- Link CSS only from this page.
- Keep global styles in `wwwroot/css/site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Update `AdminController`.

Requirements:

- Add `TeamDetails(int id)` action.
- Route:

`/Admin/Teams/Details/{id}`

- Load all data from repositories.
- Return `NotFound()` for invalid ids.

---

## 2. Admin Layout

Reuse the same admin layout.

Sidebar active item:

- Teams

---

## 3. Breadcrumbs

Display:

```text
Admin → Teams → Team Details
```

Links:

- Admin → `/Admin/Tournaments`
- Teams → `/Admin/Teams`
- Team Details → current page

---

## 4. Team Header

Display:

- Team name
- City
- Coach
- Tournament
- Tournament status
- Team record
- Goals scored
- Goals conceded

Buttons (disabled):

- Edit Team
- Manage Players

Secondary link:

- Back to Teams

---

## 5. Team Summary Cards

Display:

- Players
- Matches Played
- Wins
- Losses
- Draws
- Goals Scored

Use repository data.

---

## 6. Players Section

Section title:

Players

Create a responsive management table.

Columns:

- Number
- Player
- Position
- Age
- Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions
- Actions

Actions:

- View → `/Players/Details/{id}`
- Manage (disabled)

Do not implement management functionality.

---

## 7. Tournament Information

Section title:

Tournament

Display:

- Tournament name
- Phase
- Status
- Number of teams
- Start date
- End date

Link:

`/Admin/Tournaments/Details/{id}`

---

## 8. Recent Matches

Section title:

Tournament Matches

Display only matches played by this team in the selected tournament.

Columns:

- Opponent
- Phase
- Date
- Result
- Status
- Actions

Actions:

- View → `/Matches/Details/{id}`

Do not create an Admin Match Details page in this task.

---

## 9. Team Statistics

Display statistic cards:

- Top Scorer
- Most Yellow Cards
- Most Red Cards
- Most Two-Minute Suspensions

Each card shows:

- Player
- Statistic value

Player links:

`/Players/Details/{id}`

---

## 10. Team Management Status

Create a small status panel.

Display:

- Coach Assigned
- Players Registered
- Tournament Assigned
- Team Ready

Use badges only.

---

## 11. Empty States

Prepare hidden/commented states:

- No players.
- No matches.
- Team not assigned to tournament.
- No statistics.

---

## Check When Done

- `/Admin/Teams/Details/{id}` renders successfully.
- Page is accessible without sign-in.
- No authentication or authorization added.
- `AdminController` contains `TeamDetails(int id)`.
- Data comes from repositories.
- No sample data inside controllers.
- No sample data inside views.
- `Views/Admin/TeamDetails.cshtml` exists.
- `wwwroot/css/admin-team-details.css` exists.
- Breadcrumbs exist.
- Team header exists.
- Summary cards exist.
- Players table exists.
- Tournament information exists.
- Recent matches exist.
- Statistics cards exist.
- Management status panel exists.
- Links to `/Admin/Tournaments/Details/{id}` work.
- Links to `/Players/Details/{id}` work.
- Links to `/Matches/Details/{id}` work.
- Responsive below 768px.
- Design matches `ui-context.md`.
- No model classes modified.
- No migrations created.
- No database changes.
- No CRUD functionality implemented.
- `dotnet build` passes.
