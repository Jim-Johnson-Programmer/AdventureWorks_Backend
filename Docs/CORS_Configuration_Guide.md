# CORS Configuration in ASP.NET Core

## What is CORS?

CORS (Cross-Origin Resource Sharing) is a security feature implemented by browsers to restrict web applications running at one origin from interacting with resources from a different origin. In APIs, enabling CORS allows your backend to be called from web frontends hosted on different domains or ports.

## Typical CORS Setup in ASP.NET Core

### 1. Register CORS Services

Add the CORS services in your `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

### 2. Use CORS Middleware

Add the CORS middleware to the HTTP request pipeline, before `UseAuthorization`:

```csharp
app.UseCors("AllowAll");
```

### 3. Restricting CORS (Production Example)

For production, restrict origins, methods, and headers as needed:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.WithOrigins("https://yourfrontend.com")
              .WithMethods("GET", "POST")
              .WithHeaders("content-type");
    });
});

// ...
app.UseCors("MyPolicy");
```

## Troubleshooting CORS Issues

- Ensure CORS middleware is registered **before** authentication/authorization middleware.
- Make sure the policy name matches in both `AddCors` and `UseCors`.
- Check browser console for CORS errors and verify allowed origins.
- For local development, `AllowAnyOrigin` is convenient but not recommended for production.

## References

- [Microsoft Docs: Enable Cross-Origin Requests (CORS) in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/cors)
- [MDN Web Docs: CORS](https://developer.mozilla.org/docs/Web/HTTP/CORS)
