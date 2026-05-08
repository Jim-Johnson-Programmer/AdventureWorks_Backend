# Clean Architecture in .NET — Step-by-Step Guide

## Overview

Clean Architecture (by Robert C. Martin) organizes code into concentric layers where dependencies only point **inward**. The core business logic has no knowledge of infrastructure, UI, or external services.

```
┌─────────────────────────────────────┐
│           Presentation              │  ← API / MVC / Blazor
│  ┌───────────────────────────────┐  │
│  │        Infrastructure        │  │  ← EF Core, External APIs, Email
│  │  ┌─────────────────────────┐ │  │
│  │  │      Application       │ │  │  ← Use Cases, CQRS, DTOs
│  │  │  ┌───────────────────┐ │ │  │
│  │  │  │      Domain      │ │ │  │  ← Entities, Value Objects, Interfaces
│  │  │  └───────────────────┘ │ │  │
│  │  └─────────────────────────┘ │  │
│  └───────────────────────────────┘  │
└─────────────────────────────────────┘
```

**Dependency Rule:** Inner layers must never reference outer layers.

---

## Step 1 — Create the Solution

```bash
mkdir MyApp && cd MyApp
dotnet new sln -n MyApp
```

---

## Step 2 — Create the Projects

Create one class library per layer, plus a Web API project:

```bash
# Domain — innermost layer, no dependencies
dotnet new classlib -n MyApp.MyProject.Domain

# Application — orchestrates use cases
dotnet new classlib -n MyApp.MyProject.Application

# Infrastructure — external concerns (DB, APIs, Email, etc.)
dotnet new classlib -n MyApp.MyProject.Infrastructure

# Presentation — API entry point
dotnet new webapi -n MyApp.MyProject.API

# Add all projects to the solution
dotnet sln add MyApp.MyProject.Domain/MyApp.MyProject.Domain.csproj
dotnet sln add MyApp.MyProject.Application/MyApp.MyProject.Application.csproj
dotnet sln add MyApp.MyProject.Infrastructure/MyApp.MyProject.Infrastructure.csproj
dotnet sln add MyApp.MyProject.API/MyApp.MyProject.API.csproj
```

---

## Step 3 — Add Project References (Enforce Dependency Rule)

```bash
# Application depends on Domain
dotnet add MyApp.MyProject.Application reference MyApp.MyProject.Domain

# Infrastructure depends on Application (implements its interfaces)
dotnet add MyApp.MyProject.Infrastructure reference MyApp.MyProject.Application

# API depends on Application and Infrastructure (for DI registration)
dotnet add MyApp.MyProject.API reference MyApp.MyProject.Application
dotnet add MyApp.MyProject.API reference MyApp.MyProject.Infrastructure
```

> **Note:** `Infrastructure` and `API` must never be referenced by inner layers.

---

## Step 4 — Export Database Script for AI-Assisted Data Layer Generation

Before writing any code, export a full database script from the existing SQL Server database. This script is provided to an AI tool to generate the **Domain Entities**, **DbContext**, **Fluent API configurations**, and **Repository** implementations that the remaining steps build upon.

### 4.1 — Generate the Script in SSMS

1. In **SQL Server Management Studio (SSMS)**, open **Object Explorer**.
2. Right-click the target database (e.g., `AdventureWorks`).
3. Navigate to **Tasks → Generate Scripts...** to launch the scripting wizard.
4. In the wizard, choose:
   - **"Select specific database objects"** — include all Tables, Views, Stored Procedures, and any other objects needed.
   - **Set Scripting Options → Advanced:**
     - **Types of data to script:** `Schema and Data` (or `Schema only` if only structure is needed)
     - **Script DROP and CREATE:** `Script CREATE`
     - **Include descriptive headers:** `True`
5. Save the output as a single `.sql` file (e.g., `AdventureWorks_FullScript.sql`).

### 4.2 — Use the Script with AI to Generate the Data Layer

