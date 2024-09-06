using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class AdministradorServico: IAdministradorServico
    {
        private readonly DbContexto _ctx;
        public AdministradorServico(DbContexto contexto)
        {
            _ctx = contexto;
        }

        public void Adicionar(Administrador administrador)
        {
            _ctx.Add(administrador);
            _ctx.SaveChanges();
        }

        public void Atualizar(Administrador administrador)
        {
            _ctx.Update(administrador);
            _ctx.SaveChanges();
        }

        public void Apagar(Administrador administrador)
        {
            _ctx.Remove(administrador);
            _ctx.SaveChanges();
        }

        public List<Administrador> ListarTodos(int pagina = 1)
        {
            int itensPorPagina = 10;
            List<Administrador> administradores = _ctx.Administradores.ToList<Administrador>();
            
            return administradores.Skip((pagina - 1) * itensPorPagina).Take(itensPorPagina).ToList<Administrador>();
        }

        public Administrador Login(LoginDto login)
        {
            Administrador adm = _ctx.Administradores.Where(adm => adm.Email == login.Email && adm.Senha == login.Senha).FirstOrDefault();

            return adm;
        }

        public Administrador BuscarPorId(int id)
        {
            return _ctx.Administradores.Where(adm => adm.Id == id).FirstOrDefault();
        }
    }
}