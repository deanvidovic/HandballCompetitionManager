# Admin Players Dashboard

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Admin Players Management page at `/Admin/Players` using **mock repository data only**.

Do not implement authentication, authorization, CRUD functionality, EF Core, or database access.

## Requirements

- Reuse existing Admin layout.
- Sidebar active item: Players.
- Use `IPlayerRepository` via dependency injection.
- No hardcoded data in controllers or views.
- Accessible directly through `/Admin/Players`.

## Styling

Create `wwwroot/css/admin-players.css` and link it only from this page.

## Controller

Add `Players()` action to `AdminController`.

Route:
`/Admin/Players`

## Header

Title: Player Management

Subtitle:
Review player information, statistics and team assignments.

## Summary Cards

- Total Players
- Active Players
- Registered Teams
- Total Goals

## Search Toolbar

Search input only (no logic).

## Players Table

Columns:

- Player
- Team
- Position
- Number
- Goals
- Yellow Cards
- Red Cards
- Two-Minute Suspensions
- Actions

Actions:

- View → `/Admin/Players/Details/{id}`
- Edit (disabled)
- Delete (disabled)

## Create Action

Create Player (disabled).

## Check When Done

- `/Admin/Players` renders.
- Uses `IPlayerRepository`.
- Mock repository only.
- Sidebar marks Players active.
- Summary cards exist.
- Search exists.
- Players table exists.
- View links navigate to `/Admin/Players/Details/{id}`.
- Responsive.
- `dotnet build` passes.
