using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiCatalogo.DTOs;

public class ProdutoDtoUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "O estoque deve conter de 1 até 9999")]
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataCadastro <= DateTime.Now)
        {
            yield return new ValidationResult("A data de cadastro deve ser menor ou igual a data atual", 
                new[] {nameof(this.DataCadastro)});
        }
    }
}