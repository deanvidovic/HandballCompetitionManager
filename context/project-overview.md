# Cafe Customer App

## Overview

Handball Competition Manager is ASP.NET MVC web application for managing handball tournament (group stage + elimination phase) competitions. Application has 4 roles: Admin, Coach, Referee and Guest.

## Goals

### Admin

1. Allow an authenticated Admin to manage the entire system.
2. Allow an authenticated Admin to add teams to a competition.
3. Allow an authenticated Admin to create, view, update, and delete competitions.
4. Allow an authenticated Admin to add and remove users and manage their roles.
5. Allow an authenticated Admin to assign a Referee to a specific match.
6. Allow an authenticated Admin to assign a Coach to a specific team.
7. Allow an authenticated Admin to edit their own profile information.
8. Allow an authenticated Admin to generate tournament brackets automatically.
9. Allow an authenticated Admin to view competition statistics and reports.

### Referee

10. Allow an authenticated Referee to enter match results.
11. Allow an authenticated Referee to manage the team sheet before a match.
12. Allow an authenticated Referee to record match events and statistics (goals, cards, and two-minute suspensions).
13. Allow an authenticated Referee to confirm match results and statistics after the match.
14. Allow an authenticated Referee to view their scheduled matches.
15. Allow an authenticated Referee to edit their own profile information.

### Coach

16. Allow an authenticated Coach to manage players on their team.
17. Allow an authenticated Coach to view the players on their team.
18. Allow an authenticated Coach to view their team's match schedule.
19. Allow an authenticated Coach to view their team's past matches and statistics.
20. Allow an authenticated Coach to view player statistics for their team.
21. Allow an authenticated Coach to edit their own profile information.

### Guest

22. Allow a Guest to view public competition standings.
23. Allow a Guest to view match results.
24. Allow a Guest to view match schedules.
25. Allow a Guest to view competition brackets.
26. Allow a Guest to view team information.
27. Allow a Guest to view player statistics.
28. Prevent a Guest from creating, modifying, or deleting any data.

## Core Admin Flow

1. Admin signs in.
2. Admin creates a new competition.
3. Admin adds teams to the competition.
4. Admin assigns Coaches to teams.
5. Admin assigns Referees to specific matches.
6. The system automatically generates the tournament bracket.
7. Admin reviews and manages the generated match schedule.
8. Admin monitors entered match results, statistics, and competition progress.
9. Admin manages users, roles, teams, matches, and competitions.

## Core Referee Flow

1. Referee signs in.
2. Referee views their assigned matches.
3. Referee selects a scheduled match.
4. Referee manages the team sheet before the match starts.
5. During the match, Referee records goals, cards, and two-minute suspensions.
6. Referee enters the final match result.
7. Referee reviews and confirms all match statistics.
8. The system updates the tournament bracket and advances the winning team automatically.

## Core Coach Flow

1. Coach signs in.
2. Coach views their team information and roster.
3. Coach manages players on their team.
4. Coach reviews upcoming match schedules.
5. Coach views player and team statistics.
6. Coach reviews results and statistics from previous matches.
7. Coach updates their profile information when necessary.

## Core Guest Flow

1. Guest opens the application.
2. Guest browses available competitions.
3. Guest views tournament brackets.
4. Guest views match schedules.
5. Guest views match results.
6. Guest views public competition standings.
7. Guest views team information and player statistics.
8. Guest leaves the application without requiring authentication.

## Features

### Authentication

- Authorization and route protection.
- Normal authorization and OAuth.
- User sign-in and sign-out.
- Role-based authorization (Admin, Referee, Coach, Guest).
- Profile management for authenticated users.

### Competition Management

- Create, update, and delete competitions.
- Register teams for competitions.
- Automatically generate tournament brackets.
- Manage competition schedules.
- View competition standings.

### Team Management

- Create and manage teams.
- Assign Coaches to teams.
- Manage team rosters.
- View team information and statistics.

### User Management

- Create, update, and delete user accounts.
- Assign and manage user roles.
- Manage Referee and Coach assignments.

### Match Management

- Create and schedule matches.
- Assign Referees to matches.
- View match schedules.
- Manage tournament progression.

### Match Reporting

- Enter match results.
- Manage team sheets before matches.
- Record match events (goals, yellow cards, red cards, and two-minute suspensions).
- Confirm final match results and statistics.
- Automatically advance winning teams in the tournament bracket.

### Statistics

- Track player statistics.
- Track team statistics.
- View match statistics.
- Generate competition reports.

### Public Access

- View competition brackets.
- View match schedules and results.
- View public standings.
- View team information.
- View player statistics.

## Scope

### In Scope

- Authentication and protected routes.
- Role-based access for Admin, Referee, Coach, and Guest.
- Creating, viewing, updating, and deleting competitions.
- Adding teams to competitions.
- Assigning Coaches to teams.
- Assigning Referees to matches.
- Automatic tournament bracket generation.
- Viewing and managing match schedules.
- Managing team rosters.
- Managing team sheets before matches.
- Entering match results.
- Recording match events and statistics, including goals, cards, and two-minute suspensions.
- Confirming final match results and statistics.
- Automatically advancing winning teams in the tournament bracket.
- Viewing player, team, match, and competition statistics.
- Editing profile information for authenticated users.
- Public viewing of standings, brackets, schedules, results, teams, and player statistics.

### Out of Scope

- League-based competitions.
- Ticket sales.
- Live video streaming of matches.
- Advanced analytics and prediction systems.
- Offline mode.
- Automatic scheduling based on venue availability.
- Managing venues and referees' real-world availability.
- Editing confirmed match results without Admin permission.

## Success Criteria

1. An authenticated Admin can create, view, update, and delete competitions.
2. An authenticated Admin can add teams to a competition.
3. An authenticated Admin can assign Coaches to teams.
4. An authenticated Admin can assign Referees to specific matches.
5. The system can automatically generate a tournament bracket.
6. An authenticated Referee can view their assigned match schedule.
7. An authenticated Referee can manage the team sheet before a match.
8. An authenticated Referee can enter match results.
9. An authenticated Referee can record match events and statistics.
10. An authenticated Referee can confirm final match results and statistics.
11. The system automatically advances the winning team to the next tournament round.
12. An authenticated Coach can manage players on their team.
13. An authenticated Coach can view their team's match schedule.
14. An authenticated Coach can view past matches and statistics for their team.
15. A Guest can view public standings, brackets, schedules, results, teams, and player statistics.
16. A Guest cannot create, modify, or delete any data.
17. Authenticated users can edit their own profile information.
