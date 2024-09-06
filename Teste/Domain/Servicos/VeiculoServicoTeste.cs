using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Teste.Domain.Servicos
{
    [TestClass]
    public class VeiculoServicoTeste
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
        public void TestandoSalvarVeiculo()
        {
            // Arrange (criação de variáveis)
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos");

            Veiculo veiculo = new Veiculo {
                Id = 1,
                Ano = 1990,
                Marca = "Fiat",
                Nome = "Uno"
            };

            var veiculoServico = new VeiculoServico(context);

            // Act (ação)
            veiculoServico.Adicionar(veiculo);

            // Assert (validação)
            Assert.AreEqual(1, veiculoServico.Todos(1).Count());
        }
    }
}