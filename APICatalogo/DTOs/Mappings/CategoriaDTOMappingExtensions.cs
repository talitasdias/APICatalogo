using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings;

public static class CategoriaDTOMappingExtensions
{
    public static CategoriaDTO ToCategoriaDTO(this Categoria categoria)
    {
        return new CategoriaDTO
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
    }

    public static Categoria ToCategoria(this CategoriaDTO categoriaDto)
    {
        return new Categoria
        {
            Id = categoriaDto.Id,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };
    }

    public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
    {
        var categoriaDTOs = categorias.Select(c => new CategoriaDTO
        {
            Id = c.Id,
            Nome = c.Nome,
            ImagemUrl = c.ImagemUrl
        }).ToList();

        return categoriaDTOs;
    }
}