Provide the exported `.sql` file to an AI tool (e.g., GitHub Copilot, ChatGPT) with a prompt such as:

> _"Given the following SQL Server schema script, generate C# EF Core entity classes, a DbContext with Fluent API configurations, and repository implementations following the Clean Architecture pattern."_

The AI will produce:

| Output                          | Target Location                                            |
| ------------------------------- | ---------------------------------------------------------- |
| Entity classes                  | `MyApp.MyProject.Domain/Entities/`                         |
| `IRepository` interfaces        | `MyApp.MyProject.Domain/Interfaces/`                       |
| `AppDbContext` + Fluent configs | `MyApp.MyProject.Infrastructure/Persistence/`              |
| Repository implementations      | `MyApp.MyProject.Infrastructure/Persistence/Repositories/` |

> **Note:** Review and adjust the AI-generated code before placing it in the project. The steps that follow — Domain, Application, Infrastructure, and API layers — build directly on these generated entities and interfaces.

---

## Step 5 — Define the Domain Layer

The Domain layer contains **pure business logic** with no external dependencies.

### 5.1 — Entities

```csharp
// MyApp.MyProject.Domain/Entities/Product.cs
namespace MyApp.MyProject.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product() { } // For EF Core

    public Product(int id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));

        Id = id;
        Name = name;
        Price = price;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("Price cannot be negative.");
        Price = newPrice;
    }
}
```

### 5.2 — Repository Interfaces

```csharp
// MyApp.MyProject.Domain/Interfaces/IProductRepository.cs
namespace MyApp.MyProject.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
```

### 5.3 — Value Objects (optional but recommended)

```csharp
// MyApp.MyProject.Domain/ValueObjects/Money.cs
namespace MyApp.MyProject.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0, currency);
}
```

### 5.4 — Domain Exceptions

```csharp
// MyApp.MyProject.Domain/Exceptions/NotFoundException.cs
namespace MyApp.MyProject.Domain.Exceptions;

public class NotFoundException(string entityName, object key)
    : Exception($"{entityName} with key '{key}' was not found.");
```

---

## Step 6 — Define the Application Layer

The Application layer contains **use cases** (business workflows). It depends only on Domain.

### 6.1 — Install MediatR

```bash
dotnet add MyApp.MyProject.Application package MediatR
dotnet add MyApp.MyProject.Application package FluentValidation
```

### 6.2 — DTOs

```csharp
// MyApp.MyProject.Application/Products/DTOs/ProductDto.cs
namespace MyApp.MyProject.Application.Products.DTOs;

public record ProductDto(int Id, string Name, decimal Price);
```

### 6.3 — CQRS Queries

```csharp
// MyApp.MyProject.Application/Products/Queries/GetProductByIdQuery.cs
using MediatR;
using MyApp.MyProject.Application.Products.DTOs;

namespace MyApp.MyProject.Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

public class GetProductByIdHandler(IProductRepository repository)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(request.Id, ct);
        if (product is null) return null;
        return new ProductDto(product.Id, product.Name, product.Price);
    }
}
```

### 6.4 — CQRS Commands

```csharp
// MyApp.MyProject.Application/Products/Commands/CreateProductCommand.cs
using MediatR;

namespace MyApp.MyProject.Application.Products.Commands;

public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;

public class CreateProductHandler(IProductRepository repository)
    : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product(0, request.Name, request.Price);
        await repository.AddAsync(product, ct);
        return product.Id;
    }
}
```

### 6.5 — Validation with FluentValidation

```csharp
// MyApp.MyProject.Application/Products/Commands/CreateProductCommandValidator.cs
using FluentValidation;

namespace MyApp.MyProject.Application.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}
```

### 6.6 — Register Application Services

```csharp
// MyApp.MyProject.Application/DependencyInjection.cs
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.MyProject.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}
```

---

## Step 7 — Implement the Infrastructure Layer

The Infrastructure layer implements interfaces defined in Domain/Application.

### 7.1 — Install Entity Framework Core

