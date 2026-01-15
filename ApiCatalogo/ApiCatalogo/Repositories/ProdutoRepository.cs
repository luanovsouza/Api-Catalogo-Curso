using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
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

    public IEnumerable<Produto> GetProdutos(ProdutoParameters produtoParameters)
    {
        return GetAll().OrderBy(p => p.Nome)
            .Skip((produtoParameters.PageNumber - 1) * produtoParameters.PageSize)
            .Take(produtoParameters.PageSize).ToList();
    }
}