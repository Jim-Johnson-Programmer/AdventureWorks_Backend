# AdventureWorks 2022 — Major Transaction Process Diagrams

Diagrams are derived from the AdventureWorks2022 database schema. Each diagram represents a major business transaction domain using the actual tables and foreign-key relationships.

---

## 1. Sales Order Process

A customer places an order through a sales person, optionally with a credit card and currency conversion, resulting in line items fulfilled from product inventory.

```mermaid
flowchart TD
    A([Customer Places Order]) --> B[Sales.Customer]
    B --> C{Individual\nor Store?}
    C -- Individual --> D[Person.Person]
    C -- Store --> E[Sales.Store]
    D & E --> F[Sales.SalesOrderHeader]

    F --> G[Sales.SalesPerson\nAssigned Rep]
    F --> H[Sales.SalesTerritory\nTerritory]
    F --> I[Person.Address\nBill-To / Ship-To]
    F --> J[Sales.CreditCard\nPayment]
    F --> K[Purchasing.ShipMethod\nShipping]
    F --> L[Sales.CurrencyRate\nFX Conversion]

    F --> M[Sales.SalesOrderDetail\nLine Items]
    M --> N[Sales.SpecialOfferProduct\nDiscount Applied]
    N --> O[Production.Product\nProduct]

    F --> P[Sales.SalesOrderHeaderSalesReason\nSales Reason]
    P --> Q[Sales.SalesReason]

    M --> R([Order Fulfilled])
```

---

## 2. Purchase Order Process

A buyer employee creates a purchase order against a vendor for specific products, with line items tracking received quantities and costs.

```mermaid
flowchart TD
    A([Procurement Need Identified]) --> B[Purchasing.ProductVendor\nVendor–Product Link]
    B --> C[Purchasing.Vendor\nSelect Vendor]
    C --> D[Purchasing.PurchaseOrderHeader\nCreate PO]

    D --> E[HumanResources.Employee\nBuyer]
    D --> F[Purchasing.ShipMethod\nShip Method]
    D --> G[Purchasing.Vendor\nVendor Details]

    D --> H[Purchasing.PurchaseOrderDetail\nLine Items]
    H --> I[Production.Product\nProducts Ordered]
    H --> J[Production.UnitMeasure\nUnit of Measure]

    D --> K{Goods\nReceived?}
    K -- Yes --> L[Production.ProductInventory\nUpdate Inventory]
    K -- No --> M([Awaiting Shipment])

    L --> N[Production.TransactionHistory\nRecord Transaction]
    N --> O([PO Complete])
```

---

## 3. Manufacturing / Work Order Process

A work order drives the manufacturing of a product through routed operations at production locations, consuming components from the bill of materials.

```mermaid
flowchart TD
    A([Production Demand]) --> B[Production.Product\nProduct to Build]
    B --> C[Production.BillOfMaterials\nComponent List]
    C --> D[Production.WorkOrder\nCreate Work Order]

    D --> E[Production.WorkOrderRouting\nRoute Operations]
    E --> F[Production.Location\nWork Center]

    D --> G{Completed\nor Scrapped?}
    G -- Completed --> H[Production.ProductInventory\nAdd to Inventory]
    G -- Scrapped --> I[Production.ScrapReason\nLog Scrap Reason]

    H --> J[Production.TransactionHistory\nRecord Transaction]
    I --> J
    J --> K([Work Order Closed])
```

---

## 4. Employee Onboarding Process

A job candidate is hired, creating a person and employee record, then assigned to a department and shift with pay history tracked over time.

```mermaid
flowchart TD
    A([Candidate Applies]) --> B[HumanResources.JobCandidate\nApplication on File]
    B --> C{Hired?}
    C -- No --> D([Application Closed])
    C -- Yes --> E[Person.BusinessEntity\nCreate Entity]

    E --> F[Person.Person\nCreate Person Record]
    F --> G[Person.EmailAddress\nAdd Email]
    F --> H[Person.PersonPhone\nAdd Phone]
    F --> I[Person.BusinessEntityAddress\nAdd Address]

    F --> J[HumanResources.Employee\nCreate Employee Record]
    J --> K[HumanResources.EmployeeDepartmentHistory\nAssign Department & Shift]
    K --> L[HumanResources.Department]
    K --> M[HumanResources.Shift]

    J --> N[HumanResources.EmployeePayHistory\nSet Pay Rate]
    N --> O([Employee Active])
```

