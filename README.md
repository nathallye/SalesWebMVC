# Sales Web MVC
Building a sales system using ASP.NET Core Web App.

## Shortcuts in Visual Studio

- `Ctrl + Shift + :` - comment out selected lines with /* */
- `Ctrl + K + Ctrl + C` - comment out selected lines with //
- `Ctrl + K + D` - organize file indentation
- `cw + tab tab` - console writeline

## ASP.NET Core MVC overview

- It is a framework for creating web applications;
- Created by Microsoft and the community;
- Open source;
- Runs on both .NET Framework and .NET Core;
- The framework works with a well-defined structure, including:
  - Controllers
  - Views
  - Models
    - View Models
- https://docs.microsoft.com/en-us/aspnet/core/mvc/overview

## Project creation

In Visual Studio 2022, select the `Create a new project` > then the type of project `ASP.NET Core Web Application (Model-View-Controller)`. At the bottom we have text boxes to define the name of the project, the local on the file system and the name of the solution. For our example, let's use `SalesWebMVC` as the project and solution name. Once that's done, we're going to `Next` and we're going to select the .NET version that we're going to use in this project, to conclude we're going to click on `Create`.

## Project structure

- **wwwroot:** application resources (css, imagens, etc.);
- **Controllers:** application's MVC controllers;
- **Models:** entities and "view models";
- **Views:** pages (notice naming conventions against controllers);
- **Shared:** views used for more than one controller;
- **appsettings.json:** external resources configuration (logging, connection strings, etc.);
- **Program.cs:** entry point and app configuration.

## Controller and Razor

- **Route pattern** - Controller/Action/Id
  - Each controller method is mapped to an action.
- **@{ }** - C# block in Razor Page; 
- **ViewData** - dictionary created in controller and rendered in views;
- Tag Helpers in Razor Pages
  -  Examples: **asp-controller** and **asp-action**.
- **IActionResult** return types:

  <div align="center">
    <img width="700" src="https://user-images.githubusercontent.com/86172286/205783054-b25c2023-8088-4fc0-b4e8-cfa4ab606c52.png">
  </div>

## First Model-Controller-View - Department

- Create new folder `ViewModels` e move `ErrorViewModel` (including namespace)
  - CTRL+SHIFT+B to fix references
  
- Create class `Models/Department` (attributes Id and Name)
- Create controller
  - Right button `Controllers` > `Add` > `Controller` > `MVC Controller Empty`
    - Name: `DepartmentsController` (PLURAL)
    - Instantiate a List<Department> and return it as View method parameter

- Create new folder `Views/Departments` (PLURAL)
- Create view 
  - Right button `Views/Departments` > `Add` > `View`
    - View name: Index
    - Template: List
    - Model class: Department
    - Change Title to Departments
    - Notice:
      - @model definition
      - intellisense for model
      - Helper methods
      - @foreach block

## Deleting Department view and controller (to then generate the complete CRUD with Scaffolding)
  
- Delete controller Departments
- Delete folder Views/Departments

## CRUD Scaffolding
  
- Right button `Controllers` > `Add` > `New Scaffolded Item` > `MVC controllers with views, using Entity Framework`
  - Model class: Department
  - Data context class: + and accept the name
  - Views (options): all three
  - Controller name: DepartmentsController

## SQLServer and first migration
  
Note: we're using CODE-FIRST workflow
  
- In `appsettings.json`, set connection string from the database;
  
- Stop IIS
  - CTRL+SHIFT+B
  
- In `Package Manager Console` create first Migration with the following commands:
  - `Add-Migration Initial`
  - `Update-Database`
  
- Check database in SQL Server
  
- Test app: CTRL+F5

## Changing theme

- Go to: http://bootswatch.com/[version] (check Bootstrap version)
  - Choose a theme
  - Download bootstrap.css
    - Suggestion: rename to bootstrap-name.css
    - Save file to wwwroot/lib/bootstrap/dist/css (paste it inside Visual Studio)
  - Open _Layout.cshtml
    - Update bootstrap reference
  
## Other entities and second migration
  
<div align="center">
  <img width="700" src="https://user-images.githubusercontent.com/86172286/206231879-4781fec0-9a09-4d3d-b043-1cf8812005f6.png">
</div>
  
- Implement domain model
  - Basic attributes
    - `class` type
      - **Seller** - Id, Name, Email, BirthDate, BaseSalary
      - **SalesRecord** - Id, Date, Amount, Status(type: SaleStatus)
    - `enum` type
      - **SaleStatus** - Pending, Billed, Canceled
  
  - Association (let's use ICollection, which matches List, HashSet, etc. - INSTANTIATE!)
    - **Department** - `ICollection<Seller> Sellers` (relation a Department has several Sellers)
      - **Seller** - `Department Department` (relation a Seller has only a Department)
    - **Seller** - `ICollection<SalesRecord> Sales` (relation a Seller has several Sales)
      - **SalesRecord** - `Seller Seller` (relation a Sales Record has only a Seller)

  - Constructors
    - **default**
    - **with arguments** (include all attributes that are not collections)
  
  - Custom methods
    - **Seller** - AddSales, RemoveSales, TotalSales
    - **Department** - AddSaller, TotalSales
  
- Add DbSet's in DbContext (Data/SalesWebMVCContext)
  - `public DbSet<Seller> Seller { get; set; }`
  - `public DbSet<SalesRecord> SalesRecord { get; set; }`
  
- Update Database with Entity Framework
  - `Add-Migration [migration-name]`
  - `Update-Database`

## Seeding Service(to "populate" the database)

- Stop IIS
- In `Data` folder(same function as Repository folder), create SeedingService(same function as the [ModelName]Repository file)
- In `Program.cs`, register SeedingService for dependency injection system
- In `Program.cs`, add SeedingService as parameter of Configure method. Call Seed for development profile

## SellersController
- Create Departments and Sellers links on navbar (Views/Shared/_Layout.cshtml)
- Controller -> Add -> Controller -> MVC Controller - Empty -> SellersController
- Create folder Views/Sellers
- Views/Sellers -> Add -> View
  - View name: Index
  - Change title
  
## SellerService and basic FindAll

- Create folder Services
  - Create SellerService

- In `Program.cs`, register SellerService to dependency injection system(`builder.Services.AddScoped<SellerService, SellerService>();`)

- In SellerService, implement FindAll, returning List<Seller>
- In SellersController, implement Index method, which should call SellerService.FindAll

- In Views/Sellers/Index, write template code to show Sellers

- Suggestion: user classes "table-striped table-hover" for table
- Note: we're going to apply formatting in later classes
