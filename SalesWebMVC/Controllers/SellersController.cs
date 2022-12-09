using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {

        public readonly SellerService _sellerService;
        public readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create() 
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel); // agora a tela de cadastro já vai receber a lista de departamentos existentes
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // evitar ataques do tipo xsrf
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); // nameof - para previnir caso essa view tenha o nome trocado não quebre o código
        }

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

        // Action Delete com o método post para deletarmos de fato e redirecionar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id) 
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

        public IActionResult Edit(int? id)
        {
            if (id == null) // testando se o id existe
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null) // testando se o obj existe
            {
                return NotFound();
            }

            // se passar pelas condições acima podemos atualizar o obj
            List<Department> departments = _departmentService.FindAll(); // popular a lista de departamentos
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; // criar a view model já passando os dados do obj e a lista de departamentos
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id) // se o id passado como parâmetro para action for diferente do Id do seller
            {
                // retorna bad request
                return BadRequest();
            }
            try
            {
                // se passar pela condição acima podemos atualizar o seller
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
    }
}
