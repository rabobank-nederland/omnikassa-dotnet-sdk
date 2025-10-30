using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace example_dotnet60
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((_, serverOptions) =>
                    {
                        serverOptions.Listen(IPAddress.Parse("0.0.0.0"), 5000);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
