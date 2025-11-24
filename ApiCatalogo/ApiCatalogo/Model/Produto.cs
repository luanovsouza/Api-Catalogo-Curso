using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Model;

public class Produto
{
    [JsonIgnore]
    public int ProdutoId { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    
    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }
    
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }
    
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    
    //Relacionamentos
    
    public int CategoriaId { get; set; } //Realizando o relacionamento entre as duas entidades (FK)

    [JsonIgnore]
    public  Categoria? Categoria { get; set; }
}