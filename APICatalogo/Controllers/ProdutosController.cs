using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var produtos = _context.Produtos?.AsNoTracking().ToList();
            if (produtos is null)
                return NotFound("Produtos não encontrados!");
            return produtos;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos?.AsNoTracking().SingleOrDefault(p => p.Id == id);
            if (produto is null)
                return NotFound($"Produto de Id {id} não encontrado!");
            return produto;
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Produtos?.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Produto> Put(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest("Há divergência nos IDs informados!");

            var produtoExistente = _context.Produtos?.AsNoTracking().FirstOrDefault(p => p.Id == id);

            if (produtoExistente is null)
                return NotFound($"Produto de Id {id} não encontrado!");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos?.SingleOrDefault(p => p.Id == id);
            if (produto is null)
                return NotFound($"Produto de id {id} não encontrado!");

            _context.Produtos?.Remove(produto);
            _context.SaveChanges();

            return Ok("Produto deletado com sucesso!");            
        }
    }
}
