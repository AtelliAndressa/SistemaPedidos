using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain
{
    /// <summary>
    /// Aqui estou utilizando o DataAnnotations para exemplificar o Modelo de dados ao EFcore
    /// Muito utilizado para fazer validações, porém util para mapear o modelo de dados.
    /// O [Table] ou [column] é usado para especificar o nome correto da tabela ou coluna no db.
    /// </summary>

    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Column("Phone")] 
        public string Telefone { get; set; }

        public string CEP { get; set; }

        public string Estado { get; set; }

        public string Cidade { get; set; }
    }
}
