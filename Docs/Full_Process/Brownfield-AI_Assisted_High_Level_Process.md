# High-Level Process — Brownfield AI-Assisted Updates

---

## Step 1 — Update Existing Front End Screens

### 1.h — Map Form Controls to Database Columns

#### Form Load Mapping

When loading data into existing forms for display or editing:

- Identify the primary database table(s) that provide data for each screen (e.g., `Products` table for a product details form).
- Map each form control to the corresponding database column:
  - TextBox for `ProductName` column
  - NumericInput for `Price` column
  - DatePicker for `CreatedDate` column
  - Dropdown/Select for foreign key columns (e.g., `CategoryId` linked to `Categories` table)
- Update data loading logic in existing ViewModels to query the database and populate controls.
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

> More steps coming soon...
