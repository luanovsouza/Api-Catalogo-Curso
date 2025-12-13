using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repository;

    public CategoriasController(ICategoriaRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoriasAsync()
    {
        var categorias = await _repository.GetAllAsync();

        return Ok(categorias);
    }

    [HttpGet("CategoriaProduto/{id:int:min(1)}")]
    public ActionResult<Categoria> BuscarCategoriaProduto(int id)
    {
        var categoriaProduto = _repository.GetByIdAsync(id);

        if (categoriaProduto == null)
            return NotFound($"Categoria do id {id} não foi encontrado ou não existe...");

        return Ok(categoriaProduto);
    }


    //Buscar por ID
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Categoria>> BuscarCategoria(int id)
    {
        var categoria = await _repository.GetByIdAsync(id);

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


        await _repository.CreateAsync(categoria);

        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("ObterProduto",
            new { id = categoria.CategoriaId }, categoria); // Vai retornar 201
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest("Dados invalidos!");

        await _repository.UpdateAsync(categoria);
        

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var categoriaDeletada = _repository.GetByIdAsync(id);

        if (categoriaDeletada == null)
            return NotFound($"Categoria do id={id} não encontrada...");


        await _repository.DeleteAsync(id);
        
        return Ok(categoriaDeletada);
    }
}