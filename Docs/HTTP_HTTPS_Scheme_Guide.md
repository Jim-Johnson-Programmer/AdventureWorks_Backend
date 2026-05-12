# Understanding and Troubleshooting HTTP/HTTPS Scheme Mismatches

## What are HTTP and HTTPS Schemes?

- **HTTP**: Unencrypted web traffic (e.g., `http://localhost:5058`)
- **HTTPS**: Encrypted web traffic (e.g., `https://localhost:7036`)
- The "scheme" is the part before the `://` in a URL.

## Why Does Scheme Matching Matter?

- Browsers enforce strict security rules for cross-origin requests.
- If your Swagger UI is loaded over HTTPS, but your API is running on HTTP, the browser will block requests for security reasons (mixed content).
- Both the UI and API must use the same scheme (either both HTTP or both HTTPS) for requests to succeed.

## Where to Check the Schemes

### 1. API Launch URL

- Check your API's launch URL in `Properties/launchSettings.json`:
  ```json
  "applicationUrl": "https://localhost:7036;http://localhost:5058"
  ```
- This means your API can run on both HTTP and HTTPS.

### 2. Browser Address Bar

- When you open Swagger UI, look at the address bar:
  - If it starts with `https://`, you are using HTTPS.
  - If it starts with `http://`, you are using HTTP.

### 3. Swagger UI Network Requests

- Open browser dev tools (F12) → Network tab.
- Trigger a Swagger API call.
- Look at the "Request URL" for the failed request.
- Compare the scheme (`http` or `https`) to the one in your address bar.

## Common Problems

- **Swagger UI at `https://localhost:7036/swagger` tries to call `http://localhost:5058/api/...`**
  - This will fail due to mixed content.
- **Swagger UI at `http://localhost:5058/swagger` tries to call `https://localhost:7036/api/...`**
  - This will also fail if the API is not running on HTTPS or the certificate is not trusted.

## How to Fix

- Always use the same scheme for both Swagger UI and API.
  - If you open Swagger at `https://localhost:7036/swagger`, make sure your API is running on `https://localhost:7036`.
  - If you open Swagger at `http://localhost:5058/swagger`, make sure your API is running on `http://localhost:5058`.
- If you see a browser warning about "mixed content" or "insecure requests," switch both to HTTPS or both to HTTP (for local dev).
- For HTTPS, ensure your development certificate is trusted (run `dotnet dev-certs https --trust` if needed).

## Summary Table

| Swagger UI URL                 | API URL Called                 | Result     |
| ------------------------------ | ------------------------------ | ---------- |
| https://localhost:7036/swagger | https://localhost:7036/api/... | ✅ Works   |
| http://localhost:5058/swagger  | http://localhost:5058/api/...  | ✅ Works   |
| https://localhost:7036/swagger | http://localhost:5058/api/...  | ❌ Blocked |
| http://localhost:5058/swagger  | https://localhost:7036/api/... | ❌ Blocked |

## References

- [MDN: Mixed Content](https://developer.mozilla.org/docs/Web/Security/Mixed_content)
- [Microsoft Docs: Enforce HTTPS in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/enforcing-ssl)
