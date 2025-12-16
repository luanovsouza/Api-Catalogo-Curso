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
    private readonly IRepositoryGeneric<Categoria> _repository;

    public CategoriasController(IRepositoryGeneric<Categoria> repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> BuscarCategorias()
    {
        var categorias = _repository.GetAll();

        return Ok(categorias);
    }

    [HttpGet("CategoriaProduto/{id:int:min(1)}")]
    public ActionResult<Categoria> BuscarCategoriaProduto(int id)
    {
        var categoriaProduto = _repository.GetById(cp => cp.CategoriaId == id);

        if (categoriaProduto == null)
            return NotFound($"Categoria do id {id} não foi encontrado ou não existe...");

        return Ok(categoriaProduto);
    }


    //Buscar por ID
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Categoria> BuscarCategoria(int id)
    {
        var categoria =  _repository.GetById(c => c.CategoriaId == id);

        if (categoria == null)
            return NotFound($"Categoria do id {id} não encontrado...");

        return Ok(categoria);
    }

    //Criar uma categoria
    [HttpPost]
    public IActionResult CriarProduto([FromBody] Categoria? categoria)
    {
        if (categoria == null)
            return BadRequest("Dados inválidos digite novamente!");


        _repository.Create(categoria);

        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("ObterProduto",
            new { id = categoria.CategoriaId }, categoria); // Vai retornar 201
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest("Dados invalidos!");

        _repository.Update(categoria);
        

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoriaDeletada = _repository.GetById(cp => cp.CategoriaId == id);

        if (categoriaDeletada == null)
            return NotFound($"Categoria do id={id} não encontrada...");


        _repository.Delete(categoriaDeletada);
        
        return Ok(categoriaDeletada);
    }
}