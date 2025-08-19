using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController(IUnitOfWork uof) : ControllerBase
    {
        private readonly IUnitOfWork _uof = uof;

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDto = categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                ImagemUrl = c.ImagemUrl
            });

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            var categoriaDto = new CategoriaDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            var categoria = new Categoria { Nome = categoriaDto.Nome, ImagemUrl = categoriaDto.ImagemUrl };

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = new Categoria { Id = categoriaCriada.Id, Nome = categoriaCriada.Nome, ImagemUrl = categoriaCriada.ImagemUrl };

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDto.Id }, novaCategoriaDto);            
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest("Categoria não pode ser nula");

            if (id != categoriaDto.Id)
                return BadRequest("Há divergência nos Ids informados!");

            var categoriaExiste = _uof.CategoriaRepository.Get(c => c.Id == id);

            if (categoriaExiste is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            var categoria = new Categoria { Id = categoriaDto.Id, Nome = categoriaDto.Nome, ImagemUrl = categoriaDto.ImagemUrl };

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = new CategoriaDTO { Id = categoriaAtualizada.Id, Nome = categoriaAtualizada.Nome, ImagemUrl = categoriaAtualizada.ImagemUrl };

            return Ok(categoriaAtualizadaDto);                
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDto = new CategoriaDTO { Id = categoriaExcluida.Id, Nome = categoriaExcluida.Nome, ImagemUrl = categoriaExcluida.ImagemUrl };
            
            return Ok(categoriaExcluidaDto);
        }
    }
}
