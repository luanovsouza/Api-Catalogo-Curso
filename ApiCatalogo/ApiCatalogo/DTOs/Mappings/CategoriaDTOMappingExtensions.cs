using ApiCatalogo.Model;

namespace ApiCatalogo.DTOs.Mappings;

public static class CategoriaDtoMappingExtensions
{
    public static CategoriaDto? ToCategoriaDto(this Categoria? categoria)
    {
        if (categoria == null)
            return null;

        return new CategoriaDto
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        };
    }


    public static Categoria? ToCategoria(this CategoriaDto? categoriaDto)
    {
        if (categoriaDto is null)
            return null;
        
        return new Categoria
        {
            CategoriaId = categoriaDto.CategoriaId,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl,
        };
    }


    public static IEnumerable<CategoriaDto> ToCategoriasDtoList(this IEnumerable<Categoria>? categorias)
    {
        if(categorias is null || !categorias.Any())
            return new List<CategoriaDto>();

        return categorias.Select(categoria => new CategoriaDto
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        }).ToList();
    }
}