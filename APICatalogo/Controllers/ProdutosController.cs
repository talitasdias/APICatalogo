using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController(IRepository<Produto> repository, IProdutoRepository produtoRepository) : ControllerBase
    {
        private readonly IRepository<Produto> _repository = repository;
        private readonly IProdutoRepository _produtoRepository = produtoRepository;

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorIdCategoria(id);
            
            if (produtos is null)
                return NotFound();
            return Ok(produtos);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetAll();
            if (produtos is null)
                return NotFound("Produtos não encontrados!");
            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _repository.Get(p => p.Id == id);
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

            var produtoExiste = _repository.Get(p => p.Id == id);

            if (produtoExiste is null)
                return NotFound($"Produto de id {id} não encontrado!");

            var produtoAtualizado = _repository.Update(produto);

            return Ok(produtoAtualizado);         
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.Get(p => p.Id == id);
            if (produto is null)
                return NotFound($"Produto de id {id} não encontrado!");

            var produtoDeletado = _repository.Delete(produto);
            
            return Ok("Produto deletado com sucesso!");            
        }
    }
}
