using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces;

public interface ICategoriaRepository : IRepositoryGeneric<Categoria>
{
    Task<PagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters);
    
    Task<PagedList<Categoria>> GetCategoriaFiltroNomeAsync(CategoriaFiltroNome categoriaFiltroNome);
}