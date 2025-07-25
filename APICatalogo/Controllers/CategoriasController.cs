using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.ToList();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.AsNoTracking().SingleOrDefault(c => c.Id == id);
            if (categoria is null)
                return NotFound($"Categoria de id {id} não encontrado!");

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new {id = categoria.Id}, categoria);
        }
    }
}
