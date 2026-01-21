using ApiCatalogo.Model;
using AutoMapper;

namespace ApiCatalogo.DTOs.Mappings;

public class CategoriaDtoMapping : Profile
{
    public CategoriaDtoMapping()
    {
        CreateMap<Categoria, CategoriaDto>();
    }
}