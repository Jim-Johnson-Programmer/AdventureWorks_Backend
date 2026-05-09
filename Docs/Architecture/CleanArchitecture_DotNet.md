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
mkdir AWMicroservices && cd AWMicroservices
dotnet new sln -n AWMicroservices
```

---

## Step 2 — Create the Projects

Create one class library per layer, plus a Web API project:

```bash
# Domain — innermost layer, no dependencies
dotnet new classlib -n AWMicroservices.MyProject.Domain

# Application — orchestrates use cases
dotnet new classlib -n AWMicroservices.MyProject.Application

# Infrastructure — external concerns (DB, APIs, Email, etc.)
dotnet new classlib -n AWMicroservices.MyProject.Infrastructure

# Presentation — API entry point
dotnet new webapi -n AWMicroservices.MyProject.API

# Add all projects to the solution
dotnet sln add AWMicroservices.MyProject.Domain/AWMicroservices.MyProject.Domain.csproj
dotnet sln add AWMicroservices.MyProject.Application/AWMicroservices.MyProject.Application.csproj
dotnet sln add AWMicroservices.MyProject.Infrastructure/AWMicroservices.MyProject.Infrastructure.csproj
dotnet sln add AWMicroservices.MyProject.API/AWMicroservices.MyProject.API.csproj
```

---

## Step 3 — Add Project References (Enforce Dependency Rule)

```bash
# Application depends on Domain
dotnet add AWMicroservices.MyProject.Application reference AWMicroservices.MyProject.Domain

# Infrastructure depends on Application (implements its interfaces)
dotnet add AWMicroservices.MyProject.Infrastructure reference AWMicroservices.MyProject.Application

# API depends on Application and Infrastructure (for DI registration)
dotnet add AWMicroservices.MyProject.API reference AWMicroservices.MyProject.Application
dotnet add AWMicroservices.MyProject.API reference AWMicroservices.MyProject.Infrastructure
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

| Output                          | Target Location                                                      |
| ------------------------------- | -------------------------------------------------------------------- |
| Entity classes                  | `AWMicroservices.MyProject.Domain/Entities/`                         |
| `IRepository` interfaces        | `AWMicroservices.MyProject.Domain/Interfaces/`                       |
| `AppDbContext` + Fluent configs | `AWMicroservices.MyProject.Infrastructure/Persistence/`              |
| Repository implementations      | `AWMicroservices.MyProject.Infrastructure/Persistence/Repositories/` |

> **Note:** Review and adjust the AI-generated code before placing it in the project. The steps that follow — Domain, Application, Infrastructure, and API layers — build directly on these generated entities and interfaces.

---

## Step 5 — Define the Domain Layer

The Domain layer contains **pure business logic** with no external dependencies.

### 5.1 — Entities

```csharp
// AWMicroservices.MyProject.Domain/Entities/Product.cs
namespace AWMicroservices.MyProject.Domain.Entities;

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
// AWMicroservices.MyProject.Domain/Interfaces/IProductRepository.cs
namespace AWMicroservices.MyProject.Domain.Interfaces;

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
// AWMicroservices.MyProject.Domain/ValueObjects/Money.cs
namespace AWMicroservices.MyProject.Domain.ValueObjects;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0, currency);
}
```

### 5.4 — Domain Exceptions

```csharp
// AWMicroservices.MyProject.Domain/Exceptions/NotFoundException.cs
namespace AWMicroservices.MyProject.Domain.Exceptions;

public class NotFoundException(string entityName, object key)
    : Exception($"{entityName} with key '{key}' was not found.");
```

---

## Step 6 — Define the Application Layer

The Application layer contains **use cases** (business workflows). It depends only on Domain.

### 6.1 — Install MediatR

```bash
dotnet add AWMicroservices.MyProject.Application package MediatR
dotnet add AWMicroservices.MyProject.Application package FluentValidation
```

### 6.2 — DTOs

```csharp
// AWMicroservices.MyProject.Application/Products/DTOs/ProductDto.cs
namespace AWMicroservices.MyProject.Application.Products.DTOs;

public record ProductDto(int Id, string Name, decimal Price);
```

### 6.3 — CQRS Queries

