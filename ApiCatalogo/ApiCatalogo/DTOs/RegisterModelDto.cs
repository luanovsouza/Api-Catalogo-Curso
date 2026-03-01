using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class RegisterModelDto
{
    [Required(ErrorMessage = "O campo de Login é necessario")]
    public string? UserName { get; set; }
    
    [Required(ErrorMessage = "A senha é necessaria")]
    public string? PassWord { get; set; }
    
    [EmailAddress]
    [Required(ErrorMessage = "O E-mail é necessario")]
    public string? Email { get; set; }
}