---

## 5. Customer / Person Registration Process

A new customer is registered, linked to contact details and an optional credit card, and associated with either a store account or an individual consumer profile.

```mermaid
flowchart TD
    A([New Customer Registration]) --> B[Person.BusinessEntity\nCreate Entity]
    B --> C[Person.Person\nCreate Person]

    C --> D[Person.EmailAddress\nEmail]
    C --> E[Person.PersonPhone\nPhone]
    C --> F[Person.Password\nCredentials]
    C --> G[Person.BusinessEntityAddress\nAddress]
    G --> H[Person.Address]
    H --> I[Person.StateProvince]
    I --> J[Person.CountryRegion]

    C --> K{Account\nType?}
    K -- Individual --> L[Sales.Customer\nIndividual Customer]
    K -- Store --> M[Sales.Store\nStore Account]
    M --> L

    L --> N[Sales.PersonCreditCard\nLink Credit Card]
    N --> O[Sales.CreditCard]

    L --> P([Customer Ready to Order])
```

---

## 6. Product Catalog Management Process

A new product is defined under a category hierarchy, associated with a model and descriptions in multiple cultures, priced, and made available for sale or manufacture.

```mermaid
flowchart TD
    A([New Product Request]) --> B[Production.ProductCategory\nCategory]
    B --> C[Production.ProductSubcategory\nSubcategory]
    C --> D[Production.Product\nCreate Product]

    D --> E[Production.ProductModel\nAssign Model]
    E --> F[Production.ProductModelProductDescriptionCulture\nLocalized Descriptions]
    F --> G[Production.ProductDescription]
    F --> H[Production.Culture]

    E --> I[Production.ProductModelIllustration\nDiagrams]
    I --> J[Production.Illustration]

    D --> K[Production.ProductDocument\nTechnical Docs]
    K --> L[Production.Document]

    D --> M[Production.ProductProductPhoto\nPhotos]
    M --> N[Production.ProductPhoto]

    D --> O[Production.ProductListPriceHistory\nPrice History]
    D --> P[Production.ProductCostHistory\nCost History]

    D --> Q{Sold or\nManufactured?}
    Q -- Sold --> R[Sales.SpecialOfferProduct\nSpecial Offers]
    Q -- Manufactured --> S[Production.BillOfMaterials\nBOM / Components]

    R & S --> T([Product Active in Catalog])
```

---

## 7. Sales Person & Territory Management

Sales people are assigned to territories with quota histories tracked over time, and stores are linked to territories for reporting.

```mermaid
flowchart TD
    A([Sales Rep Assigned]) --> B[HumanResources.Employee\nEmployee Record]
    B --> C[Sales.SalesPerson\nSales Person Record]
    C --> D[Sales.SalesTerritory\nTerritory Assignment]

    D --> E[Sales.SalesTerritoryHistory\nTerritory History]
    C --> F[Sales.SalesPersonQuotaHistory\nQuota Targets]

    D --> G[Sales.Store\nStores in Territory]
    D --> H[Person.StateProvince\nGeographic Scope]
    H --> I[Sales.CountryRegionCurrency\nCurrency per Region]
    I --> J[Sales.Currency]

    C --> K([Sales Rep Operational])
```

---

## Schema Domain Overview

```mermaid
graph LR
    subgraph Person
        BE[BusinessEntity]
        P[Person]
        A[Address]
    end

    subgraph HumanResources
        E[Employee]
        D[Department]
        PH[PayHistory]
    end

    subgraph Sales
        CU[Customer]
        SOH[SalesOrderHeader]
        SOD[SalesOrderDetail]
        SP[SalesPerson]
        ST[SalesTerritory]
    end

    subgraph Purchasing
        V[Vendor]
        POH[PurchaseOrderHeader]
        POD[PurchaseOrderDetail]
    end

    subgraph Production
        PR[Product]
        WO[WorkOrder]
        BOM[BillOfMaterials]
        TH[TransactionHistory]
        INV[ProductInventory]
    end

    BE --> P
    P --> E
    P --> CU
    E --> SP
    SP --> SOH
    CU --> SOH
    SOH --> SOD
    SOD --> PR
    PR --> WO
    WO --> TH
    WO --> INV
    V --> POH
    POH --> POD
    POD --> PR
    SP --> ST
    ST --> SOH
```
