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

    public async Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int id)
    {
        var produtoPorCategoria = await GetAllAsync();
        
        var produtos = produtoPorCategoria.Where(p => p.CategoriaId == id);
        
        return produtos;
    }
    

    public async Task<PagedList<Produto>> GetProdutosAsync(ProdutoParameters produtoParameters)
    {
        var produtos = await GetAllAsync();

        var produtosOrdenado = produtos.OrderBy(p => p.Id).AsQueryable();
            
        var resultado = PagedList<Produto>.ToPagedList(produtosOrdenado, produtoParameters.PageNumber, produtoParameters.PageSize);
        
        return resultado;
    }

    public async Task<PagedList<Produto>> GetProdutoFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = await GetAllAsync();
        
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

        var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroPreco.PageNumber,
            produtosFiltroPreco.PageSize);
        
        return produtosFiltrados;
    }
}