using ApiCatalogo.Context;
using ApiCatalogo.Enums;
using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : RepositoryGeneric<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) :  base(context)
    {
    }

    public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters)
    {
        var categorias = await GetAllAsync();
        
        var categoriaOrdenada = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

        var resultado = PagedList<Categoria>.ToPagedList(categoriaOrdenada, categoriaParameters.PageNumber, categoriaParameters.PageSize);
        
        return resultado;
    }

    public async Task<PagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriaFiltroNome categoriaFiltroNome)
    {
        var categoria = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriaFiltroNome.Nome))
        {
            categoria = categoria.Where(c => c.Nome.Contains(categoriaFiltroNome.Nome));
        }
        
        var categoriaPaginada = PagedList<Categoria>.ToPagedList(categoria.AsQueryable(), categoriaFiltroNome.PageNumber,
            categoriaFiltroNome.PageSize);

        return categoriaPaginada;
    }
}