# Player Details Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create the public player details page available at `/Players/Details/{id}`.

This page is opened when a Guest clicks on a player from a team roster or statistics leaderboard. It should show a complete public overview of one player, including personal information, tournament statistics, match performance, and disciplinary records.

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

This page must use repositories only.

Required repositories:

- `IPlayerRepository`
- `ITeamRepository`
- `IMatchRepository`
- `ITournamentRepository`

Requirements:

- No sample data inside Controllers.
- No sample data inside Razor Views.
- Use dependency injection.
- Controllers depend on interfaces only.

## Styling Requirement

Create:

- `wwwroot/css/player-details.css`

Rules:

- No inline styles.
- Link CSS only from `Views/Players/Details.cshtml`.
- Keep global styles in `site.css`.
- Follow `ui-context.md`.

## Controller and Route

Create or update `PlayersController`.

Requirements:

- Add `Details(int id)` action.
- Route: `/Players/Details/{id}`.
- Load data through repositories.
- Return `NotFound()` for invalid ids.

## Breadcrumb Navigation

Structure:

Home → Tournaments → Tournament Details → Team Details → Player Details

Links:

- Home → `/`
- Tournaments → `/Tournaments`
- Tournament Details → `/Tournaments/Details/{tournamentId}`
- Team Details → `/Teams/Details/{teamId}`
- Player Details → current page

## Player Hero Section

Display:

- Full name
- Shirt number
- Position
- Team name
- Current tournament
- Age (if available)
- Short player description

Links:

- Back to Team
- Back to Tournament

All values come from repositories.

## Player Overview Stats

Display:

- Goals
- Assists
- Matches Played
- Yellow Cards
- Red Cards
- Two-Minute Suspensions

Use large stat cards.

## Tournament Performance Section

Section title:

Tournament Performance

Display statistics for the current tournament only.

Include:

- Goals scored
- Matches played
- Average goals per match
- Yellow cards
- Red cards
- Two-minute suspensions

## Match Performance History

Section title:

Tournament Match Performance

Display only matches from the currently selected tournament.

Each row should show:

- Match date
- Opponent
- Goals scored
- Yellow cards
- Red cards
- Two-minute suspensions

Link each match:

`/Matches/Details/{id}`

Do not implement Match Details page in this task.

## Rankings Section

Section title:

Tournament Rankings

Show player's rank within the tournament.

Display:

- Goal scorer ranking
- Yellow card ranking
- Red card ranking
- Two-minute suspension ranking

Use leaderboard style cards.

## Team Context Section

Section title:

Current Team

Display:

- Team name
- Coach
- Team record
- Tournament position

Link:

`/Teams/Details/{id}`

## Tournament Context Section

Section title:

Current Tournament

Display:

- Tournament name
- Status
- Current phase

Link:

`/Tournaments/Details/{id}`

## Empty States

Prepare hidden/commented states:

- No tournament statistics available.
- No matches played.
- No disciplinary records.
- Player not assigned to a team.

## Footer

Left:

Handball Competition Manager

Right:

Tournaments

## Check When Done

- `PlayersController.cs` contains `Details(int id)`.
- `/Players/Details/{id}` renders successfully.
- Data is loaded through repositories.
- No sample data exists in controllers.
- No sample data exists in Razor views.
- `Views/Players/Details.cshtml` exists.
- `wwwroot/css/player-details.css` exists.
- Breadcrumbs are present.
- Player hero section is present.
- Player overview stats are present.
- Tournament performance section is present.
- Tournament match performance section is present.
- Rankings section is present.
- Team context section is present.
- Tournament context section is present.
- Responsive below 768px.
- Design matches `ui-context.md`.
- `dotnet build` passes.
