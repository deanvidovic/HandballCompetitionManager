# Admin Match Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Match Details page available at `/Admin/Matches/Details/{id}`.

This page is opened when an Admin clicks **View** from `/Admin/Matches`.

It should provide a complete management overview of a single match, including match information, participating teams, assigned referee, tournament context, player performances, match events, statistics, and administrative status.

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

- Reuse the existing Admin layout from the other Admin pages.
- Use dependency injection.
- Use mock repositories only.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible through `/Admin/Matches/Details/{id}`.
- This is only a UI/design step.

---

## Data Source Requirement

Required repositories:

- `IMatchRepository`
- `ITeamRepository`
- `IPlayerRepository`
- `ITournamentRepository`
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
- No sample data inside controllers.
- No sample data inside Razor views.

---

## Styling Requirement

Create:

- `wwwroot/css/admin-match-details.css`

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

- Add `MatchDetails(int id)` action.
- Route:

`/Admin/Matches/Details/{id}`

- Load all data from repositories.
- Return `NotFound()` for invalid ids.

---

## 2. Admin Layout

Reuse the existing Admin layout.

Sidebar active item:

- Matches

---

## 3. Breadcrumbs

Display:

```text
Admin → Matches → Match Details
```

Links:

- Admin → `/Admin/Tournaments`
- Matches → `/Admin/Matches`
- Match Details → current page

---

## 4. Match Header

Display:

- Home team
- Away team
- Current/Final score
- Tournament
- Phase
- Match status
- Date & time
- Venue (if available)
- Assigned referee

Buttons (disabled):

- Edit Match
- Assign Referee
- Confirm Result

Secondary link:

- Back to Matches

---

## 5. Match Summary Cards

Display:

- Total Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions
- Attendance (placeholder)
- Match Duration (placeholder)

Use repository data.

---

## 6. Team Comparison

Display a side-by-side comparison for both teams.

Include:

- Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions

Keep the layout responsive.

---

## 7. Match Events Timeline

Section title:

Match Events

Display events chronologically.

Supported events:

- Goal
- Yellow Card
- Red Card
- Two-Minute Suspension

Each event should include:

- Minute
- Player
- Team
- Event type

Links:

- Player → `/Admin/Players/Details/{id}`
- Team → `/Admin/Teams/Details/{id}`

---

## 8. Player Performances

Section title:

Player Performances

Create a responsive table.

Columns:

- Player
- Team
- Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions
- Actions

Actions:

- View → `/Admin/Players/Details/{id}`

No management functionality.

---

## 9. Match Administration Panel

Display a compact status panel.

Include:

- Referee Assigned
- Team Sheets Submitted
- Match Confirmed
- Statistics Complete

Use badges only.

Below the badges add disabled buttons:

- Edit Match
- Assign Referee
- Edit Result
- Reopen Match

Visual only.

---

## 10. Tournament Context

Display:

- Tournament name
- Tournament phase
- Tournament status

Link:

`/Admin/Tournaments/Details/{id}`

---

## 11. Related Teams

Display two compact team cards.

Each card includes:

- Team
- Coach
- City

Links:

- `/Admin/Teams/Details/{id}`

---

## 12. Empty States

Prepare hidden/commented states for:

- No match events.
- No player performances.
- Referee not assigned.
- Match not started.

---

## Check When Done

- `/Admin/Matches/Details/{id}` renders successfully.
- Accessible directly without sign-in.
- No authentication or authorization added.
- `AdminController` contains `MatchDetails(int id)`.
- Data comes from repositories.
- No sample data inside controllers.
- No sample data inside views.
- `wwwroot/css/admin-match-details.css` exists.
- Breadcrumbs exist.
- Match header exists.
- Summary cards exist.
- Team comparison exists.
- Match events timeline exists.
- Player performances table exists.
- Administration panel exists.
- Tournament context exists.
- Related team cards exist.
- Player links navigate to `/Admin/Players/Details/{id}`.
- Team links navigate to `/Admin/Teams/Details/{id}`.
- Tournament link navigates to `/Admin/Tournaments/Details/{id}`.
- Responsive below 768px.
- Design matches `ui-context.md`.
- No CRUD functionality implemented.
- `dotnet build` passes.
