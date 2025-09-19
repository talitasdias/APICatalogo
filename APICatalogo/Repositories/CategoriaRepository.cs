using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;


namespace APICatalogo.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
    {
        var categorias = (await GetAllAsync()).OrderBy(c => c.Id).AsQueryable();
        //var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);
        var categoriasOrdenados = await categorias.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);
        return categoriasOrdenados;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome catFiltroNome)
    {
        var categorias = (await GetAllAsync()).AsQueryable();
        if (!string.IsNullOrEmpty(catFiltroNome.Nome))
            categorias = categorias.Where(c => c.Nome != null && c.Nome.Contains(catFiltroNome.Nome, StringComparison.InvariantCultureIgnoreCase));

        //var categoriasOrdenados = PagedList<Categoria>.ToPagedList(categorias, catFiltroNome.PageNumber, catFiltroNome.PageSize);
        var categoriasOrdenados = await categorias.ToPagedListAsync(catFiltroNome.PageNumber, catFiltroNome.PageSize);
        return categoriasOrdenados;
    }
}