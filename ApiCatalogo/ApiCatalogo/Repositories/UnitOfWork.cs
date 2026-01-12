using ApiCatalogo.Context;
using ApiCatalogo.Repositories.Interfaces;

namespace ApiCatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    // private IProdutoRepository _produtoRepo;
    // private ICategoriaRepository _categoriaRepo;
    public IProdutoRepository ProdutoRepository { get; }
    public ICategoriaRepository CategoriaRepository { get; }
    public AppDbContext _context { get; set; }
    
    public UnitOfWork(AppDbContext context,
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _context = context;
        ProdutoRepository = produtoRepository;
        CategoriaRepository = categoriaRepository;
    }

    // public IProdutoRepository ProdutoRepository
    // {
    //     get
    //     {
    //         return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
    //     }
    // }
    //
    // public ICategoriaRepository CategoriaRepository
    // {
    //     get
    //     {
    //         return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
    //     }
    // }


    

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
    
}