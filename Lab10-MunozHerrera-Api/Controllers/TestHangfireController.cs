using Hangfire; 
using Lab10_MunozHerrera.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_MunozHerrera_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestHangfireController : ControllerBase
{
    private readonly INotificationService _notificationService;

    // Inyectamos el servicio que registramos en el paso anterior
    public TestHangfireController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    // Creamos el endpoint para probar
    [HttpPost("fire-and-forget")]
    public IActionResult FireAndForgetJob()
    {
        
        // Esta es la forma correcta de encolar un job usando Inyección de Dependencias
        BackgroundJob.Enqueue<INotificationService>(
            service => service.SendNotificationAsync("usuario1"));
            
        return Ok("Job 'fire-and-forget' encolado. Revisa tu dashboard de Hangfire.");
    }
}