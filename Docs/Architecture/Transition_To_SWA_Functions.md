# Transitioning AdventureWorks Microservices to Extreme Low-Cost Azure Hosting

This guide provides a step-by-step approach to migrate a Clean Architecture microservices solution (see `AdventureWorks_Microservices_Structure.md`) to an **extreme low-cost Azure hosting model** using **Azure Static Web Apps (SWA) + Azure Functions**. This approach is ideal for small/medium apps, prototypes, SaaS MVPs, and internal tools, with typical costs of ~$0–$5/month for low traffic.

---

## Step 1: Assess Your Microservices

- Identify which APIs can be consolidated or split into Azure Functions.
- Group related endpoints into logical Azure Function Apps (e.g., Customer, Product, Sales).
- Determine if you need a frontend (SPA) to be hosted as a Static Web App.

---

## Step 2: Refactor Project Structure for SWA + Functions

- Move each API's logic into an Azure Functions project (can be C#, JavaScript, or Python).
- Place all Function Apps under a `/api` folder for SWA integration.
- Place your SPA frontend (React, Angular, Vue, Blazor WASM, etc.) in a `/app` or `/frontend` folder.

**Example Structure:**

```
AdventureWorks_Backend/
├── Docs/
├── Scripts/
├── app/                # Static frontend (optional)
├── api/                # All Azure Functions (micro-APIs)
│   ├── Customer/
│   ├── Product/
│   ├── SalesOrders/
│   ├── ...
├── tests/
├── .github/
│   └── workflows/      # For GitHub Actions CI/CD
├── staticwebapp.config.json
└── README.md
```

---

## Step 3: Convert Each API to Azure Functions

- For each microservice, create a new Azure Functions project (C# recommended for .NET teams).
- Move controller logic into HTTP-triggered functions.
- Use dependency injection and Clean Architecture patterns as much as possible (supported in .NET Functions).
- Remove ASP.NET Core-specific middleware and replace with Function bindings.

---

## Step 4: Add Static Web App Frontend (Optional)

- If you have a SPA, move it to `/app`.
- Ensure it calls your Functions via `/api/{functionName}` routes.
- Configure `staticwebapp.config.json` for custom routes, auth, and proxies if needed.

---

## Step 5: Configure Local Development

- Use the Azure Static Web Apps CLI (`swa`) for local development and testing.
- Example: `swa start app/ --api api/`
- Update launch/test scripts for Functions and SPA.

---

## Step 6: Set Up GitHub Actions for CI/CD

- Use the default SWA GitHub Actions workflow for automated deploys.
- Place the workflow YAML in `.github/workflows/`.
- On push to `main`, both frontend and Functions are built and deployed.

---

## Step 7: Deploy to Azure Static Web Apps

- Create a new Static Web App resource in Azure Portal.
- Connect your GitHub repo and select the correct folders for app and api.
- Deploy via GitHub Actions.

---

## Step 8: Configure Custom Domains & SSL

- Add your custom domain in the SWA portal.
- Free SSL is included, even on the free tier.

---

## Step 9: Monitor, Secure, and Optimize

- Enable Application Insights for Functions (add via Azure Portal or host.json).
- Use SWA logs for basic monitoring.
- For basic rate limiting, use code in Functions or Azure API Management Consumption tier (optional, still low cost).

---

## Step 10: Remove Unneeded Infrastructure

- Delete old App Service, API Management, or VM resources to avoid extra costs.
- Update documentation to reflect new architecture.

---

## Summary Table: Key Differences

| Aspect                         | SWA + Functions (gateway+API) | App Service + API Management    |
| ------------------------------ | ----------------------------- | ------------------------------- |
| **Typical cost (low traffic)** | ~$0–$5/month                  | ~$30–$200+/month                |
| **Gateway / routing**          | Built-in in Static Web Apps   | API Management (APIM)           |
| **API hosting**                | Azure Functions (consumption) | App Service (always-on)         |
| **Best for**                   | Small/medium, low-cost, MVP   | Enterprise, strict governance   |
| **Custom domains + SSL**       | Included, even on free tier   | Yes (APIM: paid)                |
| **Rate limiting, quotas**      | Basic (via Functions)         | Rich policies, throttling       |
| **Developer experience**       | Simple, GitHub-centric        | More control, more moving parts |
| **Multi-API / microservices**  | Possible, lighter-weight      | First-class, versioning         |
| **Monitoring & analytics**     | App Insights + SWA logs       | APIM analytics, App Insights    |
| **Vendor-style API portal**    | Not built-in                  | Built-in in APIM                |

---

## References

- [Azure Static Web Apps Documentation](https://learn.microsoft.com/azure/static-web-apps/)
- [Azure Functions Documentation](https://learn.microsoft.com/azure/azure-functions/)
- [staticwebapp.config.json Reference](https://learn.microsoft.com/azure/static-web-apps/configuration)
- [GitHub Actions for SWA](https://learn.microsoft.com/azure/static-web-apps/github-actions-workflow)

---

This approach gives you a scalable, cloud-native, and extremely cost-effective microservices platform for .NET and beyond!
