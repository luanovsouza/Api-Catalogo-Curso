using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces;

public interface ICategoriaRepository : IRepositoryGeneric<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters);
    
    PagedList<Categoria> GetCategoriaFiltroNome(CategoriaFiltroNome categoriaFiltroNome);
}