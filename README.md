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
    ``` C#
    public DbSet<Seller> Seller { get; set; }
    public DbSet<SalesRecord> SalesRecord { get; set; }
    ```
  
- Update Database with Entity Framework
  - `Add-Migration [migration-name]`
  - `Update-Database`

## SeedingService(to "populate" the database)

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

- In `Program.cs`, register SellerService to dependency injection system
   
  ``` C#
  builder.Services.AddScoped<SellerService, SellerService>();`
  ```

- In Services/SellerService, implement `FindAll`, returning List<Seller>
- In Services/SellersController, implement `Index` method, which should call SellerService.FindAll

- In Views/Sellers/Index, write template code to show Sellers

- Suggestion: user classes "table-striped table-hover" for table
- Note: we're going to apply formatting in later classes

## Simple Create form (Seller)

- In Views/Sellers/Index, create link to `Create`
- In Sellers controller, implement `Create` GET action
- In Views/Sellers, create `Create` view
- In Services/SellerService create `Insert` method
- In Sellers controller, implement `Create` POST action

## Foreign key not null (referential integrity)

- In `Seller Model`, add `DepartmentId`
  
- Create new migration, update database
  - `Add-Migration [migration-name]`
  - `Update-Database`
  
- Update `SellerService.Insert` for now: 
  
  ``` C#
  obj.Department = _context.Department.First();
  ```
  - To assign to this seller the id of the first department of the table

## SellerFormViewModel and Department select component

- Create `DepartmentService` with `FindAll` method

- In `Program.cs`, register `DepartmentService` to dependency injection system
  
  ``` C#
  builder.Services.AddScoped<DepartmentService, DepartmentService>();
  ```

- Create `SellerFormViewModel`
  - Attributes Seller, ICollection<Department> Departments
  
- In Seller controller:
  - New dependency: DepartmentService
    
    ``` C#
    public readonly DepartmentService _departmentService;
    ```
  
  - Update `Create` GET action
  
    ``` C#
    public IActionResult Create() 
      {
        var departments = _departmentService.FindAll();
        var viewModel = new SellerFormViewModel { Departments = departments };
        return View(viewModel); // agora a tela de cadastro já vai receber a lista de departamentos existentes
      }
    ```
  
- In Views/Sellers/Create:
  - Update model type to SellerFormViewModel
    
    ``` RAZOR
    @model SalesWebMVC.Models.ViewModels.SellerFormViewModel;
    ```
    
  - Update form fields
    - `asp-for="Seller.[attribute-name]"`
  
  - Add select component for `DepartmentId`
  
    ``` RAZOR
    <div class="form-group">
        <label asp-for="Seller.DepartmentId" class="control-label"></label>
        <select asp-for="Seller.DepartmentId"
            asp-items="@(new SelectList(Model.Departments,"Id", "Name"))" class="form-control"></select>
            @*asp-items - vai construir os itens da caixa de seleção com base na coleção, 
            o primeiro argumento do SelectList vai ser a coleção, o segundo a chave, e o terceiro o valor que vai aparecer na lista*@
    </div>
    ```
  
 - In `SellerService` Action `Insert`, delete "First" call
  
  ``` C#
  public void Insert(Seller obj)
  {
    /*obj.Department = _context.Department.First();*/
    _context.Add(obj);
    _context.SaveChanges();
  }
  ```
 
 - Reference: https://stackoverflow.com/questions/34624034/select-tag-helper-in-asp-net-core-mvc
 
## Delete seller

- In `SellerService`, create `FindById` and `Remove` operations
  
  ``` C#
  public Seller FindById(int id) 
  {
      return _context.Seller.FirstOrDefault(obj => obj.Id == id);
  }

  public void Remove(int id) 
  {
      var obj = _context.Seller.Find(id);
      _context.Seller.Remove(obj);
      _context.SaveChanges();
  }
  ```
  
- In `Seller Controller`, create `Delete` GET action
  
  ``` C#
  // Action Delete com o método get só para exibirmos a tela confirmação
  public IActionResult Delete(int? id)
  {
      if (id == null)
      {
          return NotFound();
      }

      var obj = _sellerService.FindById(id.Value);
      if (obj == null) 
      { 
          return NotFound();
      }

      return View(obj);
  }
  ```
  
- In View/Sellers/Index, check link to `Delete` action
  
  ``` RAZOR
  <a asp-action="Delete" asp-route-id="@item.Id">Delete</a>
  ```
  
- Create delete confirmation view: 
  - View/Sellers/Delete
- Test App

- In `Seller Controller`, create `Delete` POST action
- Test App
