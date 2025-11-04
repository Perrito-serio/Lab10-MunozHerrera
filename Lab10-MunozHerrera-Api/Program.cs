
using Lab10_MunozHerrera_Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Lab10_MunozHerrera.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Habilita el Dashboard de Hangfire en la ruta /hangfire
app.UseHangfireDashboard("/hangfire");


app.MapControllers();

// Hangfire se encargará de resolver INotificationService cuando sea el momento
RecurringJob.AddOrUpdate<INotificationService>(
    "job-notificacion-diaria",
    service => service.SendNotificationAsync("usuario_diario"),
    Cron.Daily);


app.Run();