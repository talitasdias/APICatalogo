using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;


namespace APICatalogo.Repositories;

public class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    private readonly AppDbContext _context = context;

    public IEnumerable<Categoria> GetCategorias()
    {
        return _context.Categorias?.AsNoTracking().ToList() ?? new List<Categoria>();
    }

    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
        return _context.Categorias?.AsNoTracking().Include(c => c.Produtos).ToList() ?? new List<Categoria>();
    }

    public Categoria? GetCategoria(int id)
    {
        return _context.Categorias?.AsNoTracking().SingleOrDefault(c => c.Id == id);
    }

    public Categoria Create(Categoria categoria)
    {
        ArgumentNullException.ThrowIfNull(categoria);

        _context.Categorias?.Add(categoria);
        _context.SaveChanges();

        return categoria;
    }

    public Categoria Update(Categoria categoria)
    {
        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return categoria;
    }

    public Categoria Delete(int id)
    {
        var categoria = _context.Categorias?.Find(id);

        ArgumentNullException.ThrowIfNull(categoria);

        _context.Remove(categoria);
        _context.SaveChanges();

        return categoria;
    }
}