```bash
dotnet add MyApp.MyProject.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add MyApp.MyProject.Infrastructure package Microsoft.EntityFrameworkCore.Tools
```

### 7.2 — DbContext

```csharp
// MyApp.MyProject.Infrastructure/Persistence/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using MyApp.MyProject.Domain.Entities;

namespace MyApp.MyProject.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

### 7.3 — Entity Configuration

```csharp
// MyApp.MyProject.Infrastructure/Persistence/Configurations/ProductConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.MyProject.Domain.Entities;

namespace MyApp.MyProject.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).HasPrecision(18, 2);
    }
}
```

### 7.4 — Repository Implementation

```csharp
// MyApp.MyProject.Infrastructure/Persistence/Repositories/ProductRepository.cs
using Microsoft.EntityFrameworkCore;
using MyApp.MyProject.Domain.Entities;
using MyApp.MyProject.Domain.Interfaces;

namespace MyApp.MyProject.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
        context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default) =>
        await context.Products.ToListAsync(ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await context.Products.AddAsync(product, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var product = await GetByIdAsync(id, ct);
        if (product is not null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync(ct);
        }
    }
}
```

### 7.5 — Register Infrastructure Services

```csharp
// MyApp.MyProject.Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyApp.MyProject.Domain.Interfaces;
using MyApp.MyProject.Infrastructure.Persistence;
using MyApp.MyProject.Infrastructure.Persistence.Repositories;

namespace MyApp.MyProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
```

---

## Step 8 — Implement the Presentation (API) Layer

### 8.1 — Wire Up DI in Program.cs

```csharp
// MyApp.MyProject.API/Program.cs
using MyApp.MyProject.Application;
using MyApp.MyProject.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

### 8.2 — Controller

```csharp
// MyApp.MyProject.API/Controllers/ProductsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyApp.MyProject.Application.Products.Commands;
using MyApp.MyProject.Application.Products.Queries;

namespace MyApp.MyProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id), ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
```

### 8.3 — Global Exception Handling (optional but recommended)

```csharp
// MyApp.MyProject.API/Middleware/ExceptionHandlingMiddleware.cs
using MyApp.MyProject.Domain.Exceptions;

namespace MyApp.MyProject.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        }
    }
}
```

Register it in `Program.cs` before `app.MapControllers()`:

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

---

## Step 9 — Apply EF Core Migrations

Once all AI-generated and hand-crafted code from the previous steps has been reviewed and placed in the correct projects:

```bash
cd MyApp.MyProject.API

dotnet add package Microsoft.EntityFrameworkCore.Design

# Create the initial migration
dotnet ef migrations add InitialCreate --project ../MyApp.MyProject.Infrastructure --startup-project .

# Apply migrations to the database
dotnet ef database update --project ../MyApp.MyProject.Infrastructure --startup-project .
```

---

## Step 10 — Add Distributed Tracing with Jaeger

Jaeger provides distributed tracing so you can visualize request flows across services. Integration uses the **OpenTelemetry** SDK, which exports traces to a Jaeger backend.

### 10.1 — Install NuGet Packages

Add OpenTelemetry packages to the **API** project (and optionally Infrastructure for DB tracing):

```bash
dotnet add MyApp.MyProject.API package OpenTelemetry.Extensions.Hosting
dotnet add MyApp.MyProject.API package OpenTelemetry.Instrumentation.AspNetCore
dotnet add MyApp.MyProject.API package OpenTelemetry.Instrumentation.Http
dotnet add MyApp.MyProject.API package OpenTelemetry.Exporter.OpenTelemetryProtocol
dotnet add MyApp.MyProject.Infrastructure package OpenTelemetry.Instrumentation.EntityFrameworkCore --prerelease
```

### 10.2 — Configure OpenTelemetry in Program.cs

```csharp
// MyApp.MyProject.API/Program.cs
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// ... existing registrations ...

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: builder.Configuration["Jaeger:ServiceName"] ?? "MyApp.MyProject.API",
            serviceVersion: "1.0.0"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(
                builder.Configuration["Jaeger:OtlpEndpoint"] ?? "http://localhost:4317");
        }));
```

