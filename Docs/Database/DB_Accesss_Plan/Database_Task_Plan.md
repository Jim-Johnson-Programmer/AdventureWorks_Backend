# Database Access Plan for Work Order Process APIs (Using AdventureWorks Tables)

## Overview

This plan outlines the database schema for a Work Order Process system using existing AdventureWorks tables. The system leverages Production.Product, Production.BillOfMaterials, Production.WorkOrder, Production.ProductInventory, Production.ScrapReason, and Production.TransactionHistory to manage manufacturing work orders, inventory, and related transactions.

The APIs will support CRUD operations for work orders, inventory management, bill of materials, and transaction tracking in a manufacturing context.

## Key Entities and Tables

### 1. Production.Product

**Purpose:** Core table for products in the catalog. Work orders are created for manufacturing these products.

**Key Columns:**

- `ProductID` (Primary Key, INT, IDENTITY) - Unique identifier for the product.
- `Name` (NVARCHAR(50), NOT NULL) - Product name.
- `ProductNumber` (NVARCHAR(25), NOT NULL, UNIQUE) - Unique product number.
- `MakeFlag` (BIT, NOT NULL, DEFAULT 1) - Indicates if the product is manufactured in-house.
- `FinishedGoodsFlag` (BIT, NOT NULL, DEFAULT 1) - Indicates if the product is a finished good.
- `Color` (NVARCHAR(15)) - Product color.
- `SafetyStockLevel` (SMALLINT) - Minimum stock level.
- `ReorderPoint` (SMALLINT) - Point at which to reorder.
- `StandardCost` (MONEY, NOT NULL) - Standard cost of the product.
- `ListPrice` (MONEY, NOT NULL) - Selling price.
- `Size` (NVARCHAR(5)) - Product size.
- `SizeUnitMeasureCode` (NCHAR(3)) - Unit of measure for size.
- `WeightUnitMeasureCode` (NCHAR(3)) - Unit of measure for weight.
- `Weight` (DECIMAL(8,2)) - Product weight.
- `DaysToManufacture` (INT, NOT NULL) - Days required to manufacture.
- `ProductLine` (NCHAR(2)) - Product line (R, M, T, S).
- `Class` (NCHAR(2)) - Product class (H, M, L).
- `Style` (NCHAR(2)) - Product style (U, M, W).
- `ProductSubcategoryID` (INT) - Foreign key to Production.ProductSubcategory.
- `ProductModelID` (INT) - Foreign key to Production.ProductModel.
- `SellStartDate` (DATETIME, NOT NULL) - Date the product was first available for sale.
- `SellEndDate` (DATETIME) - Date the product was last available for sale.
- `DiscontinuedDate` (DATETIME) - Date the product was discontinued.
- `rowguid` (UNIQUEIDENTIFIER, NOT NULL, DEFAULT NEWID()) - Row GUID.
- `ModifiedDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Last modified date.

**Foreign Keys:**

- `ProductSubcategoryID` → Production.ProductSubcategory.ProductSubcategoryID
- `ProductModelID` → Production.ProductModel.ProductModelID
- `SizeUnitMeasureCode` → Production.UnitMeasure.UnitMeasureCode
- `WeightUnitMeasureCode` → Production.UnitMeasure.UnitMeasureCode

### 2. Production.BillOfMaterials

**Purpose:** Defines the components (bill of materials) required to build a product. Used in work order planning to determine what parts are needed.

**Key Columns:**

- `BillOfMaterialsID` (Primary Key, INT, IDENTITY) - Unique identifier.
- `ProductAssemblyID` (INT) - Foreign key to Production.Product (the assembled product).
- `ComponentID` (INT, NOT NULL) - Foreign key to Production.Product (the component).
- `StartDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Effective start date.
- `EndDate` (DATETIME) - Effective end date.
- `UnitMeasureCode` (NCHAR(3), NOT NULL) - Unit of measure for the component.
- `BOMLevel` (SMALLINT, NOT NULL) - Level in the bill of materials hierarchy.
- `PerAssemblyQty` (DECIMAL(8,2), NOT NULL, DEFAULT 1.00) - Quantity of component per assembly.
- `ModifiedDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Last modified date.

**Foreign Keys:**

- `ProductAssemblyID` → Production.Product.ProductID
- `ComponentID` → Production.Product.ProductID
- `UnitMeasureCode` → Production.UnitMeasure.UnitMeasureCode

### 3. Production.WorkOrder

**Purpose:** Tracks manufacturing work orders for producing products. Central to the work order process.

**Key Columns:**

- `WorkOrderID` (Primary Key, INT, IDENTITY) - Unique identifier for the work order.
- `ProductID` (INT, NOT NULL) - Foreign key to Production.Product.
- `OrderQty` (INT, NOT NULL) - Quantity to produce.
- `StockedQty` (INT, NOT NULL) - Quantity already stocked.
- `ScrappedQty` (SMALLINT, NOT NULL, DEFAULT 0) - Quantity scrapped.
- `StartDate` (DATETIME, NOT NULL) - Planned start date.
- `EndDate` (DATETIME) - Planned end date.
- `DueDate` (DATETIME, NOT NULL) - Due date.
- `ScrapReasonID` (SMALLINT) - Foreign key to Production.ScrapReason.
- `ModifiedDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Last modified date.

