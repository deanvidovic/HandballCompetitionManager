# Tournaments Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create the public tournaments listing page available at `/Tournaments`.

This page is the primary public entry point for Guest users. It should allow visitors to browse tournaments, discover active competitions, and navigate to tournament details.

Do not:

- Create or modify any model classes.
- Create Entity Framework Core migrations or touch the database.
- Implement authentication or authorization logic.
- Implement Create, Edit, Delete, or Admin functionality.
- Use Bootstrap or any other CSS framework.
- Implement AJAX or autocomplete logic.
- Connect the page to a database.

Use mock repository data for now.

---

## Data Source Requirement

This page must use mock repositories instead of hardcoded data inside Controllers or Razor Views.

Requirements:

- Do not create sample data directly inside `TournamentsController`.
- Do not create sample data directly inside `Views/Tournaments/Index.cshtml`.
- Retrieve tournament data through a mock repository.
- Use dependency injection.
- Controllers must depend on repository interfaces only.

Architecture:

Interfaces/
ITournamentRepository.cs

Services/Mock/
MockTournamentRepository.cs

Controller flow:

TournamentsController
↓
ITournamentRepository
↓
MockTournamentRepository

The mock repository should return realistic sample tournament data.

Future replacement:

MockTournamentRepository
↓
TournamentRepository (EF Core)

No controller or view changes should be required when switching to EF Core.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/tournaments.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `tournaments.css` only from `Views/Tournaments/Index.cshtml`.
- Keep global styles only in `wwwroot/css/site.css`.

---

## 1. Controller and Route

Create or update `TournamentsController`.

Requirements:

- Public `Index()` action.
- Route: `/Tournaments`
- Return the tournaments listing page.
- Retrieve data from `ITournamentRepository`.
- Do not create sample data inside the controller.

---

## 2. Breadcrumb Navigation

Display breadcrumbs near the top of the page.

Structure:

Home → Tournaments

Links:

- Home → `/`
- Tournaments → current page

---

## 3. Page Header

Eyebrow:

PUBLIC TOURNAMENTS

Headline:

Browse handball tournaments

Subheadline:

Explore active and upcoming competitions, follow match schedules, results, standings, and player statistics.

---

## 4. Search Section

Create a large, prominent search section directly below the page header.

Purpose:

This search field should filter the existing tournament cards shown in the tournament grid.

Search placeholder:

Search tournaments by name, location, or status...

Helper text:

Type to filter tournaments by name, location, or status.

Requirements:

- Search field should be visually prepared for filtering tournament cards.
- Future search functionality should update the existing tournament grid.
- Matching tournament cards should remain visible.
- Non-matching tournament cards should be hidden.
- Do not create an autocomplete dropdown.
- Do not create a separate search results section.
- Do not implement JavaScript search logic yet.
- Do not implement AJAX yet.

---

## 5. Quick Filters

Add static filter chips above the tournament grid.

Filters:

- All
- Upcoming
- Active
- Completed

Requirements:

- Visual only.
- No filtering logic.

---

## 6. Tournament Grid

Display tournaments as responsive cards.

Desktop:

- 3 cards per row.

Tablet:

- 2 cards per row.

Mobile:

- 1 card per row.

Each card should contain:

- Tournament name
- Status badge
- Location
- Start date
- Number of teams
- Number of matches
- Short description
- Primary action button

Button:

View Details

Link:

`/Tournaments/Details/{id}`

Design:

- Use dark surfaces from `ui-context.md`.
- Use hairline borders.
- Use muted secondary text.
- Use accent color only for primary actions.

---

## 7. Empty State

Create an empty-state component but keep it hidden or commented out.

Title:

No tournaments found

Description:

Try adjusting your search or check back later for new competitions.

---

## 8. Footer

Use the same footer style as the home page.

Left:

Handball Competition Manager

Right:

Tournaments

---

## Check When Done

- `ITournamentRepository` exists.
- `MockTournamentRepository` exists.
- Dependency injection is configured.
- `TournamentsController.cs` exists.
- `Index()` action exists.
- `/Tournaments` renders successfully.
- Tournament data comes from `ITournamentRepository`.
- No sample tournament data exists inside Controllers.
- No sample tournament data exists inside Razor Views.
- `Views/Tournaments/Index.cshtml` exists.
- `wwwroot/css/tournaments.css` exists.
- Search section is present.
- Breadcrumbs are present.
- Filter chips are present.
- Tournament cards are present.
- Footer is present.
- Page is responsive below 768px.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- No Create/Edit/Delete functionality was added.
- `dotnet build` passes.
