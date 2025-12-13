using ApiCatalogo.Context;
using ApiCatalogo.Model;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
       var categorias = _context.Categorias.ToListAsync();
       
       if (categorias == null)
           throw new ArgumentNullException("Não pode ser nulo");
       
       return await categorias;
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        var categoriaId = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);
        
        return categoriaId;
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        if (categoria == null)
            throw new ArgumentNullException(nameof(categoria));
        
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        
        return categoria;
    }

    public async Task<Categoria> UpdateAsync(Categoria categoria)
    {
        if (categoria == null)
            throw new ArgumentNullException(nameof(categoria));
        
        _context.Entry(categoria).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return categoria;
    }

    public async Task<Categoria> DeleteAsync(int id)
    {
        var categoriaDeletada =  _context.Categorias.Find(id);
        
        if (categoriaDeletada == null)
            throw new ArgumentNullException(nameof(categoriaDeletada));
        
        _context.Categorias.Remove(categoriaDeletada);
        await _context.SaveChangesAsync();
        
        return categoriaDeletada;
    }
}