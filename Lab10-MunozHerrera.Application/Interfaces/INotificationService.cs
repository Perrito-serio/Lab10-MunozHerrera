namespace Lab10_MunozHerrera.Application.Interfaces;

public interface INotificationService
{
    // Usamos Task para que sea asíncrono en el futuro
    Task SendNotificationAsync(string user);
}