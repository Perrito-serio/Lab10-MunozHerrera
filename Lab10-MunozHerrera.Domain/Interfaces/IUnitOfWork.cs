namespace Lab10_MunozHerrera.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Propiedades para cada repositorio
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    ITicketRepository Tickets { get; }
    IResponseRepository Responses { get; }

    // Método para guardar cambios
    Task<int> SaveAsync();
}