using System.Text.Json.Serialization;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


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
    ServerVersion.AutoDetect(mySqlConnection))); //Auto, detectar a versão do SQL

var app = builder.Build();

// Configure the HTTP request pipeline.
    
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Api Catalogo"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();