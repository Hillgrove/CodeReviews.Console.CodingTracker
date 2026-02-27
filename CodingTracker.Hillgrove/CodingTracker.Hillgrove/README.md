# Coding Tracker

A console application to log and track coding sessions, built with C#, dapper, Spectre and SQLite.

## Requirements
1. App should be able to log daily coding time, using full CRUD
2. Use Spectre.Console to show data
3. Seperate classes in different files
4. Should tell user the specific format required and not allow other formats
5. Should use a configuration file (appsettings.json) for DB info etc.
6. Needs a CodingSession in a seperate file. Should contain the following properties:
   - Id
   - StartTime
   - EndTime
   - Duration

   When reading from DB use CodingSession as a model rather than an anonymous object.
7. User shouldn't input the duration of the session. It should be calculated on Start and End times
8. The user should be able to input the start and end times manually
9.  Needs to use Dapper
10. Should follow DRY
11. Should include a README file explaining thought process


## How It Works

The app launches into a Spectre.Console-driven menu with four options: create, view, update, and delete coding sessions.

When creating or updating a session, the user is prompted for a start date (`yyyy-MM-dd`), start time (`HH:mm`), end date, and end time. Input is validated strictly against those formats and any other format is rejected and re-prompted. The duration is never entered by the user; it is computed automatically as the difference between start and end.

All sessions are persisted in a local SQLite database via Dapper. The database path and connection string are read from `appsettings.json` at startup, and the table is created automatically if it doesn't exist. Sessions are read into a typed `List<CodingSession>` rather than anonymous objects.

### Project Structure

The project is organized around Separation of Concerns:

| Folder         | Responsibility                                                    |
| -------------- | ----------------------------------------------------------------- |
| `Models/`      | Plain data classes (`CodingSession`)                              |
| `Data/`        | Repository interface + Dapper implementation, DTO, DB initializer |
| `Controllers/` | Business logic layer between UI and data                          |
| `Services/`    | Reusable validation logic                                         |
| `UI/`          | Menus, commands (command pattern), and input helpers              |

Dependency injection (`Microsoft.Extensions.DependencyInjection`) wires everything together in `Program.cs`.

## Thoughts & Reflections

### Approach

I was already familiar with Layered Architecture and the repository pattern, but it had been about a year since I'd implemented either, so this was a good opportunity to dust off those skills. I made a conscious effort to follow DRY and SOLID throughout.

One addition that went slightly beyond the spec was the command pattern for menu items. It keeps the menu clean and each action fully encapsulated, but I'll admit it was scope creep. I got into the zone and went further than strictly necessary. It's a pattern I hadn't used before and wanted to try, so no regrets, but YAGNI is something I need to stay more mindful of.

### What Was Hard

- **Dapper** was new to me. Getting the column-to-property mapping right between the database schema and the C# model (particularly using a DTO as an intermediary) took some trial and error.
- **Spectre.Console** was also new. Learning the API surface for tables, prompts, and markup while keeping the UI clean added time.
- **Time** was a factor. Trying to do things the "proper" way took significantly longer than a quick-and-dirty approach would have.

### What Was Easy

Core C# felt comfortable throughout: syntax, control flow, algorithms, and OOP fundamentals were all second nature. The structure of the solution came together without friction once the architecture was decided.

### What I Learned

Mainly the recovery of rusty skills. Layered Architecture and the repository pattern feel solid again. I also picked up practical experience with Spectre.Console and Dapper, both of which I expect to use again.