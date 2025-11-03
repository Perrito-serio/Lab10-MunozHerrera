using Lab10_MunozHerrera.Application.DTOs;
using Lab10_MunozHerrera.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MediatR; 
using Lab10_MunozHerrera.Application.Features.Auth.Commands; 

namespace Lab10_MunozHerrera_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // 3. Añadimos IMediator
    private readonly IMediator _mediator;
    private readonly IAuthService _authService; // Se mantiene (para Login)

    // 4. Constructor actualizado para inyectar ambos servicios
    public AuthController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    // --- 5. MÉTODO REGISTER ACTUALIZADO ---
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        // Ahora recibe el 'Comando' directamente desde el body
        [FromBody] RegisterUserCommand command
    )
    {
        try
        {
            // Envía el comando a MediatR, que encontrará el Handler
            var result = await _mediator.Send(command);
            
            return Ok(new { message = result });
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error inesperado.", details = ex.Message });
        }
    }

    // --- MÉTODO LOGIN SIN CAMBIOS (POR AHORA) ---
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            // Este método sigue usando el servicio antiguo
            var token = await _authService.LoginAsync(loginDto);
            return Ok(new { token = token }); 
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error inesperado.", details = ex.Message });
        }
    }
}