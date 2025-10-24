namespace Lab10_MunozHerrera.Application.Interfaces;
using Lab10_MunozHerrera.Application.DTOs;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto registerDto);
    Task<string> LoginAsync(LoginDto loginDto);
}