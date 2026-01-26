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

    public PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters)
    {
        var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();
        var categoriasOrenadas = PagedList<Categoria>.ToPagedList(categorias, categoriaParameters.PageNumber, categoriaParameters.PageSize);
        return categoriasOrenadas;
    }

    public PagedList<Categoria> GetCategoriaFiltroNome(CategoriaFiltroNome categoriaFiltroNome)
    {
        var categoria = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

        if (!string.IsNullOrEmpty(categoriaFiltroNome.Nome))
        {
            categoria = categoria.Where(c => c.Nome.Contains(categoriaFiltroNome.Nome));
        }
        
        var categoriaPaginada = PagedList<Categoria>.ToPagedList(categoria, categoriaFiltroNome.PageNumber,
            categoriaFiltroNome.PageSize);

        return categoriaPaginada;
    }
}