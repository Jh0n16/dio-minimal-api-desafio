using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;

namespace Teste.Mocks
{
    public class AdiministradorServicoMock : IAdministradorServico
    {
        private static List<Administrador> administradores = new List<Administrador>() {
            new Administrador {Id = 1, Email = "admin@teste.com", Senha = "teste", Perfil = "Admin"},
            new Administrador {Id = 2, Email = "editor@teste.com", Senha = "teste", Perfil = "Editor"}
        };

        public void Adicionar(Administrador administrador)
        {
            administrador.Id = administradores.Count() + 1;
            administradores.Add(administrador);
        }

        public void Apagar(Administrador administrador)
        {
            administradores.Remove(administrador);
        }

        public void Atualizar(Administrador administrador)
        {
            administradores[administrador.Id - 1] = administrador;
        }

        public Administrador BuscarPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public List<Administrador> ListarTodos(int pagina = 1)
        {
            return administradores;
        }

        public Administrador Login(LoginDto login)
        {
            return administradores.Find(a => a.Email == login.Email && a.Senha == login.Senha);
        }
    }
}