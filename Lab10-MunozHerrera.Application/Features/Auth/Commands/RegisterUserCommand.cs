using Lab10_MunozHerrera.Domain.Entities;
using Lab10_MunozHerrera.Domain.Interfaces;
using MediatR;
using BCrypt.Net;

namespace Lab10_MunozHerrera.Application.Features.Auth.Commands;

// El Comando (similar a un DTO de Request)
// Hereda de IRequest<string> porque esperamos un string como respuesta
public record RegisterUserCommand(
    string Username,
    string? Email,
    string Password
) : IRequest<string>;

// El Handler (El caso de uso que procesa el comando)
// Hereda de IRequestHandler<Comando, Respuesta>
internal sealed class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // El método Handle contiene la lógica de negocio
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificar si el usuario ya existe
        var userExists = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (userExists != null)
        {
            throw new ApplicationException("El nombre de usuario ya existe.");
        }

        // 2. Encriptar la contraseña
        string passwordHash = global::BCrypt.Net.BCrypt.HashPassword(request.Password);      
        
        // 3. Crear el nuevo usuario
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = passwordHash,
            Email = request.Email,
        };

        // 4. Guardar en la BD usando Unit of Work
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        // 5. Devolver la respuesta
        return "Usuario registrado exitosamente.";
    }
}