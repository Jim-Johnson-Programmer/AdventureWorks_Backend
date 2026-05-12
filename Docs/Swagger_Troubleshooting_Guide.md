# Swagger UI/API No Response Troubleshooting Guide

## 1. Confirm API is Running

- Ensure your backend API is running and accessible at the expected URL (e.g., http://localhost:5058 or https://localhost:7036).
- Check the terminal or logs for startup errors.

## 2. Check CORS Configuration

- CORS must be enabled for browser-based tools like Swagger UI to call your API.
- For development, use:
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
  ...
  app.UseCors("AllowAll");
  ```
- For production, restrict origins and methods as needed.

## 3. Verify Swagger/OpenAPI Setup

- Swagger should be enabled in development:
  ```csharp
  if (app.Environment.IsDevelopment())
  {
      app.UseSwagger();
      app.UseSwaggerUI();
  }
  ```
- Open Swagger UI at the correct URL (e.g., https://localhost:7036/swagger).

## 4. Match HTTP/HTTPS Schemes

- Both Swagger UI and your API must use the same scheme (http or https).
- Mismatched schemes can cause CORS or network errors.

## 5. Use Browser Dev Tools

- Press F12 to open dev tools.
- Go to the Network tab.
- Try a Swagger endpoint and look for failed requests.
- Check:
  - Status code (e.g., 404, 500, CORS error)
  - Request URL (should match your API’s launch URL)
  - Error message (CORS, network, etc.)

## 6. Common Error Messages

- **CORS error:** CORS is not enabled or not configured correctly.
- **Network error:** API is not running, wrong port, or firewall is blocking.
- **404 Not Found:** Wrong endpoint or API not running at expected URL.
- **HTTPS/HTTP mismatch:** Use the same scheme for both UI and API.

## 7. Additional Checks

- Check `launchSettings.json` for correct applicationUrl.
- Make sure no firewall or proxy is blocking requests.
- If using Docker, ensure ports are mapped correctly.

## 8. Still Stuck?

- Copy the full error from the browser console/network tab.
- Check backend logs for errors.
- Share error details for further help.

## 9. Handling Large Table Timeouts

- If a Swagger endpoint times out or fails when querying a large table (e.g., GET all records), try using a more specific endpoint such as `GET by ID`.
- In Swagger UI, use the endpoint that allows you to search or retrieve a single record by its primary key value (e.g., `/api/Entity/{id}`).
- To find a valid primary key value:
  - Query the database directly for a known ID.
  - Use a smaller query or filter to get a sample ID.
  - Check your database management tool for existing records.
- This approach avoids loading massive datasets and helps confirm the API and database are working.

---

**References:**

- [Microsoft Docs: Enable Cross-Origin Requests (CORS) in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/cors)
- [MDN Web Docs: CORS](https://developer.mozilla.org/docs/Web/HTTP/CORS)
- [Microsoft Docs: Swashbuckle/Swagger for ASP.NET Core](https://learn.microsoft.com/aspnet/core/tutorials/getting-started-with-swashbuckle)
