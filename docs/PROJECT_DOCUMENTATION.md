# CompanyEmployees — Project Documentation

## Overview

This document provides a high-level overview of the `CompanyEmployees` solution, its technology stack, architecture, and guidance for running and extending the application.

## Tech Stack

- .NET 8
- C# 12
- Entity Framework Core (SQL Server provider)
- Dependency Injection (built-in .NET DI)
- ASP.NET Core middleware & filters
- SQL Server (connection configured by `sqlConnection`)

Note: The solution contains a custom logging service (`LoggerService`) and a custom CSV output formatter (`CompanyEmployees.Formatters.CsvOutputFormatter`).

## Projects (solution layout)

- `CompanyEmployees.Presentation` — Presentation/web layer (API project); registers services and middleware.
- `CompanyEmployees` — Possible web host / startup project (check which project is the actual startup). Adjust run commands accordingly.
- `Repository` — Data access layer. Contains `RepositoryContext` and `RepositoryManager`.
- `Service` — Business logic / service layer. Contains `ServiceManager` and service implementations.
- `Service.Contracts` — Service interfaces and contracts.
- `Contracts` — Shared contracts/DTOs used across layers.
- `Entities` — Domain/entity models (EF Core entity classes).
- `LoggerService` — Custom logger abstraction and implementation (`ILoggerManager`, `LoggerManager`).
- `Shared` — Shared utilities and common code used across projects.

## Key Components

- Dependency registration: `CompanyEmployees.Extensions.ServiceExtensions` exposes extension methods used during startup such as:
  - `ConfigureCors` — registers the CORS policy named `CorsPolicy`.
  - `ConfigureIISIntegration` — configures IIS options.
  - `ConfigureLoggerService` — registers the custom logger as a singleton.
  - `ConfigureRepositoryManager` and `ConfigureServiceManager` — register repository and service layer managers.
  - `ConfigureActionFilter` — registers `ValidationFilterAttribute`.
  - `ConfigureSqlContext` — registers `RepositoryContext` with a SQL Server connection using the `sqlConnection` key.
  - `AddCustomCSVFormatter` — registers `CsvOutputFormatter` for CSV output support.

- Data access
  - The EF Core `DbContext` is `RepositoryContext` (in the `Repository` project).
  - A `RepositoryManager` provides aggregated repository access (Repository Pattern).
  - `RepositoryContextFactory` exists to support design-time operations (migrations).

- Services
  - `ServiceManager` aggregates business services. Service interfaces live in `Service.Contracts`.

- Logging
  - `LoggerService` exposes `ILoggerManager` and concrete `LoggerManager` used across the solution.

- Middleware & Filters
  - Exception handling middleware is present (`ExceptionMiddlewareExtensions`), used to centralize error handling.
  - Action filters such as `ValidationFilterAttribute` are used to validate incoming requests before controller execution.

- Formatters
  - A custom CSV output formatter is available (`CsvOutputFormatter`) and registered via `AddCustomCSVFormatter`.

## Design Patterns & Principles

- Repository Pattern — data access separated in the `Repository` project via repository interfaces and `RepositoryManager`.
- Service Layer — `ServiceManager` encapsulates business logic separate from controllers.
- Dependency Injection — built-in DI used to inject repositories, services, loggers, filters, etc.
- Factory Pattern — `RepositoryContextFactory` for design-time `DbContext` creation (migrations / tooling).
- Separation of Concerns — Layers isolate responsibilities (Presentation, Service, Repository, Entities).
- Middleware & Filters — cross-cutting concerns (error handling, validation, logging) are separated from controller logic.

## Configuration

- Connection string key: `sqlConnection`. Place the actual connection string in the `appsettings.json` of the web/startup project (commonly `CompanyEmployees.Presentation` or `CompanyEmployees`).
- CORS policy name: `CorsPolicy` (configured in `ServiceExtensions.ConfigureCors`).

## Running Locally

1. Build the solution:

```
dotnet build
```

2. Run the web project (adjust the project name if a different startup project is used):

```
dotnet run --project CompanyEmployees.Presentation
```

3. Ensure the `sqlConnection` string in `appsettings.json` points to a reachable SQL Server instance. If you need to run migrations first, see the next section.

## Database Migrations

If you need to add or apply EF Core migrations, the `Repository` project contains the `DbContext` and `RepositoryContextFactory` for design-time creation. Example commands (adjust `-p` and `-s` paths if your startup/migrations projects differ):

```
# Add a migration
dotnet ef migrations add Initial -p Repository -s CompanyEmployees.Presentation

# Apply migrations to the database
dotnet ef database update -p Repository -s CompanyEmployees.Presentation
```

## Extending the Solution

- Add new services by creating interfaces in `Service.Contracts` and implementations in `Service`, then register them in `ServiceExtensions.ConfigureServiceManager` or similar registration point.
- Add new repositories in the `Repository` project and surface them via `RepositoryManager` and its interface.
- Register new middleware in the web project's startup pipeline (e.g., in `Program.cs`) using the available extension methods (e.g., `ExceptionMiddlewareExtensions`).

## Notes & Recommendations

- Keep business logic in the `Service` layer and avoid placing domain logic in controllers.
- Prefer constructor injection for dependencies to make units easier to test.
- Consider adding integration/unit test projects if not yet present.
- Consider documenting API endpoints (Swagger / OpenAPI) if not already enabled.

---

This document is a concise summary to help contributors understand the solution. For detailed exploration, review the following files and folders mentioned above (open them in your IDE):

- `CompanyEmployees.Presentation/Program.cs` or `CompanyEmployees/Program.cs`
- `CompanyEmployees/Extensions/ServiceExtensions.cs`
- `Repository/RepositoryContext.cs`, `Repository/RepositoryManager.cs`
- `Service/ServiceManager.cs`
- `LoggerService/LoggerManager.cs`
- `CompanyEmployees.Formatters/CsvOutputFormatter.cs`

