using System.Text.Json.Serialization;
using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();



builder.Services.AddSwaggerGen(c =>
{
    c.UseAllOfToExtendReferenceSchemas();
});

builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});


builder.Services.AddOpenApi();


//A minha ‘string’ de conexão
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();