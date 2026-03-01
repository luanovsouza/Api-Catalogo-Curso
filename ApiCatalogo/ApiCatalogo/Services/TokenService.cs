using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiCatalogo.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace ApiCatalogo.Services;

public class TokenService : ITokenService 
{
    public string GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config)
    {
        // //Pegando a chave dentro o AppSettings.json, onde fica a chave secreta
        // var chave = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
        //             throw new InvalidOperationException("Chave secreta invalida!");

        // //Codificando a chave la do Jwt para padrao bytes, ja q ela é string
        // var chavePrivada = Encoding.ASCII.GetBytes(chave);

        // var credenciaisAssinatura = new SigningCredentials(new SymmetricSecurityKey(chavePrivada),
        //     SecurityAlgorithms.HmacSha256Signature); //(SecurityAlgorithms.HmacSha256Signature)
        // //Isso é utilizado para assinar o token

        // //-------------------------Fazendo a construçao do token-------------------------

        // var descricaoToken = new SecurityTokenDescriptor
        // {
        //     //Obtendo as informaçoes do usuario
        //     Subject = new ClaimsIdentity(claims),

        //     //Definindo a data de expiraçao do token
        //     Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),

        //     //Obtendo a audiencia
        //     Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),

        //     //Obtendo o emissor
        //     Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),

        //     SigningCredentials = credenciaisAssinatura
        // };

        // //Responsavel por criar e validar os tokens
        // var tokenHandler = new JwtSecurityTokenHandler();

        // //Criação do token
        // var token = tokenHandler.CreateJwtSecurityToken(descricaoToken);

        // return token;

        //--------------Uma forma mais simples e direta do que essa forma--------------

        var chaveSecreta = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                              throw new InvalidOperationException("Chave secreta invalida!");
        
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta)); 
        
        var credentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWT:ValidIssuer"],
            audience: _config["JWT:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
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