using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController(IProdutoRepository repository) : ControllerBase
    {
        private readonly IProdutoRepository _repository = repository;

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos().ToList();
            if (produtos is null)
                return NotFound("Produtos não encontrados!");
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _repository.GetProduto(id);
            if (produto is null)
                return NotFound($"Produto de Id {id} não encontrado!");
            return produto;
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto não pode ser nulo.");

            var produtoCriado = _repository.Create(produto);

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

            var atualizado = _repository.Update(produto);

            if (!atualizado)
                return NotFound($"Produto de id {id} não encontrado!");

            return Ok(produto);         
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var deletado = _repository.Delete(id);
            
            if (!deletado)
                return NotFound($"Produto de id {id} não encontrado!");

            return Ok("Produto deletado com sucesso!");            
        }
    }
}
