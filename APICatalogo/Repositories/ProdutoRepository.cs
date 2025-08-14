using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
    private readonly AppDbContext _context = context;
    public IQueryable<Produto> GetProdutos()
    {
        return _context.Produtos;
    }

    public Produto? GetProduto(int id)
    {
        var produto = _context.Produtos?.FirstOrDefault(p => p.Id == id);

        if (produto is null)
            return null;

        return produto;
    }

    public Produto Create(Produto produto)
    {
        _context.Produtos?.Add(produto);
        _context.SaveChanges();

        return produto;
    }

    public bool Update(Produto produto)
    {
        if (_context.Produtos.Any(p => p.Id == produto.Id))
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool Delete(int id)
    {
        var produto = _context.Produtos?.Find(id);

        if (produto is not null)
        {
            _context.Produtos?.Remove(produto);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}