```csharp
// AWMicroservices.MyProject.Application/Products/Queries/GetProductByIdQuery.cs
using MediatR;
using AWMicroservices.MyProject.Application.Products.DTOs;

namespace AWMicroservices.MyProject.Application.Products.Queries;

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
// AWMicroservices.MyProject.Application/Products/Commands/CreateProductCommand.cs
using MediatR;

namespace AWMicroservices.MyProject.Application.Products.Commands;

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
// AWMicroservices.MyProject.Application/Products/Commands/CreateProductCommandValidator.cs
using FluentValidation;

namespace AWMicroservices.MyProject.Application.Products.Commands;

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
// AWMicroservices.MyProject.Application/DependencyInjection.cs
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AWMicroservices.MyProject.Application;

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
dotnet add AWMicroservices.MyProject.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add AWMicroservices.MyProject.Infrastructure package Microsoft.EntityFrameworkCore.Tools
```

### 7.2 — DbContext

```csharp
// AWMicroservices.MyProject.Infrastructure/Persistence/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using AWMicroservices.MyProject.Domain.Entities;

namespace AWMicroservices.MyProject.Infrastructure.Persistence;

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
// AWMicroservices.MyProject.Infrastructure/Persistence/Configurations/ProductConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMicroservices.MyProject.Domain.Entities;

namespace AWMicroservices.MyProject.Infrastructure.Persistence.Configurations;

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
// AWMicroservices.MyProject.Infrastructure/Persistence/Repositories/ProductRepository.cs
using Microsoft.EntityFrameworkCore;
using AWMicroservices.MyProject.Domain.Entities;
using AWMicroservices.MyProject.Domain.Interfaces;

namespace AWMicroservices.MyProject.Infrastructure.Persistence.Repositories;

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
// AWMicroservices.MyProject.Infrastructure/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AWMicroservices.MyProject.Domain.Interfaces;
using AWMicroservices.MyProject.Infrastructure.Persistence;
using AWMicroservices.MyProject.Infrastructure.Persistence.Repositories;

namespace AWMicroservices.MyProject.Infrastructure;

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
// AWMicroservices.MyProject.API/Program.cs
using AWMicroservices.MyProject.Application;
using AWMicroservices.MyProject.Infrastructure;

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
// AWMicroservices.MyProject.API/Controllers/ProductsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AWMicroservices.MyProject.Application.Products.Commands;
using AWMicroservices.MyProject.Application.Products.Queries;

namespace AWMicroservices.MyProject.API.Controllers;

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
// AWMicroservices.MyProject.API/Middleware/ExceptionHandlingMiddleware.cs
using AWMicroservices.MyProject.Domain.Exceptions;

namespace AWMicroservices.MyProject.API.Middleware;

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
cd AWMicroservices.MyProject.API

dotnet add package Microsoft.EntityFrameworkCore.Design

# Create the initial migration
dotnet ef migrations add InitialCreate --project ../AWMicroservices.MyProject.Infrastructure --startup-project .

# Apply migrations to the database
dotnet ef database update --project ../AWMicroservices.MyProject.Infrastructure --startup-project .
```

---

## Step 10 — Add Observability: Tracing (Jaeger), Metrics (Prometheus), and Dashboards (Grafana)

The three pillars of observability each address a different question:

| Pillar         | Tool       | Question answered                                        |
| -------------- | ---------- | -------------------------------------------------------- |
| **Traces**     | Jaeger     | "Which service slowed this request down?"                |
| **Metrics**    | Prometheus | "What is the error rate / request throughput right now?" |
| **Dashboards** | Grafana    | "Show me everything on one screen."                      |

All three are wired through the **OpenTelemetry** SDK so the application code is vendor-neutral.

### 10.1 — Install NuGet Packages

Add OpenTelemetry packages to the **API** project (and optionally Infrastructure for DB tracing):

```bash
# Core hosting and instrumentation
dotnet add AWMicroservices.MyProject.API package OpenTelemetry.Extensions.Hosting
dotnet add AWMicroservices.MyProject.API package OpenTelemetry.Instrumentation.AspNetCore
dotnet add AWMicroservices.MyProject.API package OpenTelemetry.Instrumentation.Http
dotnet add AWMicroservices.MyProject.Infrastructure package OpenTelemetry.Instrumentation.EntityFrameworkCore --prerelease

# Traces → Jaeger (via OTLP)
dotnet add AWMicroservices.MyProject.API package OpenTelemetry.Exporter.OpenTelemetryProtocol

# Metrics → Prometheus scrape endpoint
dotnet add AWMicroservices.MyProject.API package OpenTelemetry.Exporter.Prometheus.AspNetCore --prerelease
```

