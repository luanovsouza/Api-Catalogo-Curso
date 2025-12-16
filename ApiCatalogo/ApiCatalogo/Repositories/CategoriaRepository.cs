using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : RepositoryGeneric<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) :  base(context)
    {
    }

}