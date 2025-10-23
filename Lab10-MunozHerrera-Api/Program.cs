
using Lab10_MunozHerrera_Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// --- 1. Llamada a la configuración centralizada (Paso 2 del Lab 10) ---
// Esta ÚNICA línea registrará TODO:
// DbContext, Repositorios, UnitOfWork, Servicios, JWT y Swagger.
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// --- 2. Configurar el Pipeline (Middleware) ---
// (Esto es lo que el PDF llama "lo que va después de la línea 15")
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ¡MUY IMPORTANTE para JWT!
// Debe estar en este orden, antes de MapControllers.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();