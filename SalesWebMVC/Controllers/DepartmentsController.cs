using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;

namespace SalesWebMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> departments = new List<Department>();
            departments.Add(new Department { Id = 1, Name = "Eletronics" }); // sintaxe de instânciação automática
            departments.Add(new Department { Id = 2, Name = "Fashion" });

            return View(departments); // enviando dados do controller para a view
        }
    }
}
