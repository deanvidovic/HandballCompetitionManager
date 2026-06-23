Read `AGENTS.md` and `ui-context.md` before starting. Follow the design system defined in `ui-context.md` exactly.

## Scope

Create the welcome / home page for the `HomeController`.

Do not:

- Create or modify any model classes.
- Create Entity Framework Core migrations or touch the database.
- Implement authentication or authorization logic.
- Use Bootstrap or any other CSS framework.
- Use any JavaScript.

---

## 1. Top Navigation

Create `Views/Shared/_Navbar.cshtml` and include it at the top of the page.

- Fixed to the top, full width, height `56px`.
- **Left side:** site title `Handball Competition Manager` as a home link (`/`).
- **Right side:** navigation links for guests:
    - `Tournaments` → `/Tournaments`
    - `Sign In` → `/Account/Login` — styled as a secondary button.

---

## 2. Hero Section

- Eyebrow text: `HANDBALL COMPETITION MANAGER`
- Headline: `Organize. Manage. Compete.`
- Subheadline: `The complete platform for managing handball tournaments — from team registration to final results.`
- Two CTA buttons side by side:
    - Primary: `View Tournaments` → `/Tournaments`
    - Secondary: `Browse Matches` → `/Tournaments`
- Below buttons: a mock product panel styled as a large dark card. Inside, render a hardcoded scoreboard showing two fictional matches:
    ```
    RK Zagreb   3 – 1   RK Split
    RK Nexe     2 – 2   RK Požega
    ```

---

## 3. Feature Cards Section

- Eyebrow: `FEATURES`
- Section title: `Everything you need to run a tournament`
- Three cards in a row:
    1. **Tournament Management** — `🏆` — Create and manage tournaments, set brackets, and track progress.
    2. **Team & Player Profiles** — `🤾` — Register clubs, manage rosters, and track player details.
    3. **Live Match Events** — `📋` — Record goals, cards, and suspensions in real time.

---

## 4. Stats Section

Four stats in a horizontal row with vertical dividers between them:

| Number | Label          |
| ------ | -------------- |
| `12+`  | Tournaments    |
| `48`   | Teams          |
| `320`  | Players        |
| `200+` | Matches Played |

I would also like you to add JavaScript animation on those numbers when page loads.

---

## 5. Closing CTA Banner

- A single centered panel.
- Headline: `Built for every role in the competition`
- Subtext: `Administrators manage tournaments, Referees record match events, and Coaches track team performance through a single platform.`
- Primary CTA button: `Sign In` → `/Account/Login`

---

## 6. Footer

- Left: `Handball Competition Manager`
- Right: plain text link — `Tournaments`

---

## Check When Done

- `HomeController.cs` exists with `Index()` action.
- `Views/Home/Index.cshtml` renders without errors.
- `Views/Shared/_Navbar.cshtml` exists and is included.
- Navbar has site title left, guest links + Sign In right.
- All five sections are present: hero, features, stats, CTA banner, footer.
- Design matches `ui-context.md`.
- Page is responsive below `768px`.
- No model classes, migrations, or database changes were made.
- `dotnet build` passes.
