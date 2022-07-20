using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Domain;

namespace SistemaPedidos.Data
{
    public class ApplicationContext : DbContext
    {

        public DbSet<Pedido> Pedidos { get; set; }

        /// <summary>
        /// Aqui informamos qual o provider que estamos utilizando, 
        /// neste caso é o SqlServer.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Esse método recebe uma string de conexão no paramêtro.
            optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaPedidos;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Cliente>(p =>
            {                
                p.ToTable("Clientes");//nome tabela
                p.HasKey(p => p.Id);//haskey: informamos a chave primária.
                //IsRequired significa que é obrigatório.
                p.Property(p => p.Name).HasColumnType("VARCHAR(80)").IsRequired();
                p.Property(p => p.Telefone).HasColumnType("CHAR(11)");
                p.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
                p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
                p.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

                //indice
                p.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");
            });

            modelBuilder.Entity<Produto>(p =>
            {
                p.ToTable("Produtos");
                p.HasKey(p => p.Id);
                p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
                p.Property(p => p.Valor).IsRequired();
                //tipo produto é um enum então será convertido como string
                p.Property(p => p.TipoProduto).HasConversion<string>();
            });


            modelBuilder.Entity<Pedido>(p =>
            {
                p.ToTable("Pedidos");
                p.HasKey(p => p.Id);
                //HasDefaultValueSql vai usar o comando GetDate que retorna a hora atual e será adicionado pelo generatedOnAdd
                p.Property(p => p.IniciadoEm).HasDefaultValueSql("GetDate()").ValueGeneratedOnAdd();
                p.Property(p => p.Status).HasConversion<string>();
                p.Property(p => p.TipoFrete).HasConversion<int>();
                p.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

                //configura o relacionamento muitos itens para 1 pedido e exclui tudo que tiver nele quando for deletado
                p.HasMany(p => p.Itens)
                    .WithOne(p => p.Pedido)
                    .OnDelete(DeleteBehavior.Cascade);    
      
            });


            modelBuilder.Entity<PedidoItem>(p =>
            {
                p.ToTable("PedidoItens");
                p.HasKey(p => p.Id);
                p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired();
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.Desconto).IsRequired();
            });
                
        }
    }
}
