# SalesWebMVC
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
