using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        private readonly DbContexto _ctx;
        public VeiculoServico(DbContexto contexto)
        {
            _ctx = contexto;
        }

        public void Apagar(Veiculo veiculo)
        {
            _ctx.Veiculos.Remove(veiculo);
            _ctx.SaveChanges();

        }

        public void Atualizar(Veiculo veiculo)
        {
            _ctx.Veiculos.Update(veiculo);
            _ctx.SaveChanges();
        }

        public Veiculo BuscaPorId(int id)
        {
            return _ctx.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Adicionar(Veiculo veiculo)
        {
            _ctx.Veiculos.Add(veiculo);
            _ctx.SaveChanges();
        }

        public List<Veiculo> Todos(int pagina = 1, string nome = null, string marca = null)
        {
            int itensPorPagina = 10;
            List<Veiculo> veiculos = _ctx.Veiculos.ToList<Veiculo>();

            if(!string.IsNullOrEmpty(nome))
                veiculos = _ctx.Veiculos.Where(v => v.Nome.Contains(nome)).ToList<Veiculo>();

            if(!string.IsNullOrEmpty(marca))
                veiculos = _ctx.Veiculos.Where(v => v.Marca.Contains(marca)).ToList<Veiculo>();
            
            return veiculos.Skip((pagina - 1) * itensPorPagina).Take(itensPorPagina).ToList<Veiculo>();
        }
    }
}