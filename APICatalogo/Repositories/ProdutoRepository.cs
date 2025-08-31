using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    /* public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        return GetAll().OrderBy(p => p.Nome).Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize).
        Take(produtosParams.PageSize).ToList();
    } */
    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.Id).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
        return produtosOrdenados;
    }

    public IEnumerable<Produto> GetProdutosPorIdCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id);
    }
}