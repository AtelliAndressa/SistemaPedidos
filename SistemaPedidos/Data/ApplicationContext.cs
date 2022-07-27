using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaPedidos.Domain;

namespace SistemaPedidos.Data
{
    public class ApplicationContext : DbContext
    {
        //escrevendo o logging:
        private static readonly ILoggerFactory _logger = LoggerFactory
            .Create(prop => prop.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        /// <summary>
        /// Aqui informamos qual o provider que estamos utilizando, 
        /// neste caso é o SqlServer.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Esse método recebe uma string de conexão no paramêtro e mostrará no console o logging.
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaPedidos;Integrated Security=true");
        }

        /// <summary>
        /// Aqui ele procura automaticamente todas as classes que estão 
        /// implementando o IEntityTypeConfiguration neste Assembly que estou executando, 
        /// utilizando Fluent API que traz mais opções que o DataAnnotations.
        /// fazemos o mapeamento do modelo de dados com Fluent API
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }    
    }
}
