using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController(IUnitOfWork uof) : ControllerBase
    {
        private readonly IUnitOfWork _uof = uof;

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorIdCategoria(id);
            
            if (produtos is null)
                return NotFound();
            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.GetAll();
            if (produtos is null)
                return NotFound("Produtos não encontrados!");
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null)
                return NotFound($"Produto de Id {id} não encontrado!");
            return produto;
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto não pode ser nulo.");

            var produtoCriado = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produtoCriado.Id }, produtoCriado);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Produto> Put(int id, Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto não pode ser nulo!");

            if (id != produto.Id)
                return BadRequest("Há divergência nos IDs informados!");

            var produtoExiste = _uof.ProdutoRepository.Get(p => p.Id == id);

            if (produtoExiste is null)
                return NotFound($"Produto de id {id} não encontrado!");

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produtoAtualizado);         
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.Id == id);
            if (produto is null)
                return NotFound($"Produto de id {id} não encontrado!");

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            
            return Ok("Produto deletado com sucesso!");            
        }
    }
}
