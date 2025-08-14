using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController(IRepository<Categoria> repository) : ControllerBase
    {
        private readonly IRepository<Categoria> _repository = repository;

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _repository.Get(c => c.Id == id);
            if (categoria is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoriaCriada.Id }, categoriaCriada);            
        }

        [HttpPut("{id:int}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            if (categoria is null)
                return BadRequest("Categoria não pode ser nula");

            if (id != categoria.Id)
                return BadRequest("Há divergência nos Ids informados!");

            var categoriaExiste = _repository.Get(c => c.Id == id);

            if (categoriaExiste is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            var categoriaAtualizada = _repository.Update(categoria);

            return Ok(categoriaAtualizada);                       
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _repository.Get(c => c.Id == id);
            if (categoria is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            var categoriaExcluida = _repository.Delete(categoria);
            
            return Ok(categoriaExcluida);
        }
    }
}
