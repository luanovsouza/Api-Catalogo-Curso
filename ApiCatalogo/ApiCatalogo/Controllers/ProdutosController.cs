using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    //Injeção de dependencia
    private readonly IProdutoRepository _repository;

    public ProdutosController(IProdutoRepository repository)
    {
        _repository = repository;
    }


    //Buscar todos os Produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto?>>> BuscarProdutos()
    {
        var produtos = await _repository.GetAllAsync(); //Vai retornar uma lista de produtos

        return Ok(produtos);
    }

    //Buscar por ID
    [HttpGet("{id:int}", Name = "AcharProduct")]
    public async Task<ActionResult<Produto?>> FindProduct(int id)
    {
        var produto = await _repository.GetByIdAsync(id);
        
        if (produto == null)
            return NotFound("Produto não encontrado...");

        return Ok(produto);
    }

    // //Criar um produto
    [HttpPost]
    public ActionResult CriarProduto(Produto? product)
    {
        if (product == null)
            return BadRequest("O produto esta vazio, digite novamente.");


        _repository.CreateAsync(product);

        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("AcharProduct",
            new { id = product.Id }, product); 
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.Id)
            return BadRequest($"O id = {id} não existe!");

        _repository.UpdateAsync(produto);

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produtoDeletado = _repository.GetByIdAsync(id);

        if (produtoDeletado == null)
            return NotFound($"Produto do id={id} não foi localizado...");


        var categoriaExcluida = _repository.DeleteAsync(id);

        return Ok(categoriaExcluida);
    }
}