# Admin Tournament Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Tournament Details page available at `/Admin/Tournaments/Details/{id}`.

This page is opened when an Admin clicks `View` from `/Admin/Tournaments`.

It should show a management-focused overview of one tournament, including tournament metadata, teams, matches, bracket status, standings, and player statistics. This page is for reviewing and managing tournament progress, but no real Create/Edit/Delete actions should be implemented yet.

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

- Create an Admin Tournament Details page.
- Use the same dedicated admin-style layout from `/Admin/Tournaments`.
- Do not reuse the guest/public page layout as-is.
- Use mock repositories only.
- Use dependency injection.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible at `/Admin/Tournaments/Details/{id}` without authentication for now.
- This is only a UI/design step.

---

## Data Source Requirement

Use mock repositories only.

Required repositories:

- `ITournamentRepository`
- `ITeamRepository`
- `IMatchRepository`
- `IPlayerRepository`

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
- Mock data must live inside mock repositories.
- Razor Views must receive data from the controller.
- No database access.
- No EF Core queries.
- No sample data inside the controller.
- No sample data inside Razor views.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/admin-tournament-details.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `admin-tournament-details.css` only from the Admin tournament details page.
- Keep global styles only in `wwwroot/css/site.css`.
- Follow `ui-context.md`.
- Keep the page visually consistent with `/Admin/Tournaments`.

---

## 1. Controller and Route

Update `AdminController`.

Requirements:

- Add public `TournamentDetails(int id)` action.
- Route: `/Admin/Tournaments/Details/{id}`
- Return the Admin Tournament Details view.
- Retrieve tournament data from repositories.
- Do not create sample data inside the controller.
- Do not add authentication or authorization checks.

Invalid ids:

- Return `NotFound()`.

---

## 2. Admin Layout

Use the same dedicated admin layout style as `/Admin/Tournaments`.

The layout should include:

- Left sidebar.
- Top header.
- Main content area.

Sidebar active item:

- Tournaments

Do not create a completely new visual style.

---

## 3. Sidebar

Display:

- Dashboard (placeholder)
- Tournaments (active)
- Teams (placeholder)
- Matches (placeholder)
- Users (placeholder)
- Sign Out (placeholder)

Only `Tournaments` should be visually active.

Placeholder links may point to `#`.

---

## 4. Breadcrumb Navigation

Display breadcrumbs near the top of the content area.

Structure:

```text
Admin → Tournaments → Tournament Details
```

Links:

- Admin → `/Admin/Tournaments`
- Tournaments → `/Admin/Tournaments`
- Tournament Details → current page

---

## 5. Header Section

Create a management header section.

Display:

- Tournament name
- Status badge
- Location
- Start date
- End date
- Current phase
- Short description

Primary management actions:

- Edit Tournament (disabled)
- Generate Bracket (disabled)
- Export Report (disabled)

Secondary link:

- Back to Tournaments → `/Admin/Tournaments`

Do not implement action behavior.

---

## 6. Tournament Summary Cards

Display summary cards:

- Teams
- Matches
- Completed Matches
- Upcoming Matches
- Total Goals
- Registered Players

Use data from repositories.

---

## 7. Admin Status Panel

Create a compact status/progress panel.

Display:

- Tournament setup status
- Teams registered status
- Bracket generated status
- Results confirmation status
- Current phase progress

Example labels:

- Setup Complete
- Teams Registered
- Bracket Ready
- Results Pending
- Group Stage Active

All values must come from repositories or mock repository DTOs.

---

## 8. Teams Section

Section title:

Teams in Tournament

Display teams in a management table.

Columns:

- Team
- City
- Coach
- Players
- Record
- Goals For
- Goals Against
- Actions

Actions:

- View Public Page → `/Teams/Details/{id}`
- Manage (disabled)

Do not implement Manage functionality.

---

## 9. Matches Section

Section title:

Tournament Matches

Display all matches for this tournament.

Columns:

- Match
- Phase
- Date
- Status
- Score
- Referee
- Actions

Actions:

- View Public Page → `/Matches/Details/{id}`
- Manage Result (disabled)

Requirements:

- Show completed and upcoming matches.
- Group or label matches by phase if possible.
- Keep table readable and responsive.

---

## 10. Group Phase Section

Section title:

Group Phase Standings

Display group standings from repositories.

Each group table should include:

- Position
- Team
- Played
- Wins
- Draws
- Losses
- Goals For
- Goals Against
- Goal Difference
- Points

Design notes:

- Use compact admin table styling.
- Top qualifying teams should be visually emphasized.
- Mobile should scroll horizontally if needed.

---

## 11. Elimination Phase Section

Section title:

Elimination Phase

Display a bracket management preview.

Requirements:

- Show quarter-finals, semi-finals, and final if available.
- Show completed results when available.
- Show upcoming matches when available.
- Show winner progression when available.
- Add disabled action button: `Regenerate Bracket`

Do not implement bracket generation.

---

## 12. Player Statistics Section

Section title:

Tournament Leaders

Create four compact leaderboard cards:

- Top Scorers
- Most Two-Minute Suspensions
- Most Yellow Cards
- Most Red Cards

Each leaderboard should show top 3 players.

Each row should show:

- Rank
- Player name
- Team
- Statistic value

Player links:

`/Players/Details/{id}`

All values must come from repositories.

---

## 13. Admin Notes / Warnings Panel

Create a small management notes panel.

Display static/mock warnings such as:

- Some matches are still pending confirmation.
- Bracket updates after confirmed match results.
- Edit and delete actions are disabled in this design step.

This panel is informational only.

---

## 14. Empty / Future Data States

Prepare hidden/commented empty states for future use.

Examples:

- No teams assigned to this tournament.
- No matches generated yet.
- No bracket available.
- No statistics available.

Keep these empty states commented or visually hidden for now.

---

## Check When Done

- `AdminController.cs` contains a `TournamentDetails(int id)` action.
- `/Admin/Tournaments/Details/{id}` renders successfully.
- Page is accessible directly without sign-in.
- No authentication or authorization logic was added.
- No `[Authorize]` attributes were added.
- Tournament data is loaded from repositories.
- No sample data exists inside Controllers.
- No sample data exists inside Razor Views.
- `Views/Admin/TournamentDetails.cshtml` exists, or another clear Admin details view path exists.
- `wwwroot/css/admin-tournament-details.css` exists.
- CSS is linked only from the Admin tournament details page.
- Admin sidebar exists.
- Breadcrumbs are present.
- Header section is present.
- Summary cards are present.
- Status panel is present.
- Teams section is present.
- Matches section is present.
- Group phase section is present.
- Elimination phase section is present.
- Player statistics section is present.
- Admin notes/warnings panel is present.
- Public links point to `/Teams/Details/{id}`, `/Matches/Details/{id}`, and `/Players/Details/{id}`.
- Disabled management buttons are visible but do not perform actions.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- `dotnet build` passes.
