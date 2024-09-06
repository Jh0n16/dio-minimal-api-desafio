using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Teste.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTeste
    {
        private DbContexto CriarContextoDeTeste()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            var config = builder.Build();

            return new DbContexto(config);

        }
        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            // Arrange (criação de variáveis)
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

            Administrador adm = new Administrador();
            adm.Id = 1;
            adm.Email = "email@teste.com";
            adm.Senha = "senha";
            adm.Perfil = "Admin";

            var administradorServico = new AdministradorServico(context);

            // Act (ação)
            administradorServico.Adicionar(adm);

            // Assert (validação)
            Assert.AreEqual(1, administradorServico.ListarTodos(1).Count());
        }
    }
}