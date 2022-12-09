using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Data;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            /*obj.Department = _context.Department.First();*/
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id) 
        {
            // return _context.Seller.FirstOrDefault(obj => obj.Id == id);
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); // JOIN com EF
        }

        public void Remove(int id) 
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id)) // testando se já existe algum registro dentro da tabela Selller cujo o Id é igual ao Id do obj passado
            {
                // se não existir (!) será lançada uma exception
                throw new NotFoundException("Id not found!");
            }
            try
            {
                // se passar pelo if quer dizer que já existe esse objeto, então podemos atualizá-lo
                _context.Update(obj);
                _context.SaveChanges();
            }
            // catch para capturar uma possível exception de concorrencia no banco de dados.
            // Aqui estamos interceptando uma exceção do nível de acesso a dados...
            catch (DbConcurrencyException e) 
            {
                throw new DbConcurrencyException(e.Message); // ...e relançando essa exceção, só que usando a exceção em nível de serviço,
                                                             // importante para segregar as camadas, ou seja, a nossa camada de serviço não vai propagar uma exceção de acesso a dados
                                                             // se uma exceção do nível de acesso a dados acontecer a camada de serviço vai lançar uma exceção da camada dela
                                                             // e o controlador Sellers só vai ter que lhe dar execeções da camada de serviço
                                                             // isso é feito pra seguir a arquitetura MVC, o controlador conversa com a camada de serviço/services,
                                                             // exceções do nível de acesso a dados(repositories) são capturados pelos services e relançadas na forma de exceções de serviço para o controlador 
            }
        }
    }
}
