using MinimalApi.Dominio.Enums;

namespace MinimalApi.Dominio.DTOs
{
    public class AdministradorDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public PerfilEnum Perfil { get; set; }
    }
}