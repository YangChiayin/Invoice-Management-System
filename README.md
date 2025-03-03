# Invoice Management System

## Project Overview
This repository contains an **ASP.NET Core MVC** application for managing customer invoices. The system allows users to track customers, create invoices, and manage invoice line items in a structured and user-friendly interface.

## Features
- **Customer Management**: Add, view, and edit customer information including contact details and addresses.
- **Invoice Tracking**: Create and manage invoices with due dates and payment terms.
- **Line Item Management**: Add detailed line items to invoices with descriptions and amounts.
- **Dynamic UI**: AJAX-powered forms for seamless updates without page reloads.
- **Data Validation**: Comprehensive validation for all input fields.

## Technology Stack
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 9.0
- **Database**: SQL Server with migration support
- **Frontend**: Bootstrap, jQuery, and Razor views
- **Testing**: Moq for unit testing

## Project Structure
- **Controllers**: Handle HTTP requests and manage application flow.
- **Models**: Define data structures and validation rules.
- **Views**: Render UI components and handle user interaction.
- **Services**: Implement business logic and data operations.
- **Migrations**: Manage database schema changes.

## Entity Relationships
- **Customer**: Contains customer information and has many invoices.
- **Invoice**: Belongs to a customer, has payment terms, and contains multiple line items.
- **InvoiceLineItem**: Belongs to an invoice and represents a billable item.
- **PaymentTerms**: Defines payment conditions that can be applied to invoices.

## Installation
1. **Clone the repository**:
    ```bash
    git clone https://github.com/yangchiayin/chiayin_yang_assignment3.git
    ```

2. **Navigate to the project directory**:
    ```bash
    cd chiayin_yang_assignment3
    ```

3. **Restore dependencies**:
    ```bash
    dotnet restore
    ```

4. **Update the database connection string in `appsettings.json` if needed**.

5. **Apply migrations to create the database**:
    ```bash
    dotnet ef database update
    ```

6. **Run the application**:
    ```bash
    dotnet run
    ```

## Usage
1. Navigate to the **Home Page**.
2. Access the **Customers Page** to view and manage customers.
3. Select a customer to view their invoices.
4. Create new invoices with appropriate payment terms.
5. Add line items to invoices with descriptions and amounts.
6. Track payment status and due dates for each invoice.
