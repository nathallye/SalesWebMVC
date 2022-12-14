# Sales Web MVC
Building a sales system using ASP.NET Core Web App.

<div align="center">
  <img width="800" src="https://user-images.githubusercontent.com/86172286/206882439-0b68ea2d-0f54-4232-ab98-0fa571c084f7.gif">
</div>
                                                                                                                             
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
- In `Program.cs`, add SeedingService as parameter of Configure method. Call Seed for development profile.

## SellersController
- Create Departments and Sellers links on navbar (Views/Shared/_Layout.cshtml)
- `Controller` > `Add` > `Controller` > `MVC Controller - Empty` > `SellersController`
- Create folder Views/Sellers
- Views/Sellers -> `Add` -> `View`
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
  
  ``` C#
  public List<Seller> FindAll()
  {
      return _context.Seller.ToList();
  }
  ```
  
- In Services/SellersController, implement `Index` method, which should call SellerService.FindAll
  
  ``` C#
  public IActionResult Index()
  {
      var list = _sellerService.FindAll();
      return View(list);
  }
  ```

- In Views/Sellers/Index, write template code to show Sellers

- Suggestion: user classes "table-striped table-hover" for table
- Note: we're going to apply formatting in later classes

## Simple Create form (Seller)

- In Views/Sellers/Index, create link to `Create`
- In Sellers controller, implement `Create` GET action
  
  ``` C#
  public IActionResult Create() 
  {
      var departments = _departmentService.FindAll();
      var viewModel = new SellerFormViewModel { Departments = departments };
      return View(viewModel); // agora a tela de cadastro j?? vai receber a lista de departamentos existentes
  }
  ```
  
- In Views/Sellers, create `Create` view
- In Services/SellerService create `Insert` method
  
  ``` C#
  public void Insert(Seller obj)
  {
      _context.Add(obj);
      _context.SaveChanges();
  }
  ```
  
- In Sellers controller, implement `Create` POST action
  
  ``` C#
  [HttpPost]
  [ValidateAntiForgeryToken] // evitar ataques do tipo xsrf
  public IActionResult Create(Seller seller)
  {
      _sellerService.Insert(seller);
      return RedirectToAction(nameof(Index)); // nameof - para previnir caso essa view tenha o nome trocado n??o quebre o c??digo
  }
  ```

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
  
- In Sellers controller:
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
        return View(viewModel); // agora a tela de cadastro j?? vai receber a lista de departamentos existentes
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
            @*asp-items - vai construir os itens da caixa de sele????o com base na cole????o, 
            o primeiro argumento do SelectList vai ser a cole????o, o segundo a chave, e o terceiro o valor que vai aparecer na lista*@
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
  
