using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Model;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>(); // Uma boa prática inicializar no construtor a coleção
    }
    
    public int CategoriaId { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? ImagemUrl { get; set; }
    
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}