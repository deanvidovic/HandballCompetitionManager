# Team Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create the public team details page available at `/Teams/Details/{id}`.

This page is opened when a Guest clicks on a team card from the tournament details page. It should show a complete public overview of one team within a specific tournament, including team information, roster, tournament performance, tournament matches, and player statistics leaders for that team.

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

## Data Source Requirement

- Use `ITeamRepository`, `IPlayerRepository`, `IMatchRepository`, and `ITournamentRepository`.
- Do not place sample data inside controllers or Razor views.
- Use dependency injection.
- Controllers must depend on repository interfaces only.

## Styling Requirement

Create:

- `wwwroot/css/team-details.css`

Rules:

- No inline styles.
- Link CSS only from `Views/Teams/Details.cshtml`.
- Keep global styles in `site.css`.
- Follow `ui-context.md`.

## Controller and Route

Create or update `TeamsController`.

Requirements:

- Add `Details(int id)` action.
- Route: `/Teams/Details/{id}`.
- Load data through repositories.
- Return `NotFound()` for invalid ids.

## Breadcrumb Navigation

Home → Tournaments → Tournament Details → Team Details

## Team Hero Summary

Display:

- Team name
- City
- Coach name
- Number of players
- Current tournament name
- Team record
- Goals scored
- Goals conceded
- Goal difference
- Team description

Links:

- Back to Tournament
- Back to Tournaments

## Team Overview Stats

Display:

- Matches Played
- Wins
- Draws
- Losses
- Goals Scored
- Goals Conceded

## Team Roster Section

Display:

- Player name
- Shirt number
- Position
- Goals
- Yellow cards
- Red cards
- Two-minute suspensions

Optional link:

`/Players/Details/{id}`

## Tournament Matches Section

Display matches from the CURRENT tournament involving this team only.

Requirements:

- Do not show matches from other tournaments.
- Show completed and upcoming matches.
- Order chronologically.

Each match should show:

- Home team
- Away team
- Score
- Match phase
- Match status
- Date

Link:

`/Matches/Details/{id}`

## Team Statistics Leaders

Create four leaderboards:

- Top Scorers
- Most Two-Minute Suspensions
- Most Yellow Cards
- Most Red Cards

Show top 3 players for each category.

## Tournament Context Section

Display:

- Tournament name
- Tournament status
- Current phase
- Team group

Link:

`/Tournaments/Details/{id}`

## Empty States

Prepare hidden/commented states:

- No players registered.
- No tournament matches available.
- No statistics available.
- Team not assigned to tournament.

## Footer

Left:

Handball Competition Manager

Right:

Tournaments

## Check When Done

- `TeamsController.cs` contains `Details(int id)`.
- `/Teams/Details/{id}` renders successfully.
- Data is loaded through repositories.
- No sample data exists in controllers.
- No sample data exists in Razor views.
- `Views/Teams/Details.cshtml` exists.
- `wwwroot/css/team-details.css` exists.
- Team roster section exists.
- Tournament matches section exists.
- Only matches from selected tournament are displayed.
- Team leaderboards exist.
- Responsive below 768px.
- `dotnet build` passes.
