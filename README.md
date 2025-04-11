# TechXpress - E-commerce Platform with ASP.NET Core 🛒💻

## Objective 🎯:
Build a scalable and maintainable e-commerce web application for selling electronics, leveraging design patterns like NTier Architecture, Repository Pattern, and Unit of Work for efficient development and separation of concerns.

## Description 📋:
TechXpress is a full-featured e-commerce platform that allows users to browse electronics (laptops, mobiles, cameras), add them to a shopping cart, and complete purchases using integrated payment gateways. The platform includes an admin panel for managing products, categories, and orders. It follows the NTier architecture to separate concerns (UI, Business Logic, and Data Access), improving maintainability and scalability.

## Design Patterns 🧩:
- **NTier Architecture**: Separation of the application into Presentation (UI), Business Logic (Service Layer), and Data Access (Repository/Entity Framework) layers.
- **Repository Pattern**: A layer that abstracts database interactions, providing a clean API for querying and persisting data.
- **Unit of Work Pattern**: Ensures that multiple related changes are grouped together in a single transaction, reducing the risk of data inconsistencies.
- **Dependency Injection**: Inject services (repositories, business logic) into controllers to promote testability and loose coupling.

## Technologies 💻:
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **ASP.NET Identity** (for authentication)
- **Stripe** (for payment integration)
- **JQuery**
- **DataTables**
- **Toaster JS** 

# Installation ⚙️:

Ensure you have **.NET SDK** and **SQL Server** or a local database setup.

## 2. Restore NuGet packages:

- In the root directory of your project, run the following command to restore all the necessary dependencies:

   dotnet restore

## 3. Set up the database:

- You need to create and configure the database. The project uses **Entity Framework Core** for database management.

   - Run the migrations to create the database schema:

     dotnet ef database update

   - This will apply any pending migrations and set up the database tables as defined in the `DbContext`.

## 4. Configure your database connection string:

- Open `appsettings.json` and update the connection string for the database:

   Example:
   {
       "ConnectionStrings": {
           "DefaultConnection": "Server=your_server_name;Database=TechXpressDb;Trusted_Connection=True;"
       }
   }

   Make sure the connection string is correct for your local or hosted SQL Server instance.

## 5. Run the application:

- After setting up the database, you can run the application using the following command:

   dotnet run

   This will start the application locally. By default, it will be accessible at `http://localhost:5000`.

## 6. (Optional) Seed the database with initial data:

- If you need to seed the database with sample data (for products, categories, etc.), you can implement a seeding method and execute it. 

   To seed data, you might use a custom command or ensure it’s called on startup.

   Example:
   dotnet run --seed

# Usage 📦:

## 1. **Frontend**:

- Open the application in your browser, and you can start browsing electronics, adding them to your cart, and proceeding to checkout.

## 2. **Admin Panel**:

- Admin users can log in to manage products, categories, and orders through the admin panel.

## 3. **Payment**:

- During the checkout process, payments will be processed securely via **Stripe**. Ensure your **Stripe API keys** are correctly set in `appsettings.json`.

# License 📄:

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