**Foreign Keys:**

- `ProductID` → Production.Product.ProductID
- `ScrapReasonID` → Production.ScrapReason.ScrapReasonID

### 4. Production.ProductInventory

**Purpose:** Tracks inventory levels for products at various locations. Used to check availability before creating work orders.

**Key Columns:**

- `ProductID` (Primary Key, INT, NOT NULL) - Foreign key to Production.Product.
- `LocationID` (Primary Key, SMALLINT, NOT NULL) - Foreign key to Production.Location.
- `Shelf` (NVARCHAR(10), NOT NULL) - Shelf location.
- `Bin` (TINYINT, NOT NULL) - Bin location.
- `Quantity` (SMALLINT, NOT NULL, DEFAULT 0) - Current quantity.
- `rowguid` (UNIQUEIDENTIFIER, NOT NULL, DEFAULT NEWID()) - Row GUID.
- `ModifiedDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Last modified date.

**Foreign Keys:**

- `ProductID` → Production.Product.ProductID
- `LocationID` → Production.Location.LocationID

### 5. Production.ScrapReason

**Purpose:** Lookup table for reasons why products are scrapped during manufacturing.

**Key Columns:**

- `ScrapReasonID` (Primary Key, SMALLINT, IDENTITY) - Unique identifier.
- `Name` (NVARCHAR(50), NOT NULL) - Reason name.
- `ModifiedDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Last modified date.

### 6. Production.TransactionHistory

**Purpose:** Audit trail of transactions affecting product inventory, including work order completions and scrap.

**Key Columns:**

- `TransactionID` (Primary Key, INT, IDENTITY) - Unique identifier.
- `ProductID` (INT, NOT NULL) - Foreign key to Production.Product.
- `ReferenceOrderID` (INT, NOT NULL) - Reference to the order (e.g., WorkOrderID).
- `ReferenceOrderLineID` (INT, NOT NULL, DEFAULT 0) - Reference order line.
- `TransactionDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Date of transaction.
- `TransactionType` (NCHAR(1), NOT NULL) - Type (W=WorkOrder, S=Sale, P=Purchase, etc.).
- `Quantity` (INT, NOT NULL) - Quantity affected.
- `ActualCost` (MONEY) - Actual cost.
- `ModifiedDate` (DATETIME, NOT NULL, DEFAULT GETDATE()) - Last modified date.

**Foreign Keys:**

- `ProductID` → Production.Product.ProductID

## API Endpoints Plan

Based on this schema, the APIs would include:

- **WorkOrders API:** CRUD for work orders, including starting, completing, and scrapping work orders.
- **Products API:** Read product details, including bill of materials.
- **BillOfMaterials API:** Retrieve components needed for a product.
- **Inventory API:** Check and update product inventory levels.
- **ScrapReasons API:** List available scrap reasons.
- **TransactionHistory API:** Query transaction history for auditing.
- **Reports API:** Manufacturing reports (e.g., work order status, inventory levels, scrap analysis).

## Indexes and Performance Considerations

- Clustered index on primary keys.
- Non-clustered indexes on foreign keys (ProductID, LocationID, etc.).
- Index on TransactionDate in TransactionHistory for time-based queries.
- Consider partitioning TransactionHistory by date if it grows large.

## Data Integrity

- Use foreign key constraints to maintain referential integrity.
- Check constraints on quantities (e.g., OrderQty > 0).
- Triggers for updating inventory on work order completions.
