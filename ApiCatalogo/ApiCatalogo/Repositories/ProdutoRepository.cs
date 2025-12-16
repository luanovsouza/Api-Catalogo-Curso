using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class ProdutoRepository : RepositoryGeneric<Produto>, IProdutoRepository
{
    

    public ProdutoRepository(AppDbContext context) : base(context) // To usando o contexto da classe base
    //Repository Generic
    {
    }

    public IEnumerable<Produto> ObterProdutosPorCategoria(int id)
    {
        return GetAll().Where(p => p.CategoriaId == id);
    }
}