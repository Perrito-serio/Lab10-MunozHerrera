using Lab10_MunozHerrera.Domain.Entities;
using Lab10_MunozHerrera.Domain.Interfaces;
using MediatR;
using BCrypt.Net;

namespace Lab10_MunozHerrera.Application.Features.Auth.Commands;

public record RegisterUserCommand(
    string Username,
    string? Email,
    string Password
) : IRequest<string>;

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
        var userExists = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (userExists != null)
        {
            throw new ApplicationException("El nombre de usuario ya existe.");
        }

        string passwordHash = global::BCrypt.Net.BCrypt.HashPassword(request.Password);      
        
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = passwordHash,
            Email = request.Email,
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        return "Usuario registrado exitosamente.";
    }
}