using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Filters;
using ApiCatalogo.Model;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers;

[ApiController]
[Authorize]
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
    public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategorias()
    {
        var categorias = await _uof.CategoriaRepository.GetAllAsync();

        var categoriasDto = categorias.ToCategoriasDtoList();

        return Ok(categorias);
    }
    
    [HttpGet("api/filter/nome/Pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategoriasNome(
        [FromQuery] CategoriaFiltroNome categoriaFiltroNome)
    {
        var categorias = await _uof.CategoriaRepository.GetCategoriaFiltroNomeAsync(categoriaFiltroNome);
        
        return  ObterCategoria(categorias);
    }

    //Método para colocar o header no response
    private ActionResult<IEnumerable<CategoriaDto>> ObterCategoria(PagedList<Categoria> categorias)
    {
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
    
    [HttpGet("/api/CategoryPagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> Get([FromQuery] CategoriaParameters categoriaParameters)
    {
        var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriaParameters);

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
    public async Task<ActionResult<Categoria>> BuscarCategoriaProduto(int id)
    {
        var categoriaProduto = await _uof.CategoriaRepository.GetByIdAsync(cp => cp.CategoriaId == id);

        if (categoriaProduto == null)
            return NotFound($"Categoria do id {id} não foi encontrado ou não existe...");

        var categoriaDto = categoriaProduto.ToCategoriaDto();

        return Ok(categoriaDto);
    }


    //Buscar por ID
    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<ActionResult<Categoria>> BuscarCategoria(int id)
    {
        var categoria =  await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

        if (categoria == null)
            return NotFound($"Categoria do id {id} não encontrado...");

        var categoriaDto = categoria.ToCategoriaDto();

        return Ok(categoriaDto);
    }

    //Criar uma categoria
    [HttpPost]
    public async Task<IActionResult> CriarProduto([FromBody] CategoriaDto? categoriaDto)
    {
        if (categoriaDto == null)
            return BadRequest("Dados inválidos digite novamente!");

        
        var categoriaCriada = categoriaDto.ToCategoria();

        if (categoriaCriada != null)
        {
            _uof.CategoriaRepository.Create(categoriaCriada);
            await _uof.CommitAsync();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDto();

            //CreatedAtRouteResult = Ira retornar o código 201 created, e precisamos passar isso
            return new CreatedAtRouteResult("ObterProduto",
                new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto); // Vai retornar 201
        }

        return BadRequest("Categoria Vazia, digite Novamente!");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, CategoriaDto categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest("Dados invalidos!");
        
        var categoria = categoriaDto.ToCategoria();
        _uof.CategoriaRepository.Update(categoria);
        await _uof.CommitAsync();

        var categoriaAtualizadaDto = categoria.ToCategoriaDto();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var categoriaDeletada = await _uof.CategoriaRepository.GetByIdAsync(cp => cp.CategoriaId == id);

        if (categoriaDeletada == null)
            return NotFound($"Categoria do id={id} não encontrada...");

        
        _uof.CategoriaRepository.Delete(categoriaDeletada);
        await _uof.CommitAsync();
        
        var categoriaDeletadaDto = categoriaDeletada.ToCategoriaDto();
        
        return Ok(categoriaDeletadaDto);
    }
}