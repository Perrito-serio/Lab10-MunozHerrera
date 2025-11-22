
using Lab10_MunozHerrera_Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Lab10_MunozHerrera.Application.Interfaces;

using Microsoft.EntityFrameworkCore;
using Lab10_MunozHerrera.Infrastructure.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    //context.Database.EnsureCreated();
}



//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    
//}

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

// Registra el job de limpieza para que se ejecute diariamente
RecurringJob.AddOrUpdate<ICleanupService>(
    "daily-ticket-cleanup",           
    service => service.CleanOldTicketsAsync(),
    Cron.Daily);                               

app.Run();