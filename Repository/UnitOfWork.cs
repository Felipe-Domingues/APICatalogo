using APICatalogo.Context;

namespace APICatalogo.Repository;
public class UnitOfWork : IUnitOfWork
{
    private ProdutoRepository _produtoRepo;
    private CategoriaRepository _categoriaRepo;
    private AppDbContext _context;
    public IProdutoRepository ProdutoRepository
    {
        get { return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context); }
    }

    public ICategoriaRepository CategoriaRepository
    {
        get { return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context); }
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
