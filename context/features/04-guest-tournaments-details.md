# Tournament Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create the public tournament details page available at `/Tournaments/Details/{id}`.

This page is for Guest users and should show a complete public overview of one tournament. It should include tournament information, participating teams, group phase preview, elimination phase preview, match results, and player statistics.

Use mock repository data only.

Do not:

- Create or modify any model classes.
- Create Entity Framework Core migrations or touch the database.
- Implement authentication or authorization logic.
- Implement Create, Edit, Delete, or Admin functionality.
- Use Bootstrap or any other CSS framework.
- Use JavaScript.
- Implement AJAX.
- Connect the page to the database.

---

## Data Source Requirement

This page must use mock repositories instead of hardcoded data inside Controllers or Razor Views.

Requirements:

- Do not create sample data directly inside `TournamentsController`.
- Do not create sample data directly inside `Views/Tournaments/Details.cshtml`.
- Retrieve all data through repository interfaces.
- Use dependency injection.
- Controllers must depend on repository interfaces only.

Required repositories:

- `ITournamentRepository`
- `ITeamRepository`
- `IPlayerRepository`
- `IMatchRepository`

Controller flow:

TournamentsController
↓
ITournamentRepository
↓
MockTournamentRepository

Future replacement:

MockTournamentRepository
↓
TournamentRepository (EF Core)

No Controller or View changes should be required when switching to EF Core.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/tournament-details.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `tournament-details.css` only from `Views/Tournaments/Details.cshtml`.
- Keep global styles only in `wwwroot/css/site.css`.
- Keep the page visually consistent with the existing home page and tournaments listing page.

---

## 1. Controller and Route

Update `TournamentsController`.

Requirements:

- Add public `Details(int id)` action.
- Route: `/Tournaments/Details/{id}`
- Return the tournament details view.
- Retrieve data from repositories.
- Do not create sample data inside the controller.

If an invalid id is provided, return a simple `NotFound()` result.

---

## 2. Breadcrumb Navigation

Display breadcrumbs near the top of the page.

Structure:

Home → Tournaments → Tournament Details

Links:

- Home → `/`
- Tournaments → `/Tournaments`
- Tournament Details → current page

---

## 3. Tournament Hero Summary

Create a large hero/details header section.

Display:

- Tournament name
- Status badge
- Location
- Start date
- End date
- Number of teams
- Number of matches
- Current phase
- Short description

Add a secondary link:

Back to Tournaments → `/Tournaments`

All values must come from repositories.

---

## 4. Participating Teams Section

Create a section showing teams competing in the tournament.

Section title:

Participating Teams

Display teams as compact cards or rows.

Each team should show:

- Team name
- City
- Coach name
- Number of players
- Team record in tournament
- Goals scored
- Goals conceded

Team links:

Each team card should be clickable and link to:

`/Teams/Details/{id}`

Do not implement the actual Team details page in this task.

All values must come from repositories.

---

## 5. Group Phase Section

Create a group phase preview section.

Section title:

Group Phase

Display all tournament groups from repositories.

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

Requirements:

- Tables must be readable on dark background.
- On mobile, tables should scroll horizontally or stack cleanly.
- Use muted text for secondary values.
- Use a subtle accent for the top positions.

All values must come from repositories.

---

## 6. Elimination Phase Section

Create an elimination phase preview section.

Section title:

Elimination Phase

Display elimination bracket data from repositories.

Requirements:

- Support quarter-finals, semi-finals, and final rounds.
- Show completed results when available.
- Show upcoming matches when available.
- Use dark cards connected visually with spacing or simple bracket columns.
- Keep the layout responsive.

All values must come from repositories.

---

## 7. Match Results Section

Create a section with recent or important tournament matches.

Section title:

Recent Results

Each match row/card should include:

- Home team
- Away team
- Score
- Match phase
- Match status
- Date

Each match should link to:

`/Matches/Details/{id}`

Do not implement the actual Match details page in this task.

All values must come from repositories.

---

## 8. Player Statistics Section

Create a public statistics section with four statistic cards.

Section title:

Player Statistics Leaders

Create four leaderboards:

### Top Scorers

Show top 3 players by goals.

### Most Red Cards

Show top 3 players.

### Most Two-Minute Suspensions

Show top 3 players.

### Most Yellow Cards

Show top 3 players.

Design notes:

- Use four cards in a responsive grid.
- Each statistic card should be easy to scan.
- Use ranking numbers clearly.
- Use muted text for team names.
- Do not use too many colors.
- Follow `ui-context.md`.

All values must come from repositories.

---

## 9. Empty / Future Data States

Add commented-out empty states for future use.

Examples:

- No teams registered yet.
- Group phase has not started yet.
- Elimination phase has not started yet.
- No player statistics available yet.

Keep these empty states commented or visually hidden for now.

---

## 10. Footer

Use the same footer style as the home page and tournaments listing page.

Left:

Handball Competition Manager

Right:

Tournaments

---

## Check When Done

- `TournamentsController.cs` contains a `Details(int id)` action.
- `/Tournaments/Details/{id}` renders successfully.
- Tournament data is loaded through repositories.
- No sample data exists inside Controllers.
- No sample data exists inside Razor Views.
- `Views/Tournaments/Details.cshtml` exists.
- `wwwroot/css/tournament-details.css` exists.
- `tournament-details.css` is linked only from the tournament details view.
- Breadcrumbs are present.
- Tournament hero summary is present.
- Participating teams section is present.
- Team cards link to `/Teams/Details/{id}`.
- Group phase section is present.
- Elimination phase section is present.
- Recent results section is present.
- Match rows link to `/Matches/Details/{id}`.
- Player statistics leaderboards are present:
    - Top scorers
    - Most two-minute suspensions
    - Most red cards
    - Most yellow cards
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- No authentication or authorization logic was implemented.
- `dotnet build` passes.
