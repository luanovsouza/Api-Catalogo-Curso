using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> BuscarCategorias()
    {
        var categorias = _uof.CategoriaRepository.GetAll();

        var categoriasDto = categorias.ToCategoriasDtoList();

        return Ok(categorias);
    }


    [HttpGet("/api/CategoryPagination")]
    public ActionResult<IEnumerable<CategoriaDto>> Get([FromQuery] CategoriaParameters categoriaParameters)
    {
        var categorias = _uof.CategoriaRepository.GetCategorias(categoriaParameters);

        var metaData = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasPrevious,
            categorias.HasNext
        };

        if (categorias.CurrentPage > categorias.TotalPages)
        {
            return BadRequest("A lista esta vazia, não tem nada aqui!");
        }
        
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));
        
        var categoriaDto = _mapper.Map<IEnumerable<CategoriaDto>>(categorias);

        return Ok(categoriaDto);
    }

    [HttpGet("CategoriaProduto/{id:int:min(1)}")]
    public ActionResult<Categoria> BuscarCategoriaProduto(int id)
    {
        var categoriaProduto = _uof.CategoriaRepository.GetById(cp => cp.CategoriaId == id);

        if (categoriaProduto == null)
            return NotFound($"Categoria do id {id} não foi encontrado ou não existe...");

        var categoriaDto = categoriaProduto.ToCategoriaDto();

        return Ok(categoriaDto);
    }


    //Buscar por ID
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<Categoria> BuscarCategoria(int id)
    {
        var categoria =  _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

        if (categoria == null)
            return NotFound($"Categoria do id {id} não encontrado...");

        var categoriaDto = categoria.ToCategoriaDto();

        return Ok(categoriaDto);
    }

    //Criar uma categoria
    [HttpPost]
    public IActionResult CriarProduto([FromBody] CategoriaDto? categoriaDto)
    {
        if (categoriaDto == null)
            return BadRequest("Dados inválidos digite novamente!");

        
        var categoriaCriada = categoriaDto.ToCategoria();
        
        _uof.CategoriaRepository.Create(categoriaCriada);
        _uof.Commit();
        
        var novaCategoriaDto = categoriaCriada.ToCategoriaDto();
        
        //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
        return new CreatedAtRouteResult("ObterProduto",
            new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto); // Vai retornar 201
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, CategoriaDto categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest("Dados invalidos!");
        
        var categoria = categoriaDto.ToCategoria();
        _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        var categoriaAtualizadaDto = categoria.ToCategoriaDto();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoriaDeletada = _uof.CategoriaRepository.GetById(cp => cp.CategoriaId == id);

        if (categoriaDeletada == null)
            return NotFound($"Categoria do id={id} não encontrada...");

        
        _uof.CategoriaRepository.Delete(categoriaDeletada);
        _uof.Commit();
        
        var categoriaDeletadaDto = categoriaDeletada.ToCategoriaDto();
        
        return Ok(categoriaDeletadaDto);
    }
}