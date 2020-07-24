using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // metodo principale quando avvio l'applicazione, tutto viene caricato partendo da qui
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    // applicherà qualsiasi migrazione in sospeso per il context del database (Crea il db se non esistente)
                    context.Database.Migrate();
                    // verrà verificato se il database non ha utenti (vedi Seeds.cs)
                    Seed.SeedUsers(context);
                }
                catch (Exception ex)
                {
                    // in caso di errore, l'errore verrà loggato e un messaggio di errore comparirà
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //configura il logging in questo metodo
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
