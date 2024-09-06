using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.ModelViews;

namespace MinimalApi.Dominio.Validacoes
{
    public class ValidaDtos
    {
        public ErroDeValidacao ValidaVeiculoDto(VeiculoDto veiculoDto)
        {
            ErroDeValidacao erro = new ErroDeValidacao();

            if(string.IsNullOrEmpty(veiculoDto.Nome))
            {
                erro.ExisteErro = true;
                erro.Mensagens.Add("O nome do veículo não pode estar em branco!");
            }

            if(string.IsNullOrEmpty(veiculoDto.Marca))
            {
                erro.ExisteErro = true;
                erro.Mensagens.Add("A marca do veículo não pode estar em branco!");
            }

            if(veiculoDto.Ano < 1950)
            {
                erro.ExisteErro = true;
                erro.Mensagens.Add("O veículo é muito antigo! São aceitos somente veículos fabricados a partir de 1951.");
            }

            return erro;
        }

        public ErroDeValidacao ValidaAdministradorDto(AdministradorDto administradorDto)
        {
            ErroDeValidacao erro = new ErroDeValidacao();

            if(string.IsNullOrEmpty(administradorDto.Email)) 
            {
                erro.ExisteErro = true;
                erro.Mensagens.Add("O campo Email não pode ser vazio.");
            }

            if(string.IsNullOrEmpty(administradorDto.Senha)) 
            {
                erro.ExisteErro = true;
                erro.Mensagens.Add("O campo Senha não pode ser vazio.");
            }

            if(string.IsNullOrEmpty(administradorDto.Perfil.ToString())) 
            {
                erro.ExisteErro = true;
                erro.Mensagens.Add("O campo Perfil não pode ser vazio.");
            }

            return erro;

        }
    
    }
}