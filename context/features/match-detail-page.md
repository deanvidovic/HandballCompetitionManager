# Match Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create the public match details page available at `/Matches/Details/{id}`.

This page is opened when a Guest clicks on a match from Tournament Details, Team Details, or Player Details pages.

It should provide a complete public overview of a single match, including match information, participating teams, result, match events, player performances, and tournament context.

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

Required repositories:

- `IMatchRepository`
- `ITeamRepository`
- `IPlayerRepository`
- `ITournamentRepository`

Requirements:

- Do not create sample data directly inside `MatchesController`.
- Do not create sample data directly inside `Views/Matches/Details.cshtml`.
- Retrieve all data through repository interfaces.
- Use dependency injection.
- Controllers must depend on repository interfaces only.

Controller flow:

```text
MatchesController
        ↓
IMatchRepository
        ↓
MockMatchRepository
```

Future replacement:

```text
MockMatchRepository
        ↓
MatchRepository (EF Core)
```

No Controller or View changes should be required when switching to EF Core.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/match-details.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `match-details.css` only from `Views/Matches/Details.cshtml`.
- Keep global styles only in `wwwroot/css/site.css`.
- Keep the page visually consistent with the existing home page, tournaments listing page, tournament details page, team details page, and player details page.

---

## 1. Controller and Route

Create or update `MatchesController`.

Requirements:

- Add public `Details(int id)` action.
- Route: `/Matches/Details/{id}`
- Return the match details view.
- Retrieve data from repositories.
- Do not create sample data inside the controller.

Invalid ids:

- Return `NotFound()`.

---

## 2. Breadcrumb Navigation

Display breadcrumbs near the top of the page.

Preferred structure:

```text
Home → Tournaments → Tournament Details → Match Details
```

Links:

- Home → `/`
- Tournaments → `/Tournaments`
- Tournament Details → `/Tournaments/Details/{tournamentId}`
- Match Details → current page

---

## 3. Match Hero Section

Create a large match overview header section.

Display:

- Home team
- Away team
- Final score if match is completed
- Match status
- Match date
- Match phase
- Venue if available
- Tournament name

Team links:

- Home team → `/Teams/Details/{id}`
- Away team → `/Teams/Details/{id}`

Add secondary navigation links:

- Back to Tournament → `/Tournaments/Details/{tournamentId}`
- Back to Tournaments → `/Tournaments`

All values must come from repositories.

---

## 4. Match Summary Stats

Create a statistics row.

Display:

- Total Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions

Use large numbers and smaller labels.

All values must come from repositories.

---

## 5. Team Comparison Section

Section title:

Team Comparison

Display side-by-side statistics for both teams.

Include:

- Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions

Design notes:

- Use two cards or a comparison table.
- The home team should appear on the left.
- The away team should appear on the right.
- Keep the layout responsive below 768px.

All values must come from repositories.

---

## 6. Match Events Timeline

Section title:

Match Events

Display a chronological timeline of match events.

Supported events:

- Goal
- Yellow Card
- Red Card
- Two-Minute Suspension

Each event should show:

- Minute
- Player name
- Team name
- Event type

Player links:

`/Players/Details/{id}`

Team links:

`/Teams/Details/{id}`

Requirements:

- Order events chronologically.
- Use clear visual grouping.
- Use muted text for secondary information.
- Do not use too many colors.
- Follow `ui-context.md`.

All values must come from repositories.

---

## 7. Player Performances Section

Section title:

Player Performances

Display players who participated in this match.

Each player row/card should contain:

- Player name
- Team
- Goals
- Yellow cards
- Red cards
- Two-minute suspensions

Player links:

`/Players/Details/{id}`

Design notes:

- Use a table or compact card list.
- Table is preferred on desktop.
- On mobile, the table may scroll horizontally or become stacked cards.

All values must come from repositories.

---

## 8. Top Performers Section

Section title:

Top Performers

Display three small statistic cards:

### Top Goal Scorer

Player with the most goals in this match.

### Most Disciplined Player

Player with no cards or suspensions, if available.

### Most Penalized Player

Player with the highest total number of disciplinary events.

Each card should show:

- Player name
- Team name
- Statistic value

Player links:

`/Players/Details/{id}`

All values must come from repositories.

---

## 9. Tournament Context Section

Section title:

Tournament Context

Display:

- Tournament name
- Tournament status
- Current phase

Link:

`/Tournaments/Details/{id}`

All values must come from repositories.

---

## 10. Related Teams Section

Section title:

Teams

Display both participating teams.

Each team card should show:

- Team name
- City
- Coach name

Links:

`/Teams/Details/{id}`

All values must come from repositories.

---

## 11. Empty / Future Data States

Add commented-out empty states for future use.

Examples:

- Match has not started yet.
- No match events available.
- No player performances available.
- Match statistics unavailable.

Keep these empty states commented or visually hidden for now.

---

## 12. Footer

Use the same footer style as the rest of the public pages.

Left:

Handball Competition Manager

Right:

Tournaments

---

## Check When Done

- `MatchesController.cs` contains a `Details(int id)` action.
- `/Matches/Details/{id}` renders successfully.
- Match data is loaded through repositories.
- No sample data exists inside Controllers.
- No sample data exists inside Razor Views.
- `Views/Matches/Details.cshtml` exists.
- `wwwroot/css/match-details.css` exists.
- `match-details.css` is linked only from the match details view.
- Breadcrumbs are present.
- Match hero section is present.
- Match summary stats are present.
- Team comparison section is present.
- Match events timeline is present.
- Player performances section is present.
- Top performers section is present.
- Tournament context section is present.
- Related teams section is present.
- Team links go to `/Teams/Details/{id}`.
- Player links go to `/Players/Details/{id}`.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- No authentication or authorization logic was implemented.
- `dotnet build` passes.
