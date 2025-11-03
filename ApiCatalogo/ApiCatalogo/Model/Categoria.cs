using System.Collections.ObjectModel;

namespace ApiCatalogo.Model;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>(); // Uma boa prática inicializar no construtor a coleção
    }
    
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? ImagemUrl { get; set; }
    public ICollection<Produto>? Produtos { get; set; }
}