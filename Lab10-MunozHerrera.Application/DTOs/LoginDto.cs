using System.ComponentModel.DataAnnotations;

namespace Lab10_MunozHerrera.Application.DTOs;

public class LoginDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}