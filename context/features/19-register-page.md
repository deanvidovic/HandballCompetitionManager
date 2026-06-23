# Sign Up Page

Read `AGENTS.md`, `architecture.md`, `code-standards.md`, and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create a design-only Sign Up page available at `/Account/Register`.

This page allows a new user to create an account for the Handball Competition Manager.

The page is public and should follow the same visual style as the Home page and Guest pages.

Do not:

- Implement authentication.
- Implement registration logic.
- Implement ASP.NET Core Identity functionality.
- Create users in the database.
- Create or modify model classes.
- Create Entity Framework Core migrations or touch the database.
- Use Bootstrap or any other CSS framework.
- Use JavaScript.
- Implement AJAX.

---

## Requirements

- Use the public Guest layout.
- Keep the design language consistent with the Home page.
- Use mock DTOs if needed.
- Do not hardcode logic inside Controllers or Razor Views.
- The page must be directly accessible through `/Account/Register`.
- This is a UI/design step only.

---

## Styling Requirement

Create a dedicated CSS file:

- `wwwroot/css/register.css`

Rules:

- Do not place page-specific styles inside `_Layout.cshtml`.
- Do not use inline styles.
- Link `register.css` only from `Views/Account/Register.cshtml`.
- Keep global styles only in `wwwroot/css/site.css`.
- Follow `ui-context.md`.

---

## 1. Controller and Route

Create or update `AccountController`.

Requirements:

- Add public `Register()` action.
- Route:

`/Account/Register`

- Return the Register view.
- Do not implement POST actions.
- Do not implement authentication or registration logic.

---

## 2. Breadcrumbs

Display:

Home → Sign Up

Links:

- Home → `/`
- Sign Up → current page

---

## 3. Page Header

Title:

Create your account

Subtitle:

Join Handball Competition Manager and start participating in handball tournaments.

---

## 4. Registration Form

Create a centered registration card.

Fields:

- First Name
- Last Name
- Email Address
- Password
- Confirm Password

All fields should be visually editable.

Do not submit the form.

Do not implement validation.

Buttons:

- Create Account (disabled)

Below the button display:

Already have an account?

Link:

Sign In → `/Account/Login`

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

## 6. Terms Section

Below the form display small muted text:

By creating an account, you agree to the Terms of Service and Privacy Policy.

Terms of Service and Privacy Policy may point to `#`.

---

## 7. Footer

Reuse the same footer style as the Home page.

Left:

Handball Competition Manager

Right:

Tournaments

---

## Check When Done

- `/Account/Register` renders successfully.
- `AccountController` contains `Register()`.
- `Views/Account/Register.cshtml` exists.
- `wwwroot/css/register.css` exists.
- Page uses the Guest layout.
- Breadcrumbs exist.
- Registration card exists.
- Registration form exists.
- Create Account button exists and is disabled.
- Google OAuth button exists (visual only).
- Sign In link points to `/Account/Login`.
- Footer exists.
- Page is responsive below 768px.
- Design matches `ui-context.md`.
- No authentication logic implemented.
- No registration logic implemented.
- No POST actions created.
- No model classes modified.
- No migrations created.
- No database changes made.
- `dotnet build` passes.
