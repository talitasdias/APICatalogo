using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public IEnumerable<Produto> GetProdutosPorIdCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id);
    }
}