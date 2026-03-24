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

    
    
    
    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Message = $"Role '{roleName}' criada com sucesso!"
                });
            }
            else
            {
                return BadRequest($"Ocorreu um erro ao criar a role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        return BadRequest(new
        {
            Message = $"Role '{roleName}' já existe!"
        });

    }

    [HttpPost("CreateUserToRole")]
    public async Task<IActionResult> CreateUserToRole( string userName, string roleName)
    {
        try
        {
            //Buscando o usuario no banco do identity do usuario
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("Usuário não encontrado!");
            }
        
            //Adicionando o usuario a role
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Mensagem = $"O usuário '{userName}' foi adicionado à role '{roleName}' com sucesso!"
                });
            }

            return BadRequest(new
            {
                erro =
                    $"Não foi possível adicionar o usuário '{userName}' à role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

[HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email
        };
        
        var newUser = await _userManager.CreateAsync(user, model.PassWord!);

        if (!newUser.Succeeded)
        {
            return BadRequest(newUser.Errors);
        }
        
        return Ok(new
        {
            Message = "Usuário registrado com sucesso!"
        });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModelDto model)
    {
        //Buscando o usuario na banco do identity do usuario
    
        var user = await _userManager.FindByNameAsync(model.UserName!); // essa "!" siginifca que 
        //eu tenho certeza de que nao é uma propiedade nula
    
        //Verificando a senha do usuario, e vendo se o usuario nao é nulo 
        if (user is not null && await _userManager.CheckPasswordAsync(user, model.PassWord!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            
            //Lista de Claims usadas para criar o token de autenticação
            var authClaims = new List<Claim>
            {
                //Claim (Informaçao do usuario) do nome dele
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Colocando a role nas claims
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
            var tokenExpiration = _configuration["JWT:TokenValidityInMinutes"];
            
            var tokenUser = _tokenService.GenerateAcessToken(user);
            
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _userManager.UpdateAsync(user);
            
            return Ok(new
            {
                token = tokenUser,
                expiration = tokenExpiration,
                refreshToken
            });
        }
        return BadRequest("Usuario ou senha incorretos!");
    }
}