# Design Patterns in CompanyEmployees

This document lists design patterns and architectural patterns observed in the solution and where they appear. It also notes patterns that are not present (e.g., CQRS) and gives short guidance on adding them if desired.

## Patterns observed

- Repository Pattern
  - Where: `Repository` project, implemented via repository interfaces and `RepositoryManager`.
  - Purpose: Encapsulates data access logic and abstracts EF Core usage from higher layers.

- Service Layer (Service Facade)
  - Where: `Service` project and `ServiceManager`.
  - Purpose: Encapsulates business logic and orchestrates repositories and domain operations for controllers.

- Singleton
  - Where: `LoggerService` registration in `CompanyEmployees.Extensions.ServiceExtensions` (`ConfigureLoggerService`) registers `ILoggerManager` as a singleton.
  - Purpose: Provides a single shared logger instance across the application.

- Factory (Design-time DB context factory)
  - Where: `CompanyEmployees.ContextFactory.RepositoryContextFactory` (or similar factory class in the `Repository` project).
  - Purpose: Creates `RepositoryContext` instances for design-time operations such as EF Core migrations and tooling.

- Dependency Injection (DI)
  - Where: `CompanyEmployees.Presentation` (startup/Program.cs) and `CompanyEmployees.Extensions.ServiceExtensions` for registrations.
  - Purpose: Centralizes dependency wiring and promotes testability / loose coupling.

- Middleware pipeline
  - Where: `CompanyEmployees.Extensions.ExceptionMiddlewareExtensions` and middleware registered in the web project.
  - Purpose: Centralizes cross-cutting concerns (error handling, logging, etc.) outside controllers.

- Action Filter pattern
  - Where: `ValidationFilterAttribute` registered via `ConfigureActionFilter`.
  - Purpose: Encapsulates request validation before controller execution.

- Formatter (Adapter-like behavior)
  - Where: `CompanyEmployees.Formatters.CsvOutputFormatter` and registration via `AddCustomCSVFormatter`.
  - Purpose: Adapts application data to CSV responses (output formatting concern separated from controllers).

- Unit-of-Work (resembles)
  - Where: `RepositoryManager` aggregates multiple repositories. While not a separate explicit Unit-of-Work class, `RepositoryManager` often fulfills similar responsibilities by coordinating repository operations and encapsulating commits.

## Patterns not observed (or not explicitly implemented)

- CQRS (Command Query Responsibility Segregation)
  - Status: No explicit CQRS implementation was found in the codebase (no dedicated command/query folders, no command/query handler interfaces, and no references to common mediator libraries such as `MediatR`).
  - If you want CQRS: introduce separate `Commands` and `Queries` pipelines, create `ICommandHandler<TCommand>` and `IQueryHandler<TQuery, TResult>` abstractions (or use `MediatR`), keep query models and command models separate, and consider organizing projects/folders per feature.

- Event Sourcing
  - Status: Not present.

## Guidance to add CQRS (quick outline)

1. Add a mediator library (optional): e.g., add `MediatR` to the solution.
2. Create `Commands` and `Queries` for features, plus corresponding handlers:
   - `CreateEmployeeCommand` + `CreateEmployeeCommandHandler`
   - `GetEmployeesQuery` + `GetEmployeesQueryHandler`
3. Keep query handlers free of side-effects and use read-optimized DTOs. Keep command handlers to perform state changes and persist via repositories.
4. Register handlers in DI (or let MediatR register them automatically).
5. Update controllers to publish commands / send queries to the mediator instead of calling service methods directly.

## Notes

- The current architecture already follows layered principles (Presentation → Service → Repository → Entities) which makes introducing CQRS incremental and non-breaking if desired.
- `RepositoryManager` and `ServiceManager` provide convenient wiring points for adding new patterns.


Last scanned areas: `RepositoryManager`, `RepositoryContextFactory`, `ServiceManager`, `LoggerService`, and extension registration in `CompanyEmployees.Extensions.ServiceExtensions`. If you want, I can add a short example of a `Command` / `Query` implementation and how to register `MediatR` in the project.
