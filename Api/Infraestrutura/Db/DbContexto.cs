using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db
{
    public class DbContexto: DbContext 
    {
        private readonly IConfiguration _contexto;
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        
        public DbContexto(IConfiguration contexto)
        {
            _contexto = contexto;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                string connectionString = _contexto.GetConnectionString("MySql").ToString();
                if(!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString)
                    );
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>().HasData(
                new Administrador {
                    Id = 1,
                    Email = "admin@email.com",
                    Senha = "senhasegura",
                    Perfil = "adm" 
                }
            );
        }

    }
}