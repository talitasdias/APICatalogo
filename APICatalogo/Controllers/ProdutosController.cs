using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos is null)
                return NotFound("Produtos não encontrados!");
            return produtos;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _context.Produtos.SingleOrDefault(p => p.Id == id);
            if (produto is null)
                return NotFound($"Produto de Id {id} não encontrado!");
            return produto;
        }
    }
}
