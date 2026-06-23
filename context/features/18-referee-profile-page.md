# Referee Profile

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Referee Profile page available at `/Referee/Profile`.

This page allows a Referee to view and manage their own account information. It should follow the same dashboard style as the Referee My Matches page.

Use mock repository data only.

Do not:

- Implement authentication.
- Implement authorization.
- Require the user to be signed in.
- Add `[Authorize]` attributes.
- Redirect unauthenticated users.
- Create or modify any model classes.
- Create Entity Framework Core migrations or touch the database.
- Implement save/update/delete functionality.
- Connect the page to the database.
- Use Bootstrap or any other CSS framework.
- Use JavaScript.
- Implement AJAX.

---

## Requirements

- Reuse the dedicated Referee dashboard layout.
- Keep the same design language as the Admin dashboard.
- Do not reuse the Guest/Public layout.
- Use dependency injection.
- Use mock repositories only.
- Do not hardcode data inside Controllers or Razor Views.
- The page must be directly accessible through `/Referee/Profile`.
- This is a UI/design step only.

---

## Data Source Requirement

Required repositories:

- `IUserRepository`

Optional repositories:

- `IMatchRepository`

Controller flow:

```text
RefereeController
        ↓
IUserRepository
        ↓
MockUserRepository
```

Requirements:

- Controllers depend only on interfaces.
- Mock data lives inside repositories.
- No EF Core.
- No database access.
- No sample data inside controllers.
- No sample data inside views.

---

## Styling Requirement

Create:

- `wwwroot/css/referee-profile.css`

Rules:

- Do not use inline styles.
- Link CSS only from this page.
- Keep global styles in `wwwroot/css/site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Create or update `RefereeController`.

Requirements:

- Add `Profile()` action.
- Route:

`/Referee/Profile`

- Load profile data from repositories.
- Do not implement authentication or authorization logic.
- Do not implement POST actions.

---

## 2. Referee Layout

Reuse the dedicated Referee dashboard layout.

Sidebar:

- My Matches
- Profile (active)
- Sign Out

Links:

- My Matches → `/Referee/MyMatches`
- Profile → `/Referee/Profile`

---

## 3. Header

Title:

My Profile

Subtitle:

View and manage your personal information.

---

## 4. Profile Information

Display an editable form with values prefilled from the mock repository.

Fields:

- First Name
- Last Name
- Email
- Phone Number
- City
- Referee Certification
- Short Biography

The form is visual only.

---

## 5. Account Information

Display read-only information:

- User ID
- Role
- Account Status
- Created Date
- Last Login

---

## 6. Assignment Summary

Display four summary cards:

- Assigned Matches
- Upcoming Matches
- Completed Matches
- Confirmed Reports

Use repository data.

---

## 7. Action Buttons

Display:

- Save Changes (disabled)
- Cancel (disabled)
- Delete Profile (disabled)

Do not implement functionality.

---

## 8. Empty State

Prepare hidden/commented state.

Title:

Profile unavailable

Description:

Profile information could not be loaded.

---

## Check When Done

- `/Referee/Profile` renders successfully.
- Page is accessible directly without sign-in.
- No authentication or authorization added.
- `RefereeController` exists.
- `Profile()` action exists.
- Data comes from repositories.
- Mock repository is used.
- No sample data inside controllers.
- No sample data inside views.
- Dedicated Referee layout exists.
- Sidebar marks Profile as active.
- Header exists.
- Editable profile form exists.
- Account information section exists.
- Assignment summary exists.
- Save, Cancel and Delete buttons exist and are disabled.
- No POST actions were created.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No model classes were modified.
- No migrations were created.
- No database changes were made.
- `dotnet build` passes.
