using System.Text;
using System.Text.Json.Serialization;
using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using ApiCatalogo.Services;
using ApiCatalogo.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();


builder.Services.AddOpenApi();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//A minha ‘string’ de conexão
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");


var secrectKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid Secret Key!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//É como se fosse, "Minha aplicação usa
    //autenicação e o tipo padrao é JWT Bearer
    options.DefaultChallengeScheme =
        JwtBearerDefaults
            .AuthenticationScheme; // Isso significa que por padrao o sisttema de autenticação, vai usar token
}).AddJwtBearer(options =>
{
    options.SaveToken = true;//Significa se o token deve ser salvo apos uma autenticaçao bem sucedida
    options.RequireHttpsMetadata = false; //Indica se é preciso HTTPS para transmitir o token OBS: Em produção deve ser true
    
    //Classe que permite configurar os parametros de validaçao do token

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //Siginifica, configurações, validar a validade do Emissor da audiencia e o tempo de vida do token
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        
        //Vai validar a assinatura de chave do emissor
        ValidateIssuerSigningKey = true,
        
        //Permite ajustar o tempo entre o servidor de autenticação e aplicaçao
        ClockSkew = TimeSpan.Zero,
        
        //Os dois esta sendo atribuido o valor de audiencia e emissor
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        
        //Gerando a chave, usando a chave simetrica usando a secrectkey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey))
    };
});

    
//Configurando o MySql 
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, //Aqui estou, dizendo para usar, meu, SQL com essa conexão
    ServerVersion.AutoDetect(mySqlConnection))); //Auto-detectar a versão do SQL

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();/*
    Toda vez que chamar e que, todo mundo chamar a Interface da categoria, vai
    usar a classe CategoriaRepository
 */
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepositoryGeneric<>),  typeof(RepositoryGeneric<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddAutoMapper(typeof(ProdutoDtoMapping));//Fazendo a configuração do serviço do AutoMapper no container

var app = builder.Build();

// Configure the HTTP request pipeline.
    
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Api Catalogo"));
   // app.ConfigureExceptionsHandler();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();