### 10.3 — Add Configuration to appsettings.json

```json
// MyApp.MyProject.API/appsettings.json
{
  "Jaeger": {
    "ServiceName": "MyApp.MyProject.API",
    "OtlpEndpoint": "http://localhost:4317"
  }
}
```

For each environment override in `appsettings.Development.json`:

```json
{
  "Jaeger": {
    "OtlpEndpoint": "http://localhost:4317"
  }
}
```

### 10.4 — Run Jaeger Locally with Docker

```bash
docker run -d --name jaeger \
  -p 4317:4317 \
  -p 4318:4318 \
  -p 16686:16686 \
  jaegertracing/all-in-one:latest
```

| Port  | Purpose                        |
| ----- | ------------------------------ |
| 4317  | OTLP gRPC receiver (traces in) |
| 4318  | OTLP HTTP receiver (traces in) |
| 16686 | Jaeger UI (open in browser)    |

Open the Jaeger UI at `http://localhost:16686` after starting the application.

### 10.5 — Add Custom Spans in Application Code (Optional)

Inject `ActivitySource` to create custom trace spans inside use-case handlers:

```csharp
// MyApp.MyProject.Application/Tracing/AppActivitySource.cs
using System.Diagnostics;

namespace MyApp.MyProject.Application.Tracing;

public static class AppActivitySource
{
    public static readonly ActivitySource Instance =
        new("MyApp.MyProject.Application", "1.0.0");
}
```

Usage inside a MediatR handler:

```csharp
using System.Diagnostics;
using MyApp.MyProject.Application.Tracing;

public class GetProductByIdHandler(IProductRepository repository)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        using var activity = AppActivitySource.Instance.StartActivity("GetProductById");
        activity?.SetTag("product.id", request.Id);

        var product = await repository.GetByIdAsync(request.Id, ct);
        if (product is null) return null;
        return new ProductDto(product.Id, product.Name, product.Price);
    }
}
```

Register the `ActivitySource` in the Application `DependencyInjection.cs`:

```csharp
services.AddSingleton(AppActivitySource.Instance);
```

And add it to the OpenTelemetry tracing builder in `Program.cs`:

```csharp
.AddSource(AppActivitySource.Instance.Name)
```

---

## Step 11 — Add Health Checks

Health checks expose HTTP endpoints that monitoring systems, container orchestrators (Kubernetes, Docker), and load balancers can poll to determine whether the application is alive and ready to serve traffic.

### 11.1 — Install NuGet Packages

```bash
dotnet add MyApp.MyProject.API package Microsoft.Extensions.Diagnostics.HealthChecks
dotnet add MyApp.MyProject.API package AspNetCore.HealthChecks.SqlServer
dotnet add MyApp.MyProject.API package AspNetCore.HealthChecks.UI
dotnet add MyApp.MyProject.API package AspNetCore.HealthChecks.UI.Client
dotnet add MyApp.MyProject.API package AspNetCore.HealthChecks.UI.InMemory.Storage
```

### 11.2 — Register Health Checks in Program.cs

```csharp
// MyApp.MyProject.API/Program.cs
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// ... existing registrations ...

builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sql-server",
        tags: ["db", "sql", "ready"])
    .AddDbContextCheck<AppDbContext>(
        name: "ef-core-dbcontext",
        tags: ["db", "ready"]);

builder.Services
    .AddHealthChecksUI(options =>
    {
        options.SetEvaluationTimeInSeconds(15);
        options.AddHealthCheckEndpoint("API Self", "/health");
    })
    .AddInMemoryStorage();
```

### 11.3 — Map Health Check Endpoints

Add the following after `app.UseHttpsRedirection()` and before `app.MapControllers()` in `Program.cs`:

```csharp
// Liveness — is the process running?
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // no checks, just confirms the process is up
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Readiness — is the app ready to serve traffic?
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Full report — all registered checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Health Checks UI dashboard
app.MapHealthChecksUI(options => options.UIPath = "/health-ui");
```

