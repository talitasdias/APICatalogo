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

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome catFiltroNome)
    {
        var categorias = GetAll().AsQueryable();
        if (!string.IsNullOrEmpty(catFiltroNome.Nome))
            categorias = categorias.Where(c => c.Nome.Contains(catFiltroNome.Nome, StringComparison.InvariantCultureIgnoreCase));

        var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias, catFiltroNome.PageNumber, catFiltroNome.PageSize);
        return categoriasOrdenados;
    }
}