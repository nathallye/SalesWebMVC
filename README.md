# Sales Web MVC
Building a sales system using ASP.NET Core Web App.

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

  ![image](https://user-images.githubusercontent.com/86172286/205783054-b25c2023-8088-4fc0-b4e8-cfa4ab606c52.png)

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
