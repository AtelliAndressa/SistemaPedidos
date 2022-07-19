using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.ValueObjects
{
    /// <summary>
    /// CIF - remetente paga o frete
    /// FOB - destinatário paga o frete
    /// SemFrete - retirado na loja
    /// </summary>
    public enum TipoFrete
    {
        CIF,

        FOB,

        SemFrete
    }
}
