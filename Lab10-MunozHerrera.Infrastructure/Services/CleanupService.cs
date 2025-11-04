using Lab10_MunozHerrera.Application.Interfaces;
using Lab10_MunozHerrera.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Lab10_MunozHerrera.Infrastructure.Services;

public class CleanupService : ICleanupService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CleanupService> _logger;

    // Inyectamos el UnitOfWork para acceder a los repositorios
    public CleanupService(IUnitOfWork unitOfWork, ILogger<CleanupService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task CleanOldTicketsAsync()
    {
        _logger.LogInformation("Iniciando job de limpieza de tickets viejos...");

        // 1. Definir la fecha de corte (hace 30 días)
        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        // 2. Obtener todos los tickets (para un lab está bien, en producción filtraríamos en la BD)
        var allTickets = await _unitOfWork.Tickets.GetAllAsync();

        // 3. Encontrar los tickets que cumplan la condición
        var oldTickets = allTickets
            .Where(t => t.Status == "Closed" && 
                        t.ClosedAt.HasValue && 
                        t.ClosedAt.Value < cutoffDate)
            .ToList();

        if (!oldTickets.Any())
        {
            _logger.LogInformation("No se encontraron tickets viejos para limpiar.");
            return;
        }

        // 4. Eliminar los tickets encontrados
        foreach (var ticket in oldTickets)
        {
            _unitOfWork.Tickets.Remove(ticket);
        }

        // 5. Guardar los cambios en la BD
        var count = await _unitOfWork.SaveAsync();

        _logger.LogInformation($"Limpieza completada. Se eliminaron {count} tickets.");
    }
}