using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.Id).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
        return produtosOrdenados;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco prodFiltroPreco)
    {
        var produtos = GetAll().AsQueryable();
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

    public IEnumerable<Produto> GetProdutosPorIdCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id);
    }
}