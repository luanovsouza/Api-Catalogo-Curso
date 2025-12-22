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
    private readonly IUnitOfWork _uof;

    public ProdutosController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    
    //Buscar todos os Produtos
    [HttpGet]
    public ActionResult<IEnumerable<Produto?>> BuscarProdutos()
    {
        var produtos = _uof.ProdutoRepository.GetAll(); //Vai retornar uma lista de produtos

        return Ok(produtos);
    }
    
    [HttpGet("produtosCategoria/{id:int}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetById(pr => pr.CategoriaId == id);

        if (produtos is null)
            return NotFound();
        
        return Ok(produtos);
    }

    //Buscar por ID
    [HttpGet("{id:int}", Name = "AcharProduct")]
    public ActionResult<Produto?> FindProduct(int id)
    {
        var produto = _uof.ProdutoRepository.GetById(pr => pr.CategoriaId == id);
        
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


        _uof.ProdutoRepository.Create(product);
        _uof.Commit();

        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("AcharProduct",
            new { id = product.Id }, product); 
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.Id)
            return BadRequest($"O id = {id} não existe!");

        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produtoDeletado = _uof.ProdutoRepository.GetById(pd => pd.Id == id);

        if (produtoDeletado == null)
            return NotFound($"Produto do id={id} não foi localizado...");


        var categoriaExcluida = _uof.ProdutoRepository.Delete(produtoDeletado);
        _uof.Commit();

        return Ok(categoriaExcluida);
    }
}