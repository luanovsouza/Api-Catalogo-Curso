using ApiCatalogo.Context;
using ApiCatalogo.Enums;
using ApiCatalogo.Filters;
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
    

    public PagedList<Produto> GetProdutos(ProdutoParameters produtoParameters)
    {
        var produtos = GetAll().OrderBy(p => p.Id).AsQueryable();
        var produtosOrenados = PagedList<Produto>.ToPagedList(produtos, produtoParameters.PageNumber, produtoParameters.PageSize);
        return produtosOrenados;
    }

    public PagedList<Produto> GetProdutoFiltroPreco(ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = GetAll().OrderBy(p => p.Id).AsQueryable();
        
        if (produtosFiltroPreco.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPreco.PrecoCriterio.ToString()))
        {
            switch (produtosFiltroPreco.PrecoCriterio)
            {
                case PrecoCriterio.Maior:
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPreco.Preco).OrderBy(p => p.Id);
                    break;
                
                case PrecoCriterio.Menor:
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPreco.Preco).OrderBy(p => p.Id);
                    break;
                
                case PrecoCriterio.Igual:
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPreco.Preco).OrderBy(p => p.Id);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException("Esse argumento nao existe!");
            }
        }

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroPreco.PageNumber,
            produtosFiltroPreco.PageSize);
        
        return produtosFiltrados;
    }
}