### 10.2 — Configure OpenTelemetry in Program.cs

```csharp
// AWMicroservices.MyProject.API/Program.cs
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// ... existing registrations ...

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: builder.Configuration["Observability:ServiceName"] ?? "AWMicroservices.MyProject.API",
            serviceVersion: "1.0.0"))

    // ── Tracing → Jaeger ──────────────────────────────────────────────────
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddSource(AppActivitySource.Instance.Name)          // custom spans
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(
                builder.Configuration["Observability:JaegerOtlpEndpoint"] ?? "http://localhost:4317");
        }))

    // ── Metrics → Prometheus scrape endpoint (/metrics) ───────────────────
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()      // request count, duration, errors
        .AddHttpClientInstrumentation()      // outbound HTTP call metrics
        .AddRuntimeInstrumentation()         // GC, thread pool, heap
        .AddPrometheusExporter());           // exposes /metrics for Prometheus to scrape
```

Map the `/metrics` scrape endpoint in the middleware pipeline (after `app.UseHttpsRedirection()`):

```csharp
// AWMicroservices.MyProject.API/Program.cs  (middleware pipeline section)
app.MapPrometheusScrapingEndpoint(); // default path: /metrics
```

### 10.3 — Add Configuration to appsettings.json

```json
// AWMicroservices.MyProject.API/appsettings.json
{
  "Observability": {
    "ServiceName": "AWMicroservices.MyProject.API",
    "JaegerOtlpEndpoint": "http://localhost:4317"
  }
}
```

Override for local development in `appsettings.Development.json`:

```json
{
  "Observability": {
    "JaegerOtlpEndpoint": "http://localhost:4317"
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

### 10.5 — Run Prometheus and Grafana Locally with Docker Compose

Create `docker-compose.observability.yml` in the solution root:

```yaml
# docker-compose.observability.yml
services:
  prometheus:
    image: prom/prometheus:latest
    container_name: prometheus
    ports:
      - "9090:9090"
    volumes:
      - ./observability/prometheus.yml:/etc/prometheus/prometheus.yml:ro
    command:
      - "--config.file=/etc/prometheus/prometheus.yml"

  grafana:
    image: grafana/grafana:latest
    container_name: grafana
    ports:
      - "3000:3000"
    environment:
      - GF_SECURITY_ADMIN_USER=admin
      - GF_SECURITY_ADMIN_PASSWORD=admin
    volumes:
      - grafana-data:/var/lib/grafana
      - ./observability/grafana/provisioning:/etc/grafana/provisioning:ro

volumes:
  grafana-data:
```

Create the Prometheus scrape config at `observability/prometheus.yml`:

```yaml
# observability/prometheus.yml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: "AWMicroservices-api"
    static_configs:
      - targets: ["host.docker.internal:5001"] # API /metrics endpoint
    metrics_path: /metrics
```

> **Note:** Replace `host.docker.internal:5001` with the actual host and port your API runs on. On Linux hosts use your machine's LAN IP instead of `host.docker.internal`.

Start the stack:

```bash
docker compose -f docker-compose.observability.yml up -d
```

| URL                     | Purpose                           |
| ----------------------- | --------------------------------- |
| `http://localhost:9090` | Prometheus query UI               |
| `http://localhost:3000` | Grafana dashboard (admin / admin) |

### 10.6 — Configure Grafana to Use Prometheus as a Data Source

1. Open Grafana at `http://localhost:3000` and log in (admin / admin).
2. Go to **Connections → Data Sources → Add data source**.
3. Select **Prometheus**.
4. Set the URL to `http://prometheus:9090` (Docker service name resolves inside the compose network).
5. Click **Save & Test** — you should see "Data source is working".

To add a pre-built ASP.NET Core dashboard:

