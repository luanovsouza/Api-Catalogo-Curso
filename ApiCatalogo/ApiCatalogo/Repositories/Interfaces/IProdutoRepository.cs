using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces;

public interface IProdutoRepository : IRepositoryGeneric<Produto>
{
    Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int id);
    Task<PagedList<Produto>> GetProdutosAsync(ProdutoParameters produtoParameters);
    Task<PagedList<Produto>> GetProdutoFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
}