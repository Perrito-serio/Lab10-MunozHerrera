// --- USINGS CORRECTOS ---
using Lab10_MunozHerrera.Application.Interfaces;
using Lab10_MunozHerrera.Domain.Interfaces;
using Lab10_MunozHerrera.Infrastructure.Persistence.Data;
using Lab10_MunozHerrera.Infrastructure.Repositories;
using Lab10_MunozHerrera.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab10_MunozHerrera.Infrastructure.Extensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configurar DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}