1. Go to **Dashboards → Import**.
2. Enter dashboard ID **`19924`** (ASP.NET Core — OpenTelemetry) from grafana.com.
3. Select the Prometheus data source and click **Import**.

### 10.7 — Add Custom Spans in Application Code (Optional)

Inject `ActivitySource` to create custom trace spans inside use-case handlers:

```csharp
// AWMicroservices.MyProject.Application/Tracing/AppActivitySource.cs
using System.Diagnostics;

namespace AWMicroservices.MyProject.Application.Tracing;

public static class AppActivitySource
{
    public static readonly ActivitySource Instance =
        new("AWMicroservices.MyProject.Application", "1.0.0");
}
```

Usage inside a MediatR handler:

```csharp
using System.Diagnostics;
using AWMicroservices.MyProject.Application.Tracing;

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

The `.AddSource(AppActivitySource.Instance.Name)` call in Step 10.2 already connects it to the OpenTelemetry tracing pipeline.

---

## Step 11 — Add Health Checks

Health checks expose HTTP endpoints that monitoring systems, container orchestrators (Kubernetes, Docker), and load balancers can poll to determine whether the application is alive and ready to serve traffic.

### 11.1 — Install NuGet Packages

```bash
dotnet add AWMicroservices.MyProject.API package Microsoft.Extensions.Diagnostics.HealthChecks
dotnet add AWMicroservices.MyProject.API package AspNetCore.HealthChecks.SqlServer
dotnet add AWMicroservices.MyProject.API package AspNetCore.HealthChecks.UI
dotnet add AWMicroservices.MyProject.API package AspNetCore.HealthChecks.UI.Client
dotnet add AWMicroservices.MyProject.API package AspNetCore.HealthChecks.UI.InMemory.Storage
```

### 11.2 — Register Health Checks in Program.cs

```csharp
// AWMicroservices.MyProject.API/Program.cs
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
// AWMicroservices.MyProject.Infrastructure/HealthChecks/ExternalApiHealthCheck.cs
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AWMicroservices.MyProject.Infrastructure.HealthChecks;

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

## Step 12 — Add Resilience with Polly

Polly (via `Microsoft.Extensions.Http.Resilience`) adds retry, circuit-breaker, timeout, and hedging policies to outbound `HttpClient` calls made from the Infrastructure layer. In .NET 8+ the recommended entry point is the built-in resilience pipeline; raw Polly policies are still available for fine-grained control.

### 12.1 — Install NuGet Package

```bash
dotnet add AWMicroservices.MyProject.Infrastructure package Microsoft.Extensions.Http.Resilience
```

> `Microsoft.Extensions.Http.Resilience` targets Polly v8 and integrates directly with `IHttpClientBuilder`.

### 12.2 — Add a Standard Resilience Pipeline (Recommended)

The standard pipeline bundles retry, circuit breaker, attempt timeout, and total request timeout with sensible defaults.

```csharp
// AWMicroservices.MyProject.Infrastructure/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AWMicroservices.MyProject.Domain.Interfaces;
using AWMicroservices.MyProject.Infrastructure.Persistence;
using AWMicroservices.MyProject.Infrastructure.Persistence.Repositories;

namespace AWMicroservices.MyProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();

        // Named HttpClient for an external API with a standard resilience pipeline
        services.AddHttpClient("ExternalApiClient", client =>
            {
                client.BaseAddress = new Uri(
                    configuration["ExternalApi:BaseUrl"] ?? "https://api.example.com");
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddStandardResilienceHandler(); // retry + circuit-breaker + timeouts

        return services;
    }
}
```

### 12.3 — Customise the Resilience Pipeline (Optional)

Override individual pipeline options when the defaults do not fit:

```csharp
services.AddHttpClient("ExternalApiClient", client =>
    {
        client.BaseAddress = new Uri(configuration["ExternalApi:BaseUrl"]!);
    })
    .AddStandardResilienceHandler(options =>
    {
        // Retry up to 3 times with exponential back-off
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromMilliseconds(500);
        options.Retry.UseJitter = true;

        // Open the circuit after 5 failures in a 30-second window
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.MinimumThroughput = 5;
        options.CircuitBreaker.FailureRatio = 0.5;

        // Per-attempt and total timeout
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(45);
    });
```

