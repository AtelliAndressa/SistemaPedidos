using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidos.Domain;

namespace SistemaPedidos.Data.Configurations
{
    public class ClienteConfigurations : IEntityTypeConfiguration<Cliente>
    {
        /// <summary>
        /// Aqui configuramos todo o modelo de dados 
        /// individualmente para cada entidade, 
        /// fazemos o mapeamento do modelo de dados com Fluent API
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");//nome tabela
            builder.HasKey(p => p.Id);//haskey: informamos a chave primária.

            //IsRequired significa que é obrigatório.
            builder.Property(p => p.Name).HasColumnType("VARCHAR(80)").IsRequired();
            builder.Property(p => p.Telefone).HasColumnType("CHAR(11)");
            builder.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
            builder.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
            builder.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

            //indice
            builder.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone");
        }
    }
}
