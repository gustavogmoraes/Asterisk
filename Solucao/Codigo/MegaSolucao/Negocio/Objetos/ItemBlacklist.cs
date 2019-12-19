using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaSolucao.Negocio.Objetos
{
    public class ItemBlacklist
    {
        public string UniqueId { get; set; }

        public DateTime DataDoCadastro { get; set; }

        public string Numero { get; set; }

        public string Observacao { get; set; }
    }
}
