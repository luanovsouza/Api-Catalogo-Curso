using ApiCatalogo.Context;
using ApiCatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    //Injeção de dependencia
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }


    //Buscar todos os Produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto?>>> BuscarProdutos()
    {
        var produtos = await _context.Produtos.Take(5).ToListAsync(); //Vai retornar uma lista de produtos

        return produtos;
    }

    //Buscar por ID
    [HttpGet("{id:int}", Name = "AcharProduct")]
    public async Task<ActionResult<Produto?>> FindProduct(int id)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p != null && p.ProdutoId == id);
        if (produto == null)
            return NotFound("Produto não encontrado...");

        return Ok(produto);
    }

    // //Criar um produto
    [HttpPost]
    public ActionResult CriarProduto(Produto? product)
    {
        if (product == null)
            return BadRequest();


        _context.Produtos.Add(product);
        _context.SaveChanges();

        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("AcharProduct",
            new { id = product.ProdutoId }, product); // Vai retornar 201
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest();

        _context.Entry(produto).State = EntityState.Modified; //Estou dizendo q o produto esta modificado
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produtoDeletado = _context.Produtos.FirstOrDefault(p => p != null && p.ProdutoId == id);

        if (produtoDeletado == null)
            return NotFound($"Produto do id={id} não foi localizado...");


        _context.Produtos.Remove(produtoDeletado);
        _context.SaveChanges();

        return Ok(produtoDeletado);
    }
}