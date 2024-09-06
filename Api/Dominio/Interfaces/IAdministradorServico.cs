using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        public Administrador Login(LoginDto login);
        public List<Administrador> ListarTodos(int pagina = 1);
        public Administrador BuscarPorId(int id);
        public void Adicionar(Administrador administrador);
        public void Atualizar(Administrador administrador);
        public void Apagar(Administrador administrador);
    }
}