using Lab10_MunozHerrera.Domain.Entities;
using Lab10_MunozHerrera.Domain.Interfaces;
using Lab10_MunozHerrera.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore; // Necesario para FirstOrDefaultAsync

namespace Lab10_MunozHerrera.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    // Implementación del método específico
    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles) // Opcional: Cargar relaciones
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}

// --- Repite el patrón para las otras entidades ---
// (Puedes dejarlos vacíos por ahora si no tienen métodos especiales)

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context) { }
}

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context) { }
}

public class ResponseRepository : GenericRepository<Response>, IResponseRepository
{
    public ResponseRepository(AppDbContext context) : base(context) { }
}