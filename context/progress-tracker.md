# Progress Tracker

Update this file after every meaningful implementation
change.

## Current Phase

- Complete

## Current Goal

- Define the initial C# object model for Handball Competition Manager.

## Completed

- Read `context/features/01-entities.md`.
- Checked for `AGENTS.md`; no file was present in the repository.
- Added initial model classes in `Models/`:
  - `Competition`
  - `Team`
  - `Player`
  - `Venue`
  - `Match`
  - `Referee`
- Added custom enums:
  - `CompetitionStatus`
  - `MatchStatus`
  - `RefereeRole`
- Replaced explicit many-to-many middle classes with direct EF Core navigation collections:
  - `Team.Competitions` and `Competition.Teams`
  - `Match.Referees` and `Referee.Matches`
- Removed `TeamCompetition` and `MatchOfficial`.
- Verified the project builds successfully with `dotnet build`.

## In Progress

- None.

## Next Up

- Next feature step from `context/features/` when ready.

## Open Questions

- None.

## Architecture Decisions

- Teams and competitions use direct many-to-many navigation properties because no extra data is currently needed on the relationship.
- Matches reference `HomeTeam`, `AwayTeam`, `Competition`, and `Venue` with foreign key properties so Entity Framework Core can infer one-to-many relationships later.
- Referees and matches use direct many-to-many navigation properties because no extra data is currently needed on the relationship.
- Models are plain EF-friendly C# classes only; no database context, migrations, controllers, views, services, seed data, or schema updates were added.

## Session Notes

- Scope from `01-entities.md` is models only. Database configuration and migrations are intentionally deferred.
