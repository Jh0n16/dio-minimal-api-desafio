using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Interfaces
{
    public interface IVeiculoServico
    {
        public List<Veiculo> Todos(int pagina=1, string nome=null, string marca=null);        
        public Veiculo BuscaPorId(int id);
        public void Adicionar(Veiculo veiculo);
        public void Atualizar(Veiculo veiculo);
        public void Apagar(Veiculo veiculo);
    }
}