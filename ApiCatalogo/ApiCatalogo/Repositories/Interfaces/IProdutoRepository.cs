using ApiCatalogo.Model;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces;

public interface IProdutoRepository : IRepositoryGeneric<Produto>
{
    IEnumerable<Produto> ObterProdutosPorCategoria(int id);
    //IEnumerable<Produto> GetProdutos(ProdutoParameters produtoParameters);
    
    PagedList<Produto> GetProdutos(ProdutoParameters produtoParameters);
}