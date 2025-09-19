using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public async Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
        var produtos = (await GetAllAsync()).OrderBy(p => p.Id).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
        return produtosOrdenados;
    }

    public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco prodFiltroPreco)
    {
        var produtos = (await GetAllAsync()).AsQueryable();
        if (prodFiltroPreco.Preco.HasValue && !string.IsNullOrEmpty(prodFiltroPreco.PrecoCriterio))
        {
            if (prodFiltroPreco.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco > prodFiltroPreco.Preco).OrderBy(p => p.Preco);
            else if (prodFiltroPreco.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco < prodFiltroPreco.Preco).OrderBy(p => p.Preco);
            else if (prodFiltroPreco.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco == prodFiltroPreco.Preco).OrderBy(p => p.Preco);
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, prodFiltroPreco.PageNumber, prodFiltroPreco.PageSize);

        return produtosFiltrados;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorIdCategoriaAsync(int id)
    {
        return (await GetAllAsync()).Where(p => p.CategoriaId == id);
    }
}