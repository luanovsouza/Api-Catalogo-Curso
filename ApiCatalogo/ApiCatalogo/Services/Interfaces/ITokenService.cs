using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiCatalogo.Services.Interfaces;

public interface ITokenService
{
    //Vai gerar o token de acesso
    // JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config);
    string GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config);
    // Claims siginifica informaçoes do usuario
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    //Esse método vai ser utilizado para extrair as informaçoes das claims do tokens
}