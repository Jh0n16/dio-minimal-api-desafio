using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Enums;

namespace Teste.Domain.Entidades
{
    [TestClass]
    public class AdministradorTeste
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arrange (criação de variáveis)
            Administrador adm = new Administrador();

            // Act (ação)
            adm.Id = 1;
            adm.Email = "email@teste.com";
            adm.Senha = "senha";
            adm.Perfil = "Admin";

            // Assert (validação)
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("email@teste.com", adm.Email);
            Assert.AreEqual("senha", adm.Senha);
            Assert.AreEqual("Admin", adm.Perfil);

        }

    }
}