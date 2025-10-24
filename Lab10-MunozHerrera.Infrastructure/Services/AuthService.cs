using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab10_MunozHerrera.Application.DTOs;
using Lab10_MunozHerrera.Application.Interfaces;
using Lab10_MunozHerrera.Domain.Entities;
using Lab10_MunozHerrera.Domain.Interfaces;
using Microsoft.Extensions.Configuration; 
using Microsoft.IdentityModel.Tokens;

namespace Lab10_MunozHerrera.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        // 1. Verificar si el usuario ya existe
        var userExists = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
        if (userExists != null)
        {
            throw new ApplicationException("El nombre de usuario ya existe.");
        }

        // 2. Encriptar la contraseña (¡Práctica recomendada!)
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        // 3. Crear el nuevo usuario
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = registerDto.Username,
            PasswordHash = passwordHash,
            Email = registerDto.Email,
            // CreatedAt = DateTime.UtcNow
        };

        // 4. Guardar en la BD
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        // 5. Devolver un mensaje o token (aquí solo un mensaje)
        return "Usuario registrado exitosamente.";
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        // 1. Buscar al usuario
        var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
        if (user == null)
        {
            throw new ApplicationException("Credenciales inválidas.");
        }

        // 2. Verificar la contraseña
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new ApplicationException("Credenciales inválidas.");
        }

        // 3. Si es válido, generar el Token JWT
        var token = GenerateJwtToken(user);
        return token;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Clave secreta (de appsettings.json)
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        // Claims (Información que va dentro del token)
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Puedes añadir roles aquí si los cargas desde la BD
            // new Claim(ClaimTypes.Role, "Admin"),
        };
        
        // Cargar roles del usuario (si existen)
        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Duración del token
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}