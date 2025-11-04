using Lab10_MunozHerrera.Application.Interfaces;

namespace Lab10_MunozHerrera.Infrastructure.Services;

public class NotificationService : INotificationService
{
    public Task SendNotificationAsync(string user)
    {
        Console.WriteLine($"Notificación enviada a {user} en {DateTime.Now}");

        Console.WriteLine($"Notificación enviada EXITOSAMENTE a {user} en {DateTime.Now}");
        
        return Task.CompletedTask;
    }
}