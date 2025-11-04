namespace Lab10_MunozHerrera.Application.Interfaces;

public interface ICleanupService
{
    // Una tarea que buscará y eliminará tickets viejos
    Task CleanOldTicketsAsync();
}