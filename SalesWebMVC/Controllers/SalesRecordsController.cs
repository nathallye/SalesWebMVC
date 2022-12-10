using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue) // testa se a data mínima existe, se não foi 
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1); // será enviada uma data padrão (ano atual, mês 1, dia 1)
            }
            if (!maxDate.HasValue) // testa se a data máxima existe, se não foi
            {
                maxDate = DateTime.Now; // será enviada uma data padrão(data atual)
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); // envia a data mínima formatada para a View 
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd"); // envia a data máxima formatada para a View 

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public IActionResult GroupingSearch()
        {
            return View();
        }
    }
}
