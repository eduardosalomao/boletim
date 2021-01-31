using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApi.SchoolUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseSetting(WebHostDefaults.ApplicationKey, "ApiBoletim")
               .UseStartup<Startup>()
               .UseUrls("https://localhost:6004", "http://localhost:6003", "https://meuboletim.com.br", "https://homolog.meuboletim.com.br", "https://alternativa.meuboletim.com.br")
               .Build();
    }
}
