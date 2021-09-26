using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolutionTemplate.DAL.Context;

namespace SolutionTemplate.BlazorUI.Hosting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await db_initializer.InitializeAsync();
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(host => host
                   .UseStartup<Startup>()
                )
        ;
    }
}
