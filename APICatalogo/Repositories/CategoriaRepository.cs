using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;


namespace APICatalogo.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
    {
        var categorias = GetAll().OrderBy(c => c.Id).AsQueryable();
        var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);
        return categoriasOrdenados;
    }
}