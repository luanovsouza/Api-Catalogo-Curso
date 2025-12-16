using ApiCatalogo.Model;

namespace ApiCatalogo.Repositories.Interfaces;

public interface IProdutoRepository : IRepositoryGeneric<Produto>
{
    IEnumerable<Produto> ObterProdutosPorCategoria(int id);
}