- In `Sellers Controller`, create `Delete` GET action
  
  ``` C#
  // Action Delete com o m??todo get s?? para exibirmos a tela confirma????o
  public IActionResult Delete(int? id)
  {
      if (id == null) // testando se o id foi passado
      {
          return NotFound();
      }

      var obj = _sellerService.FindById(id.Value);
      if (obj == null) // testando se o id passado existe
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

- In `Sellers Controller`, create `Delete` POST action
  
  ``` C#
  // Action Delete com o m??todo post para deletarmos de fato e redirecionar
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult Delete(int id)
  {

      _sellerService.Remove(id);
      return RedirectToAction(nameof(Index));
  }
  ```
  
- Test App

## Seller details and eager loading
  
- In View/Sellers/Index, check link to `Details` action
  
  ``` RAZOR
  <a asp-action="Details" asp-route-id="@item.Id">Details</a>
  ```
  
- In `Sellers Controller`, create `Details` GET action
  
  ``` C#
  public IActionResult Details(int? id) 
  {
      if (id == null) // testando se o id foi passado
      {
          return NotFound();
      }

      var obj = _sellerService.FindById(id.Value);
      if (obj == null) // testando se o id passado existe
      {
          return NotFound();
      }

      return View(obj);
  }
  ```
  
- Create view: View/Sellers/Details

- https://docs.microsoft.com/en-us/ef/core/querying/related-data
- In SellerService include in FindById: Include(obj => obj.Department) (namespace: Microsoft.EntityFrameworkCore)
  
  ``` C#
  public Seller FindById(int id) 
  {
      // return _context.Seller.FirstOrDefault(obj => obj.Id == id);
      return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); // JOIN com EF
  }
  ```

## Update seller and custom service exception

- Create Services/Exceptions folder
- Create NotFoundException and DbConcurrencyException
  
  ``` C#
  namespace SalesWebMVC.Services.Exceptions
  {
      public class NotFoundException : ApplicationException
      {
          public NotFoundException(string message) : base(message)
          { 

          }
      }
  }
  ```
  
  ``` C#
  namespace SalesWebMVC.Services.Exceptions
  {
      public class DbConcurrencyException : ApplicationException
      {
          public DbConcurrencyException(string message) : base(message)
          { 

          }
      }
  }
  ```
  
- In `SellerService`, create `Update` method
  
  ``` C#
  public void Update(Seller obj)
  {
      if (!_context.Seller.Any(x => x.Id == obj.Id)) // testando se j?? existe algum registro dentro da tabela Selller cujo o Id ?? igual ao Id do obj passado
      {
          // se n??o existir (!) ser?? lan??ada uma exception
          throw new NotFoundException("Id not found!");
      }
      try
      {
          // se passar pelo if quer dizer que j?? existe esse objeto, ent??o podemos atualiz??-lo
          _context.Update(obj);
          _context.SaveChanges();
      }
      // catch para capturar uma poss??vel exception de concorrencia no banco de dados.
      // Aqui estamos interceptando uma exce????o do n??vel de acesso a dados...
      catch (DbConcurrencyException e) 
      {
          throw new DbConcurrencyException(e.Message); // ...e relan??ando essa exce????o, s?? que usando a exce????o em n??vel de servi??o,
                                                       // importante para segregar as camadas, ou seja, a nossa camada de servi??o n??o vai propagar uma exce????o de acesso a dados
                                                       // se uma exce????o do n??vel de acesso a dados acontecer a camada de servi??o vai lan??ar uma exce????o da camada dela
                                                       // e o controlador Sellers s?? vai ter que lhe dar exece????es da camada de servi??o
                                                       // isso ?? feito pra seguir a arquitetura MVC, o controlador conversa com a camada de servi??o/services,
                                                       // exce????es do n??vel de acesso a dados(repositories) s??o capturados pelos services e relan??adas na forma de exce????es de servi??o para o controlador 
      }
  }
  ```
  
- In View/Sellers/Index, check link to `Edit` action
  
  ``` RAZOR
  <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm btn-success">Edit</a>
  ```
  
- In `Sellers Controller`, create `Edit` GET action
  
  ``` C#
  public IActionResult Edit(int? id)
  {
      if (id == null) // testando se o id foi passado
      {
          return NotFound();
      }

      var obj = _sellerService.FindById(id.Value);
      if (obj == null) // testando se o id passado existe
      {
          return NotFound();
      }

      // se passar pelas condi????es acima podemos atualizar o obj
      List<Department> departments = _departmentService.FindAll(); // popular a lista de departamentos
      SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; // criar a view model j?? passando os dados do obj e a lista de departamentos
      return View(viewModel);
  }
  ```
  
- Create view: View/Sellers/Edit (similar do Create, plus hidden id)
  
  ``` RAZOR
  <input type="hidden" asp-for="Seller.Id" />
  ```
  
- Test app

- In `Sellers Controller`, create `Edit` POST action
  
  ``` C#
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult Edit(int id, Seller seller)
  {
      if (id != seller.Id) // se o id passado como par??metro para action for diferente do Id do seller
      {
          // retorna bad request
          return BadRequest();
      }
      try
      {
          // se passar pela condi????o acima podemos atualizar o seller
          _sellerService.Update(seller);
          return RedirectToAction(nameof(Index));
      }
      catch (NotFoundException)
      {
          return NotFound();
      }
      catch (DbConcurrencyException)
      {
          return BadRequest();
      }
  }
  ```
  
- Test app
  
- Notice: ASP.NET Core selects option based on `DepartmentId`
  
## Returning custom error page

- Update `ErrorViewModel.cs` in Models/ViewModels
  
  ``` C#
  namespace SalesWebMVC.Models.ViewModels
  {
      public class ErrorViewModel
      {
          public string? RequestId { get; set; }
          public string Message { get; set; }

          public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
      }
  }
  ```
  
- Update `Error.cshtml` in Views/Shared
  
  ``` RAZOR
  @model SalesWebMVC.Models.ViewModels.ErrorViewModel;

  @{
      ViewData["Title"] = "Error";
  }

  <h1 class="text-danger">@ViewData["Title"].</h1>
  <h2 class="text-danger">@Model.Message</h2>

  @if (Model.ShowRequestId)
  {
      <p>
          <strong>Request ID:</strong> <code>@Model.RequestId</code>
      </p>
  }
  ```
  
- In `Sellers Controller`:
  - Create Error action with message parameter
  
    ``` C#
    public IActionResult Error(string message)
    {
        var viewModel = new ErrorViewModel
        {
            Message = message,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // "macete" do framework para pegar o Id interno da requisi????o
        };

        return View(viewModel);
    }
    ```
  
  - Update method calls
    - Swap calls from NotFound() and BadRequest() to the `RedirectToAction(Error)` action.

## App locale, number and date formatting
  
- In Program.cs, define localization options
  
  ``` C#
  // Config locale
  var enUS = new CultureInfo("en-US");
  var localizationOptions = new RequestLocalizationOptions
  {
      DefaultRequestCulture = new RequestCulture(enUS),
      SupportedCultures = new List<CultureInfo> { enUS },
      SupportedUICultures = new List<CultureInfo> { enUS }
  };
  ```
  
- Update Model Seller:
  - Define custom labels [Display]
  
    ``` C#
    public class Seller
    {
      // ...
      [Display(Name = "E-mail")]
      public string Email { get; set; }

      [Display(Name = "Birth Date")]
      public DateTime BirthDate { get; set; }

      [Display(Name = "Base Salary")]
      public double BaseSalary { get; set; }
      // ...
    }
    ```
  
  - Define semantics for date [DataType]
  
    ``` C#
    public class Seller
    {
      // ...
      [Display(Name = "E-mail")]
      [DataType(DataType.EmailAddress)]
      public string Email { get; set; }

      [Display(Name = "Birth Date")]
      [DataType(DataType.Date)]
      // ...
    }
    ```
  
  - Define display formats [DisplayFormat]
  
    ``` C#
    public class Seller
    {
      // ...
      [Display(Name = "Birth Date")]
      [DataType(DataType.Date)]
      [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
      public DateTime BirthDate { get; set; }

      [Display(Name = "Base Salary")]
      [DisplayFormat(DataFormatString = "{0:F2}")]
      public double BaseSalary { get; set; }
      // ...
    }
    ```
  
## Validation
  
- In Seller, add validation annotations

  - `[Required(ErrorMessage = "{0} required")`
  
    ``` C#
    public class Seller
    {
      // ...
      [Required(ErrorMessage = "{0} required")] // {0} pega o nome do atributo
      [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} e and {1}.")] // {2} pega o MinimumLength, {1} o tamanho m??ximo determinado 
      public string Name { get; set; }

      [Required(ErrorMessage = "{0} required")]
      [Display(Name = "E-mail")]
      [DataType(DataType.EmailAddress)]
      public string Email { get; set; }

      [Required(ErrorMessage = "{0} required")]
      [Display(Name = "Birth Date")]
      [DataType(DataType.Date)]
      [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
      public DateTime BirthDate { get; set; }

      [Required(ErrorMessage = "{0} required")]
      [Display(Name = "Base Salary")]
      [DisplayFormat(DataFormatString = "{0:F2}")]
      // ...
    }
    ```
  
  - `[EmailAddress(ErrorMessage = "Enter a valid email")]`
    
    ``` C#
    public class Seller
    {
      // ...
      [Required(ErrorMessage = "{0} required")]
      [EmailAddress(ErrorMessage = "Enter a valid email")]
      [Display(Name = "E-mail")]
      [DataType(DataType.EmailAddress)]
      public string Email { get; set; }
      // ...
    }
    ```
  
  - `[Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]`
    
    ``` C#
    public class Seller
    {
      // ...
      [Required(ErrorMessage = "{0} required")]
      [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
      [Display(Name = "Base Salary")]
      [DisplayFormat(DataFormatString = "{0:F2}")]
      public double BaseSalary { get; set; }
      // ...
    }
    ```
  
- Update HTML for Create and Edit view

  - Summary:

    ``` RAZOR
    <div asp-validation-summary="All" class="text-danger"></div>
    ```

  - Client-side validation. To work validation even when javascript is disabled, the framework does this work(paste code snippet at end of view):

  ``` RAZOR
  @section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
  }
  ```
  
  - Field:
    
    ``` RAZOR
    <span asp-validation-for="Name" class="text-danger"></span>
    ```
  
 - Update SellersController in Create and Edit actions (to prevent empty form submission when browser javascript is disabled)
  
  ``` C#
  [HttpPost]
  [ValidateAntiForgeryToken] // evitar ataques do tipo xsrf
  public IActionResult Create(Seller seller)
  {
      if (!ModelState.IsValid)
      {
          var departments = _departmentService.FindAll();
          var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
          return View(viewModel); // vai ficar retornando para a view com os dados que j?? foram preenchidos, at?? que todos estejam v??lidos
      }

      _sellerService.Insert(seller);
      return RedirectToAction(nameof(Index)); // nameof - para previnir caso essa view tenha o nome trocado n??o quebre o c??digo
  }
  ```
  
## Asynchronous operations using Tasks (async, await)

- Update DepartmentService
  
  ``` C#
  public class DepartmentService
  {
      private readonly SalesWebMVCContext _context;

      public DepartmentService(SalesWebMVCContext context)
      {
          _context = context;
      }

      // sync
      /*public List<Department> FindAll()
      {
          return _context.Department.OrderBy(x => x.Name).ToList();
      }*/

      // async
      public async Task<List<Department>> FindAllAsync()
      {
          return await _context.Department.OrderBy(x => x.Name).ToListAsync();
      }
  }
  ```
  
- Update SellerService
- Update SellersController
  
## Exception handling for delete (referential integrity)
  
- Create custom exception IntegrityException
  
  ``` C#
  namespace SalesWebMVC.Services.Exceptions
  {
      public class IntegrityException : ApplicationException
      {
          public IntegrityException(string message) : base(message) 
          { 
          }
      }
  }
  ```
  
- In SellerService in RemoveAsync action, catch DbUpdateException and throw IntegrityException
  
  ``` C#
  public async Task RemoveAsync(int id)
  {
      try
      {
          var obj = _context.Seller.Find(id);
          _context.Seller.Remove(obj);
          await _context.SaveChangesAsync();
      }
      catch(DbUpdateException e) 
      {
          throw new IntegrityException(e.Message);
      }
  }
  ```
  
- In SellersController, update Delete POST action
  
  ``` C#
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete(int id)
  {
      try
      {
          await _sellerService.RemoveAsync(id);
          return RedirectToAction(nameof(Index));
      }
      catch (IntegrityException e)
      {
          return RedirectToAction(nameof(Error), new { message = e.Message });
      }
  }
  ```

## Preparing sales search navigation views
  
- Create SalesRecordsController with Index, SimpleSearch and GroupingSearch action
  
  ``` C#
  namespace SalesWebMVC.Controllers
  {
      public class SalesRecordsController : Controller
      {
          public IActionResult Index()
          {
              return View();
          }

          public IActionResult SimpleSearch()
          { 
              return View();
          }

          public IActionResult GroupingSearch()
          {
              return View();
          }
      }
  }
  ```
  
 - Create folder Views/SalesRecords
   - Create Index view with search forms
   - Create SimpleSearch and GroupingSearch views
  
 - Create "Sales" link on main navbar(Views/Shared/_Layout.cshtml)
  
  ``` RAZOR
  <li class="nav-item">
      <a class="nav-link text-white" asp-area="" asp-controller="SalesRecords" asp-action="Index">Sales</a>
  </li>
  ```
  
## Implementing simple search
  
- Create SalesRecordService with FindByDate operation
  
  ``` C#
  namespace SalesWebMVC.Services
  {
      public class SalesRecordService
      {
          private readonly SalesWebMVCContext _context;

          public SalesRecordService(SalesWebMVCContext context)
          {
              _context = context;
          }

          public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
          {
              var result = from obj in _context.SalesRecord select obj; // essa declara????o vai pegar esse SalesRecord que ?? do tipo DbSet,
                                                                        // e vai construir um obj result do tipo IQueryable
                                                                        // e "em cima" desse obj result vamos poder acrescentar detalhes da consulta
              if (minDate.HasValue) // se a data m??nima existir(foi passada) 
              {
                  result = result.Where(x => x.Date >= minDate.Value); // restri????o de data m??nima
              }
              if (maxDate.HasValue) // se a data m??xima existir(foi passada)
              {
                  result = result.Where(x => x.Date <= maxDate.Value); // restri????o de data m??xima
              }
              return await result
                  .Include(x => x.Seller) // join com a tabela Seller
                  .Include(x => x.Seller.Department) // join com a tabela Department
                  .OrderByDescending(x => x.Date)
                  .ToListAsync();
          }
      }
  }
  ```
  
- In Prohram.cs, register SalesRecordService to dependency injection system
  
  ``` C#
  builder.Services.AddScoped<SalesRecordService, SalesRecordService>();
  ```
  
- In SalesRecordsController, update SimpleSearch action
  
  ``` C#
  public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
  {
      if (!minDate.HasValue) // testa se a data m??nima existe, se n??o foi 
      {
          minDate = new DateTime(DateTime.Now.Year, 1, 1); // ser?? enviada uma data padr??o (ano atual, m??s 1, dia 1)
      }
      if (!maxDate.HasValue) // testa se a data m??xima existe, se n??o foi
      {
          maxDate = DateTime.Now; // ser?? enviada uma data padr??o(data atual)
      }
  
      ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); // envia a data m??nima formatada para a View 
      ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd"); // envia a data m??xima formatada para a View 

      var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
      return View(result);
  }
  ```
  
- Update SimpleSearch view
  
## Implementing grouping search
  
- In SalesRecordService create FindByDateGrouping operation
  
  ``` C#
  public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
  {
      var result = from obj in _context.SalesRecord select obj;
      if (minDate.HasValue)
      {
          result = result.Where(x => x.Date >= minDate.Value);
      }
      if (maxDate.HasValue)
      {
          result = result.Where(x => x.Date <= maxDate.Value);
      }
      return await result
          .Include(x => x.Seller)
          .Include(x => x.Seller.Department)
          .OrderByDescending(x => x.Date)
          .GroupBy(x => x.Seller.Department)
          .ToListAsync();
  }
  ```
  
- In SalesRecordsController, update GroupingSearch action
  
  ``` C#
  public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
  {
      if (!minDate.HasValue)
      {
          minDate = new DateTime(DateTime.Now.Year, 1, 1);
      }
      if (!maxDate.HasValue)
      {
          maxDate = DateTime.Now;
      }

      ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
      ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

      var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
      return View(result);
  }
  ```
  
- Update GroupingSearch view
