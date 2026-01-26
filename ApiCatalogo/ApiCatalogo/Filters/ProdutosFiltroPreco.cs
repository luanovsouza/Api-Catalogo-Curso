using ApiCatalogo.Enums;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Filters;

public class ProdutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public PrecoCriterio? PrecoCriterio{ get; set; } // Maior, menor ou igual
}