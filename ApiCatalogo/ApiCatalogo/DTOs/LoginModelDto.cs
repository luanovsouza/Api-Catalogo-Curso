using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class LoginModelDto
{
    [Required(ErrorMessage = "O campo de Login é necessario")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "A senha é necessaria")]
    public string? PassWord { get; set; }
}