using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiCatalogo.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using ApiCatalogo.Model;

namespace ApiCatalogo.Services;

public class TokenService : ITokenService 
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAcessToken(ApplicationUser user)
    {
        //--------------Uma forma mais simples e direta do que essa forma--------------

        try
        {
            var chaveSecreta = _configuration.GetSection("JWT").GetValue<string>("SecretKey") ??
                               throw new InvalidOperationException("Chave secreta invalida!");
        
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta)); 
        
            var credentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
        
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string GenerateRefreshToken()
    {
        //Um array de bytes seguros
        var bytesAleatoriosSeguros = new byte[128];

        using var geradorDeNumerosAleatorios = RandomNumberGenerator.Create();

        //Preenchendo minha variavel de bytes aleatorios
        geradorDeNumerosAleatorios.GetBytes(bytesAleatoriosSeguros);

        //Convertendo para base 64
        var refreshToken = Convert.ToBase64String(bytesAleatoriosSeguros);

        return refreshToken;
    }

    //Utilizado para obter as informações das claims do token para criar o token novamente
    //Ou seja criando um novo token a partir do referesh token
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration config)
    {
        var chaveSecreta = config.GetSection("JWT").GetValue<string>("SecretKey") ??
                           throw new InvalidOperationException("Chave secreta invalida!");

        var parametrosValidacaoToken = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta)),

            ValidateLifetime = false
        };

        //Utilizado para manipular o token
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Fazendo a validaçao do token usando o token handler
        var principal = tokenHandler.ValidateToken(token,
            parametrosValidacaoToken, out SecurityToken securityToken);
        
        //Verificando se o token nao é um JwtSecurityToken, ou o algoritimo usado nao for HmacSha256
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Token invalido!");
        }

        return principal;
    }
}