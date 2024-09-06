using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.ModelViews;
using Teste.Helpers;

namespace Teste.Resquest
{
    [TestClass]
    public class AdministradorRequestTeste
    {
        [TestInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task LoginTeste()
        {
            LoginDto login = new LoginDto {
                Email = "admin@teste.com",
                Senha = "teste"
            };

            var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "Application/json");

            var response = await Setup.client.PostAsync("/admin/login", content);

            Assert.Equals(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(admLogado.Email);
            Assert.IsNotNull(admLogado.Perfil);
            Assert.IsNotNull(admLogado.Token);
        }
    }
}