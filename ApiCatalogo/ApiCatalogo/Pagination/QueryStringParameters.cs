namespace ApiCatalogo.Pagination;

public abstract class QueryStringParameters
{
    const int MaxPageSize = 50; // Maximo de itens por pagina
    public int PageNumber { get; set; } = 1; // Numero da pagina
    private int _pageSize = MaxPageSize; //tamanho de itens por pagina

    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}