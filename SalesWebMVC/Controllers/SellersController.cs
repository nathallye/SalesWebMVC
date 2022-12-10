using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

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

        // sync
        /*public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }*/

        // async
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel); // agora a tela de cadastro já vai receber a lista de departamentos existentes
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // evitar ataques do tipo xsrf
        public async Task<IActionResult> Create(Seller seller)
        {
            /*if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); // vai ficar retornando para a view com os dados que já foram preenchidos, até que todos estejam válidos
            }*/

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index)); // nameof - para previnir caso essa view tenha o nome trocado não quebre o código
        }

        // Action Delete com o método get só para exibirmos a tela confirmação
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // testando se o id foi passado
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) // testando se o id passado existe
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // Action Delete com o método post para deletarmos de fato e redirecionar
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // testando se o id foi passado
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) // testando se o id passado existe
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // se passar pelas condições acima podemos atualizar o obj
            List<Department> departments = await _departmentService.FindAllAsync(); // popular a lista de departamentos
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; // criar a view model já passando os dados do obj e a lista de departamentos
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            /*if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); // vai ficar retornando para a view com os dados que já foram preenchidos, até que todos estejam válidos
            }*/

            if (id != seller.Id) // se o id passado como parâmetro para action for diferente do Id do seller
            {
                // retorna bad request
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                // se passar pela condição acima podemos atualizar o seller
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // "macete" do framework para pegar o Id interno da requisição
            };

            return View(viewModel);
        }
    }
}
