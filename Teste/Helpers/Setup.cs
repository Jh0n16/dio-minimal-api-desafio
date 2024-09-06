using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi;
using MinimalApi.Dominio.Interfaces;
using Teste.Mocks;

namespace Teste.Helpers
{
    public class Setup
    {
        public const string PORT = "5001";
        public static TestContext testContext;
        public static WebApplicationFactory<Startup> http;
        public static HttpClient client;

        public static void ClassInit(TestContext testContext)
        {
            Setup.testContext = testContext;
            
            Setup.http = new WebApplicationFactory<Startup>();
            Setup.http = Setup.http.WithWebHostBuilder(builder => {
                builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");
                builder.ConfigureServices(services => {
                    services.AddScoped<IAdministradorServico, AdiministradorServicoMock>();
    
                });
            });

            Setup.client = Setup.http.CreateClient();
        }

        public static void ClassCleanup()
        {
            Setup.http.Dispose();
        }
    }
}