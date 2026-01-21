namespace ApiCatalogo.Pagination;

public class PagedList<T> : List<T> where T : class
{
    public int CurrentPage { get; set; } // Pagina atual
    public int TotalPages { get; set; } // Total de paginas
    public int PageSize { get; set; } // Numero de itens exibidos
    public int TotalCount { get; set; } // Numero total de elementos da fonte de dados
    
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize); // Calculando o tamanho total de paginas
        //dividindo o total de paginas dividindo o total de itens pelo o tamanho da pagina
        
        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var count = query.Count(); // Conta o total de registros da consulta
        var item = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        
        return new PagedList<T>(item, count, pageNumber, pageSize);
    }
}