### 12.4 — Consume the HttpClient in a Service

Inject `IHttpClientFactory` into any Infrastructure service that calls external APIs:

```csharp
// AWMicroservices.MyProject.Infrastructure/Services/ExternalProductService.cs
using System.Net.Http.Json;
using AWMicroservices.MyProject.Application.Contracts;
using AWMicroservices.MyProject.Application.Products.DTOs;

namespace AWMicroservices.MyProject.Infrastructure.Services;

public class ExternalProductService(IHttpClientFactory httpClientFactory)
    : IExternalProductService
{
    public async Task<ProductDto?> GetExternalProductAsync(int id, CancellationToken ct = default)
    {
        var client = httpClientFactory.CreateClient("ExternalApiClient");
        return await client.GetFromJsonAsync<ProductDto>($"/products/{id}", ct);
    }
}
```

Define the interface in the Application layer (it lives in `Application/Contracts/`) so Infrastructure can implement it without leaking dependencies inward:

```csharp
// AWMicroservices.MyProject.Application/Contracts/IExternalProductService.cs
using AWMicroservices.MyProject.Application.Products.DTOs;

namespace AWMicroservices.MyProject.Application.Contracts;

public interface IExternalProductService
{
    Task<ProductDto?> GetExternalProductAsync(int id, CancellationToken ct = default);
}
```

Register the service in `Infrastructure/DependencyInjection.cs`:

```csharp
services.AddScoped<IExternalProductService, ExternalProductService>();
```

### 12.5 — Polly Policy Summary

| Policy              | Default Behaviour (Standard Pipeline)                         |
| ------------------- | ------------------------------------------------------------- |
| **Retry**           | 3 attempts, exponential back-off with jitter                  |
| **Circuit Breaker** | Opens after 50 % failures over a 30-second sampling window    |
| **Attempt Timeout** | 10 seconds per individual attempt                             |
| **Total Timeout**   | 30 seconds for the entire request (including all retries)     |
| **Hedging**         | Not in standard pipeline; add via `AddStandardHedgingHandler` |

---

## Step 13 — Add an Ocelot API Gateway

Ocelot is a .NET API Gateway that acts as a single entry point for client traffic. It routes requests to the appropriate downstream microservices, handles authentication, rate-limiting, load balancing, and more — without clients needing to know individual service addresses.

### 13.1 — Create the Gateway Project

```bash
# Create a minimal Web API project for the gateway
dotnet new web -n AWMicroservices.MyProject.Gateway

# Add it to the solution
dotnet sln add AWMicroservices.MyProject.Gateway/AWMicroservices.MyProject.Gateway.csproj
```

> The Gateway has **no references** to any other project in the solution. It only needs Ocelot and is a standalone reverse-proxy entry point.

### 13.2 — Install Ocelot

```bash
dotnet add AWMicroservices.MyProject.Gateway package Ocelot
```

For JWT authentication forwarding also add:

```bash
dotnet add AWMicroservices.MyProject.Gateway package Ocelot.Provider.Polly   # optional: Polly QoS in Ocelot
dotnet add AWMicroservices.MyProject.Gateway package Microsoft.AspNetCore.Authentication.JwtBearer
```

### 13.3 — Create ocelot.json

Define the downstream routes Ocelot will proxy. Place this file in the Gateway project root.

```json
// AWMicroservices.MyProject.Gateway/ocelot.json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/products/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [{ "Host": "localhost", "Port": 7001 }],
      "UpstreamPathTemplate": "/gateway/products/{everything}",
      "UpstreamHttpMethod": ["GET", "POST", "PUT", "DELETE"],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer",
        "AllowedScopes": []
      },
      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "1s",
        "PeriodTimespan": 1,
        "Limit": 10
      },
      "QoSOptions": {
        "ExceptionsAllowedBeforeBreaking": 3,
        "DurationOfBreak": 10000,
        "TimeoutValue": 5000
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:7000"
  }
}
```

