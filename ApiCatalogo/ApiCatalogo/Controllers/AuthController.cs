using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiCatalogo.DTOs;
using ApiCatalogo.Model;
using ApiCatalogo.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    // [HttpPost]
    // public async Task<IActionResult> Login([FromBody] LoginModelDto model)
    // {
    //     //Buscando o usuario na banco do identity do usuario
    //
    //     var user = await _userManager.FindByNameAsync(model.UserName!); // essa "!" siginifca que 
    //     //eu tenho certeza de que nao é uma propiedade nula
    //
    //     //Verificando a senha do usuario, e vendo se o usuario nao é nulo 
    //     if (user is not null && await _userManager.CheckPasswordAsync(user, model.PassWord!))
    //     {
    //         var userRoles = await _userManager.GetRolesAsync(user);
    //         
    //         //Lista de Claims usadas para criar o token de autenticação
    //         var authClaims = new List<Claim>
    //         {
    //             //Claim (Informaçao do usuario) do nome dele
    //             new Claim(ClaimTypes.Name, user.UserName!),
    //             new Claim(ClaimTypes.Email, user.Email!),
    //             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //         };
    //
    //         foreach (var userRole in userRoles)
    //         {
    //             authClaims.Add(new Claim(ClaimTypes.Role, userRole));
    //         }
    //     }
    // }
}