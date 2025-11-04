using Lab10_MunozHerrera.Application.Interfaces;
using Lab10_MunozHerrera.Domain.Interfaces;
using Lab10_MunozHerrera.Infrastructure.Persistence.Data;
using Lab10_MunozHerrera.Infrastructure.Repositories;
using Lab10_MunozHerrera.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.PostgreSql;

namespace Lab10_MunozHerrera.Infrastructure.Extensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IAuthService, AuthService>();

        
        // 1. Configura Hangfire
        services.AddHangfire(config =>
            // Usa la nueva sintaxis con "options" para evitar la advertencia
            config.UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
            })
        );

        // 2. Añade el servidor de Hangfire
        services.AddHangfireServer();
        
        return services;
    }
}