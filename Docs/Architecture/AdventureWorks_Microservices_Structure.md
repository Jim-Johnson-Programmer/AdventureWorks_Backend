# AdventureWorks Microservices — Folder & Project Structure

This guide shows a recommended folder and project structure for a Clean Architecture solution with **7 microservice APIs** (e.g., Customer, Product, Sales, Purchase, Manufacturing, Employment, and Inventory) in a typical workplace. It covers both:

- **Standard (non-Kubernetes) structure**
- **Kubernetes-friendly structure**

---

## 1. Standard (Non-Kubernetes) Structure

```
AdventureWorks_Backend/
│
├── AWMicroservices.slnx
├── README.md
├── Docs/
│   └── ...
├── Scripts/
│   └── ...
├── src/
│   ├── CustomerManagement/
│   │   ├── AWMicroservices.Customer.API/
│   │   ├── AWMicroservices.Customer.Application/
│   │   ├── AWMicroservices.Customer.Domain/
│   │   └── AWMicroservices.Customer.Infrastructure/
│   ├── ProductCatalog/
│   │   ├── AWMicroservices.Product.API/
│   │   ├── AWMicroservices.Product.Application/
│   │   ├── AWMicroservices.Product.Domain/
│   │   └── AWMicroservices.Product.Infrastructure/
│   ├── SalesOrders/
│   │   ├── AWMicroservices.SalesOrders.API/
│   │   ├── AWMicroservices.SalesOrders.Application/
│   │   ├── AWMicroservices.SalesOrders.Domain/
│   │   └── AWMicroservices.SalesOrders.Infrastructure/
│   ├── PurchaseOrders/
│   │   └── ...
│   ├── ManufacturingWorkOrders/
│   │   └── ...
│   ├── Employment/
│   │   └── ...
│   └── Inventory/
│       └── ...
├── tests/
│   ├── CustomerManagement.Tests/
│   ├── ProductCatalog.Tests/
│   ├── SalesOrders.Tests/
│   ├── ...
└── .vscode/
    ├── launch.json
    └── tasks.json
```

**Key Points:**

- Each microservice has its own folder under `src/`, with Clean Architecture layers as subfolders.
- Shared scripts, docs, and solution file are at the root.
- Tests are grouped by service under `tests/`.
- `.vscode/` contains workspace-wide debugging and task configs.

---

## 2. Kubernetes-Friendly Structure

For Kubernetes, add deployment manifests and Helm charts for each service:

```
AdventureWorks_Backend/
│
├── ... (same as above)
├── k8s/
│   ├── base/
│   │   ├── namespace.yaml
│   │   └── ingress.yaml
│   ├── customer/
│   │   ├── deployment.yaml
│   │   ├── service.yaml
│   │   └── configmap.yaml
│   ├── product/
│   │   └── ...
│   ├── salesorders/
│   │   └── ...
│   ├── ...
│   └── helm/
│       ├── Chart.yaml
│       ├── values.yaml
│       └── templates/
│           ├── customer-deployment.yaml
│           ├── product-deployment.yaml
│           └── ...
```

**Kubernetes Additions:**

- `k8s/` holds all Kubernetes manifests and Helm charts.
- Each service has its own deployment/service/configmap YAMLs.
- `base/` for shared resources (namespace, ingress, etc.).
- `helm/` for Helm-based deployments (optional, but recommended for DRY and parameterization).

---

## 3. Example: Customer Microservice

```
src/CustomerManagement/
├── AWMicroservices.Customer.API/
├── AWMicroservices.Customer.Application/
├── AWMicroservices.Customer.Domain/
└── AWMicroservices.Customer.Infrastructure/

k8s/customer/
├── deployment.yaml
├── service.yaml
└── configmap.yaml
```

---

## 4. Best Practices

- Keep each microservice isolated (separate projects, configs, and deployments).
- Use root-level solution file for easy IDE management.
- Store all Kubernetes manifests in a single `k8s/` folder for clarity.
- Use Helm for templating and parameterization in production.
- Place all documentation and scripts in dedicated root folders.

---

## 5. Summary Table

| Folder/File            | Purpose                                     |
| ---------------------- | ------------------------------------------- |
| `src/`                 | All microservice source code                |
| `tests/`               | Unit/integration tests per service          |
| `Docs/`                | Documentation, architecture, onboarding     |
| `Scripts/`             | Utility scripts (migrations, cleanup, etc.) |
| `k8s/`                 | Kubernetes manifests and Helm charts        |
| `.vscode/`             | Workspace-wide VS Code configs              |
| `AWMicroservices.slnx` | Solution file for all projects              |

---

Follow this structure for scalable, maintainable, and cloud-ready microservices development in .NET!
