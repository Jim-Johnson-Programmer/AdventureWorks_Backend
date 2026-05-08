# High-Level Process — Combining Front End and Back End

---

## Step 1 — Create Front End Screens

### 1.a — Create Front End Project

Set up the front end project using the appropriate framework (e.g., Blazor, Angular, React, MAUI, WPF, etc.) and add it to the solution.

### 1.b — Install Style Libraries

Install UI and styling libraries appropriate to the project, such as:

- **CSS frameworks:** Bootstrap, Tailwind CSS
- **Component libraries:** Telerik, Syncfusion, MudBlazor, Radzen
- **Cross-platform UI:** Avalonia, MAUI Controls
- Configure themes and global style entry points.

### 1.c — Import Wireframes or Screenshots

Import wireframes or screen mockups into the project workspace. These serve as the layout and navigation blueprint for AI-assisted screen generation.

- Place wireframe images or PDFs in a `/Designs/Wireframes/` folder.
- Reference them during AI prompts to establish expected layout and component placement.

### 1.d — Import Existing Style Screenshots for Branding

If available, import screenshots of existing applications, websites, or brand assets to capture current style and theme information.

- Place branding assets in a `/Designs/Branding/` folder.
- Include color palettes, logos, typography samples, and any existing UI screenshots.
- Present these to the AI as style references so generated screens reflect existing branding.

### 1.e — Have AI Create Screens

With the project, tools, wireframes, and branding all in place, prompt AI to generate screens.

- Provide the wireframes, branding screenshots, and installed component library context to the AI.
- Request complete screen/page components including layout, navigation, and data-bound controls.
- Generate one screen at a time, validating each before moving to the next.

### 1.f — Fine-Tune Screens

#### 1.f.1 — Refine Style

Adjust colors, typography, spacing, and component variants to align precisely with brand guidelines and design intent.

#### 1.f.2 — Disable / Hide Items to Force Navigation

Lock down navigation paths appropriate to each screen's role:

- Disable or hide controls that are not yet functional.
- Enforce intended user flow by restricting access to out-of-scope actions.
- Use placeholder states or "coming soon" guards where needed.

#### 1.f.3 — Update Screens to Better Serve Intended Purpose

Review each screen against its use case and refine:

- Adjust field order, labels, and groupings for clarity.
- Add or remove controls based on actual workflow requirements.
- Validate that the screen communicates the correct intent to the end user.

### 1.g — Generate ViewModels / Models to Support Screens

Generate the ViewModels and Models that back each screen, supporting:

- **Data binding** to screen controls.
- **Lazy loading** of data collections.
- **Pre-submit activities** such as:
  - Auto-complete lookups
  - Dynamic drop-down population
  - Field-level validation
  - Dependent field updates

### 1.h — Map Form Controls to Database Columns

#### Form Load Mapping

When loading data into forms for display or editing:

- Identify the primary database table(s) that provide data for each screen (e.g., `Products` table for a product details form).
- Map each form control to the corresponding database column:
  - TextBox for `ProductName` column
  - NumericInput for `Price` column
  - DatePicker for `CreatedDate` column
  - Dropdown/Select for foreign key columns (e.g., `CategoryId` linked to `Categories` table)
- Implement data loading logic in ViewModels to query the database and populate controls.
- Handle related data by loading lookup tables for dropdowns and auto-complete fields.
- Support both single-record loads (e.g., edit form) and list loads (e.g., grid views).

#### Form Event Updates

For dynamic, real-time updates during user interaction:

- Map control events (e.g., onChange, onBlur, onSelect) to trigger updates in related controls or data.
- Implement cascading updates, such as selecting a category that filters available subcategories in another dropdown.
- Handle field-level validation and dependent field updates (e.g., changing quantity updates total price).
- Ensure updates are reflected in the ViewModel state, with options for temporary persistence or immediate database saves.
- Use events to load additional data on demand (e.g., lazy loading details when expanding a section).

#### Form Submission Mapping

On form submission for data origination (insert) or updates:

- Map each control back to the database table columns for INSERT or UPDATE operations.
- Validate all data before submission, including cross-field validations and business rules.
- Handle multi-table operations within transactions (e.g., updating a product and its related inventory).
- Provide user feedback on submission success/failure, including error messages for validation issues.
- Support both full form submissions and partial updates (e.g., saving drafts or individual field changes).

---

## Step 2 — Start Back End Creation

### 2.a — Create Clean Architecture Web API Projects

Follow the Clean Architecture pattern to scaffold the back end solution:

- Create Domain, Application, Infrastructure, and API projects.
- Add project references to enforce the dependency rule (see `CleanArchitecture_DotNet.md`).

### 2.b — Export Database Script

Export a full database script from the existing SQL Server database to serve as the source of truth for AI-assisted data layer generation:

- In SSMS, right-click the target database → **Tasks → Generate Scripts...**
- Select all relevant Tables, Views, and Stored Procedures.
- Save as a single `.sql` file (e.g., `AdventureWorks_FullScript.sql`).
- Use this script with AI to generate Domain Entities, DbContext, Fluent API configurations, and Repository implementations.

> More steps coming soon...

---
