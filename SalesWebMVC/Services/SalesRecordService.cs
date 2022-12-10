using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

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
            var result = from obj in _context.SalesRecord select obj; // essa declaração vai pegar esse SalesRecord que é do tipo DbSet,
                                                                      // e vai construir um obj result do tipo IQueryable
                                                                      // e "em cima" desse obj result vamos poder acrescentar detalhes da consulta
            if (minDate.HasValue) // se a data mínima existir(foi passada) 
            {
                result = result.Where(x => x.Date >= minDate.Value); // restrição de data mínima
            }
            if (maxDate.HasValue) // se a data máxima existir(foi passada)
            {
                result = result.Where(x => x.Date <= maxDate.Value); // restrição de data máxima
            }
            return await result
                .Include(x => x.Seller) // join com a tabela Seller
                .Include(x => x.Seller.Department) // join com a tabela Department
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}
