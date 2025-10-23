using Lab10_MunozHerrera.Domain.Interfaces;
using Lab10_MunozHerrera.Infrastructure.Persistence.Data;

namespace Lab10_MunozHerrera.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    // Instancias de los repositorios
    public IUserRepository Users { get; private set; }
    public IRoleRepository Roles { get; private set; }
    public ITicketRepository Tickets { get; private set; }
    public IResponseRepository Responses { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        // Inicializar los repositorios
        Users = new UserRepository(_context);
        Roles = new RoleRepository(_context);
        Tickets = new TicketRepository(_context);
        Responses = new ResponseRepository(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}