### 11.4 — Add a Custom Health Check (Optional)

For checks not covered by a library (e.g., a third-party API dependency):

```csharp
// MyApp.MyProject.Infrastructure/HealthChecks/ExternalApiHealthCheck.cs
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyApp.MyProject.Infrastructure.HealthChecks;

public class ExternalApiHealthCheck(IHttpClientFactory httpClientFactory)
    : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://external-api/ping", cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("External API is reachable.")
                : HealthCheckResult.Degraded("External API returned a non-success status.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("External API is unreachable.", ex);
        }
    }
}
```

Register it in the Infrastructure `DependencyInjection.cs`:

```csharp
services.AddHealthChecks()
    .AddCheck<ExternalApiHealthCheck>("external-api", tags: ["external", "ready"]);
```

### 11.5 — Health Check Endpoints Reference

| Endpoint        | Purpose                                        |
| --------------- | ---------------------------------------------- |
| `/health`       | Full report of all registered checks           |
| `/health/live`  | Liveness — confirms the process is running     |
| `/health/ready` | Readiness — confirms the app can serve traffic |
| `/health-ui`    | Browser dashboard (HealthChecks UI)            |

---

## Final Folder Structure

```
MyApp/
├── MyApp.sln
└── MyProject/
    ├── MyApp.MyProject.Domain/
    │   ├── Entities/
    │   │   └── Product.cs
    │   ├── Interfaces/
    │   │   └── IProductRepository.cs
    │   ├── ValueObjects/
    │   │   └── Money.cs
    │   └── Exceptions/
    │       └── NotFoundException.cs
    ├── MyApp.MyProject.Application/
    │   ├── DependencyInjection.cs
    │   └── Products/
    │       ├── Commands/
    │       │   ├── CreateProductCommand.cs
    │       │   └── CreateProductCommandValidator.cs
    │       ├── Queries/
    │       │   └── GetProductByIdQuery.cs
    │       └── DTOs/
    │           └── ProductDto.cs
    ├── MyApp.MyProject.Infrastructure/
    │   ├── DependencyInjection.cs
    │   └── Persistence/
    │       ├── AppDbContext.cs
    │       ├── Configurations/
    │       │   └── ProductConfiguration.cs
    │       └── Repositories/
    │           └── ProductRepository.cs
    └── MyApp.MyProject.API/
        ├── Program.cs
        ├── Controllers/
        │   └── ProductsController.cs
        └── Middleware/
            └── ExceptionHandlingMiddleware.cs
```

---

## Key Principles Summary

| Principle                | Description                                                                        |
| ------------------------ | ---------------------------------------------------------------------------------- |
| **Dependency Rule**      | Dependencies only point inward. Domain knows nothing about outer layers.           |
| **Entities**             | Encapsulate business rules; use private setters and guard clauses.                 |
| **Use Cases**            | Application layer orchestrates entities; one class per use case (CQRS).            |
| **Interfaces**           | Defined in Domain/Application, implemented in Infrastructure.                      |
| **DI Registration**      | Each layer owns its own `DependencyInjection.cs` extension method.                 |
| **No Framework Leakage** | Domain and Application layers reference no EF Core, ASP.NET, or third-party infra. |

---

## References

- [Clean Architecture in .NET — Video Tutorial (YouTube)](https://www.youtube.com/watch?v=rjefnUC9Z90&list=PLpKSP8oN83Cmvj2JcpFJsjPRXAaOL6Mf4&index=5)
- [Vertical Slice vs Clean Architecture Comparison (YouTube)](https://www.youtube.com/watch?v=T-EwN9UqRwE&list=PLpKSP8oN83Cmvj2JcpFJsjPRXAaOL6Mf4&index=1)
- [Clean Architecture in .NET — Additional Reference (YouTube)](https://www.youtube.com/watch?v=0phlmiI-1Kk)
