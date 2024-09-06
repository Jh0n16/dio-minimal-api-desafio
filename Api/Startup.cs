using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.Validacoes;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private string key;
        public Startup(IConfiguration config)
        {
            Configuration = config;
            key = Configuration.GetSection("Jwt").ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT:"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            services.AddDbContext<DbContexto>(op =>
            {
                op.UseMySql(
                    Configuration.GetConnectionString("MySql"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql"))
                );
            });
            services.AddScoped<IAdministradorServico, AdministradorServico>();
            services.AddScoped<IVeiculoServico, VeiculoServico>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                #region Home
                endpoints.MapGet("/", () => Results.Json(new Home())).WithTags("Home").AllowAnonymous();
                #endregion

                #region Veiculos
                endpoints.MapGet("/veiculos", ([FromQuery] int pagina, IVeiculoServico veiculoServico) =>
                {
                    List<Veiculo> veiculos = veiculoServico.Todos(pagina: pagina);

                    return Results.Ok(veiculos);

                }).WithTags("Veiculos").RequireAuthorization();

                endpoints.MapGet("/veiculo/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
                {
                    Veiculo veiculo = veiculoServico.BuscaPorId(id);

                    if (veiculo == null) return Results.NotFound();

                    return Results.Ok(veiculo);

                }).WithTags("Veiculos").RequireAuthorization();

                endpoints.MapPost("/veiculos", ([FromBody] VeiculoDto veiculoDto, IVeiculoServico veiculoServico) =>
                {

                    ErroDeValidacao erro = new ValidaDtos().ValidaVeiculoDto(veiculoDto);
                    if (erro.ExisteErro) return Results.BadRequest(erro);

                    Veiculo veiculo = new Veiculo
                    {
                        Nome = veiculoDto.Nome,
                        Marca = veiculoDto.Marca,
                        Ano = veiculoDto.Ano
                    };

                    veiculoServico.Adicionar(veiculo);

                    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);

                }).WithTags("Veiculos").RequireAuthorization();

                endpoints.MapPut("/veiculo/{id}", ([FromRoute] int id, [FromBody] VeiculoDto veiculoDto, IVeiculoServico veiculoServico) =>
                {
                    Veiculo veiculo = veiculoServico.BuscaPorId(id);
                    if (veiculo == null) return Results.NotFound();

                    ErroDeValidacao erro = new ValidaDtos().ValidaVeiculoDto(veiculoDto);
                    if (erro.ExisteErro) return Results.BadRequest(erro);

                    veiculo.Nome = veiculoDto.Nome;
                    veiculo.Marca = veiculoDto.Marca;
                    veiculo.Ano = veiculoDto.Ano;

                    veiculoServico.Atualizar(veiculo);

                    return Results.Ok(veiculo);

                }).WithTags("Veiculos").RequireAuthorization();

                endpoints.MapDelete("/veiculo/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
                {
                    Veiculo veiculo = veiculoServico.BuscaPorId(id);
                    if (veiculo == null) return Results.NotFound();

                    veiculoServico.Apagar(veiculo);

                    return Results.NoContent();

                }).WithTags("Veiculos").RequireAuthorization();
                #endregion

                #region Admin
                string GerarToken(Administrador adm)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>(){
                        new Claim("Email", adm.Email),
                        new Claim("Perfil", adm.Perfil),
                        new Claim(ClaimTypes.Role, adm.Perfil)
                    };

                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: credentials
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }

                endpoints.MapPost("/admin/login", ([FromBody] LoginDto login, IAdministradorServico administradorServico) =>
                {
                    Administrador adm = administradorServico.Login(login);
                    if (adm != null)
                    {
                        string token = GerarToken(adm);
                        return Results.Ok(new AdministradorLogado
                        {
                            Email = adm.Email,
                            Perfil = adm.Perfil,
                            Token = token
                        });
                    }

                    return Results.Unauthorized();

                })
                .WithTags("Admin")
                .AllowAnonymous();

                endpoints.MapGet("/admin", ([FromQuery] int pagina, IAdministradorServico administradorServico) =>
                {
                    List<Administrador> administradores = administradorServico.ListarTodos(pagina);
                    List<AdministradorModelView> listaParaExibicaoDeAdministradores = new List<AdministradorModelView>();

                    foreach (Administrador adm in administradores)
                    {
                        listaParaExibicaoDeAdministradores.Add(new AdministradorModelView
                        {
                            Id = adm.Id,
                            Email = adm.Email,
                            Perfil = adm.Perfil.ToString()
                        });
                    }

                    return Results.Ok(listaParaExibicaoDeAdministradores);

                })
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                endpoints.MapPost("/admin", ([FromBody] AdministradorDto administradorDto, IAdministradorServico administradorServico) =>
                {
                    ErroDeValidacao erro = new ValidaDtos().ValidaAdministradorDto(administradorDto);
                    if (erro.ExisteErro) return Results.BadRequest(erro);

                    Administrador admin = new Administrador
                    {
                        Email = administradorDto.Email,
                        Senha = administradorDto.Senha,
                        Perfil = administradorDto.Perfil.ToString()
                    };

                    administradorServico.Adicionar(admin);

                    return Results.Created($"admin/{admin.Id}", admin);

                })
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                endpoints.MapGet("/admin/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
                {
                    Administrador administrador = administradorServico.BuscarPorId(id);
                    if (administrador == null) return Results.NotFound();

                    return Results.Ok(administrador);

                })
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                endpoints.MapPut("/admin/{id}", ([FromRoute] int id, [FromBody] AdministradorDto administradorDto, IAdministradorServico administradorServico) =>
                {
                    Administrador administrador = administradorServico.BuscarPorId(id);
                    if (administrador == null) return Results.NotFound();

                    ErroDeValidacao erro = new ValidaDtos().ValidaAdministradorDto(administradorDto);
                    if (erro.ExisteErro) return Results.BadRequest();

                    administrador.Email = administradorDto.Email;
                    administrador.Perfil = administradorDto.Perfil.ToString();
                    administrador.Senha = administradorDto.Senha;

                    administradorServico.Atualizar(administrador);

                    return Results.Ok(administrador);

                })
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");

                endpoints.MapDelete("/admin/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
                {
                    Administrador administrador = administradorServico.BuscarPorId(id);
                    if (administrador == null) return Results.NotFound();

                    administradorServico.Apagar(administrador);

                    return Results.NoContent();

                })
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
                .WithTags("Admin");
                #endregion
            });
        }
    }
}