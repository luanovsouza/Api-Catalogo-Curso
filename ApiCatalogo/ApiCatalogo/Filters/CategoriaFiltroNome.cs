using ApiCatalogo.Enums;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Filters;

public class CategoriaFiltroNome : QueryStringParameters
{
    public string? Nome { get; set; }
}