using Lab10_MunozHerrera.Domain.Entities; 

namespace Lab10_MunozHerrera.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    // Aquí puedes añadir métodos específicos de User, ej:
    Task<User> GetByUsernameAsync(string username);
}

// --- Repite el patrón para las otras entidades ---

public interface IRoleRepository : IGenericRepository<Role> { }
public interface ITicketRepository : IGenericRepository<Ticket> { }
public interface IResponseRepository : IGenericRepository<Response> { }