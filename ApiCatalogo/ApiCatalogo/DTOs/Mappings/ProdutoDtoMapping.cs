using ApiCatalogo.Model;
using AutoMapper;

namespace ApiCatalogo.DTOs.Mappings;

public class ProdutoDtoMapping : Profile
{
    public ProdutoDtoMapping()
    {
        CreateMap<Produto, ProdutoDto>().ReverseMap();// O reverseMap serve para poder reverter de Produto para Dto
        //e o Dto para Produto
        CreateMap<Categoria, CategoriaDto>().ReverseMap();;
        CreateMap<Produto, ProdutoDtoUpdateResponse>().ReverseMap();
        CreateMap<Produto, ProdutoDtoUpdateRequest>().ReverseMap();
    }
}