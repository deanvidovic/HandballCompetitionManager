Read `AGENTS.md` before starting.

Create an object model for my ASP.NET Core MVC project called "Handball Competition Manager".

## Scope For This Step

For this first step, only create the C# model classes in the `Models/` folder.

Do not:

- Create or update the database.
- Create Entity Framework Core migrations.
- Run migrations.
- Modify database schema.
- Implement controllers, services, views, or seed data.

The goal is only to define the object model with properties, enums, primary keys, foreign keys, and navigation properties. Database configuration and migrations will be handled later.

## Requirements

- Create at least 7 model classes.
- At least 4 classes must be complex and have more than 5 properties.
- Use meaningful property names and correct C# data types.
- Include at least one custom enum.
- Include at least one `DateTime` property.
- Define correct relationships:
    - one-to-many relationships
    - many-to-many relationships
- Use ASP.NET Core MVC with Entity Framework Core.
- Include navigation properties.
- Include primary keys and foreign keys.
- Keep the model realistic but not too complicated.

## Check When Done

- All model classes are created without syntax errors.
- At least 7 model classes are included.
- At least 4 model classes have more than 5 properties.
- At least one custom enum is included.
- At least one `DateTime` property is included.
- One-to-many relationships are correctly defined.
- Many-to-many relationships are correctly defined.
- Navigation properties are included for all relationships.
- Model classes are structured so Entity Framework Core migrations can be added later.
- The project builds without errors.
