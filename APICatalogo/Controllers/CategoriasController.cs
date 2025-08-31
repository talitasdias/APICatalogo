using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController(IUnitOfWork uof) : ControllerBase
    {
        private readonly IUnitOfWork _uof = uof;

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters catParams)
        {
            var categorias = _uof.CategoriaRepository.GetCategorias(catParams);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metadata);

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            if (!categorias.Any())
                return Ok(new List<CategoriaDTO>());

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            var categoriaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null) return BadRequest();

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

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

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

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

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();
            
            return Ok(categoriaExcluidaDto);
        }
    }
}
