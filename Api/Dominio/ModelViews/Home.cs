using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews
{
    public class Home
    {
        public string Mensagem {get => "Bem vindo(a) a Minimal API!";}
        public string Documentacao {get => "/swagger";}
    }
}