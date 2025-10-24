using System.ComponentModel.DataAnnotations;

namespace Lab10_MunozHerrera.Application.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string Password { get; set; } = null!;
}