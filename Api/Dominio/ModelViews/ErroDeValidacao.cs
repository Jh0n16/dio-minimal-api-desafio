using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews
{
    public class ErroDeValidacao
    {
        public bool ExisteErro { get; set; } = false;
        public List<string> Mensagens { get; set; } = new List<string>();
    }
}