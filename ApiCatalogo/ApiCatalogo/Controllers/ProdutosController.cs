using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    //Injeção de dependencia
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }


    [HttpGet("/api/Pagination")]
    public ActionResult<IEnumerable<ProdutoDto>> Get([FromQuery] ProdutoParameters produtoParameters)
    {
        var produtos = _uof.ProdutoRepository.GetProdutos(produtoParameters);

        var metaData = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasPrevious,
            produtos.HasNext
        };// Passando no header as informações da pagina
        
        if (produtos.CurrentPage > produtos.TotalPages)
        {
            return BadRequest("A lista esta vazia, não tem nada aqui!");
        }
        
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData)); //Adicionando no response
        
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        
        return Ok(produtosDto);
    }
    
    //Buscar todos os Produtos
    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDto>> BuscarProdutos()
    {
        var produtos = _uof.ProdutoRepository.GetAll(); //Vai retornar uma lista de produtos
        
        //var destino = _mapper.Map<Destino>(origem);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        
        return Ok(produtosDto);
    }
    
    [HttpGet("produtosCategoria/{id:int}")]
    public ActionResult<IEnumerable<ProdutoDto>> GetProdutosCategoria(int id)
    {
        var produto = _uof.ProdutoRepository.GetById(pr => pr.CategoriaId == id);

        if (produto is null)
            return NotFound();
        
        var produtoDto = _mapper.Map<IEnumerable<ProdutoDto>>(produto);
        
        return Ok(produtoDto);
    }

    //Buscar por ID
    [HttpGet("{id:int}", Name = "AcharProduct")]
    public ActionResult<ProdutoDto> FindProduct(int id)
    {
        var produto = _uof.ProdutoRepository.GetById(pr => pr.CategoriaId == id);
        
        if (produto == null)
            return NotFound("Produto não encontrado...");
        
        var produtoDto = _mapper.Map<ProdutoDto>(produto);
        
        return Ok(produtoDto);
    }
    
    // //Criar um produto
    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> CriarProduto(ProdutoDto? productDto)
    {
        if (productDto == null)
            return BadRequest("O produto esta vazio, digite novamente.");

        var product = _mapper.Map<Produto>(productDto);
        
        _uof.ProdutoRepository.Create(product); 
        await _uof.Commit();
        
        var newproductDto = _mapper.Map<ProdutoDto>(product);
        
        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("AcharProduct",
            new { id = newproductDto.Id }, newproductDto); 
    }


    [HttpPatch("{id:int}/UpdatePartial")]
    public async Task<ActionResult<ProdutoDtoUpdateResponse>> Patch(int id,
        JsonPatchDocument<ProdutoDtoUpdateRequest>? patchDoc)
    {
        if (patchDoc == null || id <= 0)
            return BadRequest();

        var produto = _uof.ProdutoRepository.GetById(pr => pr.Id == id);
        
        if (produto == null)
            return NotFound("Produto nao encontrado...");

        var produtoDtoReq = _mapper.Map<ProdutoDtoUpdateRequest>(produto);

        patchDoc.ApplyTo(produtoDtoReq, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _mapper.Map(produtoDtoReq, produto);
    
        _uof.ProdutoRepository.Update(produto);
        await _uof.Commit();
        

        //var response = _mapper.Map<ProdutoDtoUpdateResponse>(produto);
        return NoContent();
    }
    
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> Put(int id, ProdutoDto? produtoDto)
    {
        if (id != produtoDto.Id)
            return BadRequest($"O id = {id} não existe!");

        var produto = _mapper.Map<Produto>(produtoDto);
        
        _uof.ProdutoRepository.Update(produto);
        await _uof.Commit();
        
        var produtoDtoAtualizado = _mapper.Map<ProdutoDto>(produto);

        return Ok(produtoDtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> Delete(int id)
    {
        var produtoDeletado = _uof.ProdutoRepository.GetById(pd => pd.Id == id);

        if (produtoDeletado == null)
            return NotFound($"Produto do id={id} não foi localizado...");


        var categoriaExcluida = _uof.ProdutoRepository.Delete(produtoDeletado);
        await _uof.Commit();
        
        var categoriaDeletadaDto = _mapper.Map<ProdutoDto>(categoriaExcluida);

        return Ok(categoriaDeletadaDto);
    }
}