| Field                    | Purpose                                                  |
| ------------------------ | -------------------------------------------------------- |
| `DownstreamPathTemplate` | The real URL path on the downstream service              |
| `UpstreamPathTemplate`   | The public path clients call on the gateway              |
| `DownstreamHostAndPorts` | Address of the actual microservice                       |
| `AuthenticationOptions`  | Enforce a JWT bearer token before forwarding the request |
| `RateLimitOptions`       | Per-client request rate limiting                         |
| `QoSOptions`             | Polly circuit-breaker and timeout applied by Ocelot      |

Create an environment-specific override for local development:

```json
// AWMicroservices.MyProject.Gateway/ocelot.Development.json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/products/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [{ "Host": "localhost", "Port": 5001 }],
      "UpstreamPathTemplate": "/gateway/products/{everything}",
      "UpstreamHttpMethod": ["GET", "POST", "PUT", "DELETE"]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

### 13.4 — Configure Program.cs

```csharp
// AWMicroservices.MyProject.Gateway/Program.cs
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Load ocelot.json (and environment override if present)
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",
                 optional: true, reloadOnChange: true);

// JWT authentication — the gateway validates the token and forwards claims
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience  = builder.Configuration["Jwt:Audience"];
    });

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();

await app.UseOcelot();

app.Run();
```

### 13.5 — Add Gateway Configuration to appsettings.json

```json
// AWMicroservices.MyProject.Gateway/appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Ocelot": "Warning"
    }
  },
  "Jwt": {
    "Authority": "https://your-identity-provider",
    "Audience": "your-api-audience"
  }
}
```

### 13.6 — Register the Gateway in the Solution

```bash
# The gateway project is already added to the solution (Step 13.1).
# To run it together with the API during development, use a launch profile
# or the .NET Aspire orchestration (if applicable).

# Verify the solution structure
dotnet sln list
```

### 13.7 — Gateway Architecture Overview

```
Client
  │
  ▼
┌─────────────────────────────────────────┐
│  AWMicroservices.MyProject.Gateway  (port 5000)   │  ← Ocelot (routing, auth, rate-limit, QoS)
└─────────────────────────────────────────┘
  │  /gateway/products/{id}  →  /api/products/{id}
  ▼
┌─────────────────────────────────────────┐
│  AWMicroservices.MyProject.API      (port 5001)   │  ← Clean Architecture API (Controllers → MediatR)
└─────────────────────────────────────────┘
  │
  ▼
┌─────────────────────────────────────────┐
│  SQL Server / External APIs             │
└─────────────────────────────────────────┘
```

---

## Final Folder Structure

```
AWMicroservices/
├── AWMicroservices.sln
└── MyProject/
    ├── AWMicroservices.MyProject.Domain/
    │   ├── Entities/
    │   │   └── Product.cs
    │   ├── Interfaces/
    │   │   └── IProductRepository.cs
    │   ├── ValueObjects/
    │   │   └── Money.cs
    │   └── Exceptions/
    │       └── NotFoundException.cs
    ├── AWMicroservices.MyProject.Application/
    │   ├── DependencyInjection.cs
    │   ├── Contracts/
    │   │   └── IExternalProductService.cs        ← added in Step 12
    │   └── Products/
    │       ├── Commands/
    │       │   ├── CreateProductCommand.cs
    │       │   └── CreateProductCommandValidator.cs
    │       ├── Queries/
    │       │   └── GetProductByIdQuery.cs
    │       └── DTOs/
    │           └── ProductDto.cs
    ├── AWMicroservices.MyProject.Infrastructure/
    │   ├── DependencyInjection.cs
    │   ├── Persistence/
    │   │   ├── AppDbContext.cs
    │   │   ├── Configurations/
    │   │   │   └── ProductConfiguration.cs
    │   │   └── Repositories/
    │   │       └── ProductRepository.cs
    │   ├── Services/
    │   │   └── ExternalProductService.cs         ← added in Step 12 (Polly HttpClient)
    │   └── HealthChecks/
    │       └── ExternalApiHealthCheck.cs
    ├── AWMicroservices.MyProject.API/
    │   ├── Program.cs
    │   ├── Controllers/
    │   │   └── ProductsController.cs
    │   └── Middleware/
    │       └── ExceptionHandlingMiddleware.cs
    └── AWMicroservices.MyProject.Gateway/                  ← added in Step 13 (Ocelot)
        ├── Program.cs
        ├── ocelot.json
        └── ocelot.Development.json
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
