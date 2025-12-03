using ApiCatalogo.Context;
using ApiCatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
    {
        return await _context.Categorias.Include(p => p.Produtos)
            .Where(c => c.CategoriaId <= 5)
            .AsNoTracking().ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoriasAsync()
    {
        return await _context.Categorias.AsNoTracking().ToListAsync();
    }

    [HttpGet("CategoriaProduto/{id:int:min(1)}")]
    public ActionResult<Categoria> BuscarCategoriaProduto(int id)
    {
        var categoriaProduto = _context.Categorias.AsNoTracking().Where(cp =>
                cp.CategoriaId <= 10)
            .Include(p => p.Produtos)
            .FirstOrDefault(cp => cp.CategoriaId == id);

        if (categoriaProduto == null)
            return NotFound($"Categoria do id {id} não foi encontrado ou não existe...");

        return Ok(categoriaProduto);
    }


    //Buscar por ID
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Categoria>> BuscarCategoria(int id)
    {
        var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync
            (c => c.CategoriaId == id);

        if (categoria == null)
            return NotFound($"Categoria do id {id} não encontrado...");

        return Ok(categoria);
    }

    //Criar uma categoria
    [HttpPost]
    public async Task<IActionResult> CriarProduto([FromBody] Categoria? categoria)
    {
        if (categoria == null)
            return BadRequest("Dados inválidos digite novamente!");


        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("ObterProduto",
            new { id = categoria.CategoriaId }, categoria); // Vai retornar 201
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest("Dados invalidos!");

        _context.Entry(categoria).State = EntityState.Modified; //Estou dizendo q o produto esta modificado
        await _context.SaveChangesAsync();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var categoriaDeletada = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

        if (categoriaDeletada == null)
            return NotFound($"Categoria do id={id} não encontrada...");


        _context.Categorias.Remove(categoriaDeletada);
        await _context.SaveChangesAsync();

        return Ok(categoriaDeletada);
    }
}