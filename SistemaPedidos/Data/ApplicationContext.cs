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
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaPedidos;Integrated Security=true",
                p => p.EnableRetryOnFailure(
                    maxRetryCount: 2, 
                    maxRetryDelay: TimeSpan.FromSeconds(5), 
                    errorNumbersToAdd: null));
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
            MapearPropriedadesEsquecidas(modelBuilder);
        }    


        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string)); 
                foreach(var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        //toda vez que ele achar uma propriedade do tipo string ele irá
                        //configurar ela como varchar 100
                        //property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}
