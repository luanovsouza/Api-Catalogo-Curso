using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection"); // string de conexao
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, 
    ServerVersion.AutoDetect(mySqlConnection)));

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