using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.Entidades;

namespace Teste.Domain.Entidades
{
    [TestClass]
    public class VeiculoTeste
    {
         [TestMethod]
        public void TestarGetSetPropriedades()
        {
            Veiculo veiculo = new Veiculo();

            veiculo.Id = 1;
            veiculo.Marca = "Fiat";
            veiculo.Nome = "Uno";
            veiculo.Ano = 1951;

            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("Fiat", veiculo.Marca);
            Assert.AreEqual("Uno", veiculo.Nome);
            Assert.AreEqual(1951, veiculo.Ano);
        }
    }
}