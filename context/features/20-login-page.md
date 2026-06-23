# Login Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Login page available at `/Account/Login`.

This page allows an existing user to sign in to the Handball Competition Manager.

The page is public and should follow the same visual style as the Home page, Guest pages, and Sign Up page.

Do not:

- Implement authentication.
- Implement login logic.
- Implement ASP.NET Core Identity functionality.
- Create or modify users in the database.
- Create or modify model classes.
- Create Entity Framework Core migrations or touch the database.
- Use Bootstrap or any other CSS framework.
- Use JavaScript.
- Implement AJAX.

---

## Requirements

- Use the public Guest layout.
- Keep the design language consistent with the Home page and Sign Up page.
- Do not hardcode business logic inside Controllers or Razor Views.
- The page must be directly accessible through `/Account/Login`.
- This is a UI/design step only.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/login.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `login.css` only from `Views/Account/Login.cshtml`.
- Keep global styles only in `wwwroot/css/site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Create or update `AccountController`.

Requirements:

- Add public `Login()` action.
- Route:

`/Account/Login`

- Return the Login view.
- Do not implement POST actions.
- Do not implement authentication or login logic.

---

## 2. Breadcrumbs

Display:

Home → Sign In

Links:

- Home → `/`
- Sign In → current page

---

## 3. Page Header

Title:

Sign in to your account

Subtitle:

Access your tournaments, matches, teams, and profile information.

---

## 4. Login Form

Create a centered login card.

Fields:

- Email Address
- Password

All fields should be visually editable.

Do not submit the form.

Do not implement validation.

Buttons:

- Sign In (disabled)

Below the button display:

Don't have an account?

Link:

Sign Up → `/Account/Register`

Also include a muted link:

Forgot password? → `#`

---

## 5. OAuth Section

Display a divider with text:

or

Below the divider create a secondary button:

Continue with Google

Requirements:

- Use a Google icon placeholder.
- Button is visual only.
- Do not implement Google OAuth.

---

## 6. Role Hint Section

Below the form, add a small muted information block.

Text:

Administrators, Coaches, and Referees can sign in to access their dashboards.

This is informational only.

---

## 7. Footer

Reuse the same footer style as the Home page.

Left:

Handball Competition Manager

Right:

Tournaments

---

## Check When Done

- `/Account/Login` renders successfully.
- `AccountController` contains `Login()`.
- `Views/Account/Login.cshtml` exists.
- `wwwroot/css/login.css` exists.
- Page uses the Guest layout.
- Breadcrumbs exist.
- Login card exists.
- Login form exists.
- Sign In button exists and is disabled.
- Google OAuth button exists (visual only).
- Sign Up link points to `/Account/Register`.
- Forgot password link exists and points to `#`.
- Role hint section exists.
- Footer exists.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No authentication logic implemented.
- No login logic implemented.
- No POST actions created.
- No model classes modified.
- No migrations created.
- No database changes made.
- `dotnet build` passes.
