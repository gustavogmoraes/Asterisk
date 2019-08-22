using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace MegaSolucao.Infraestrutura
{
    public class ConexaoMySql
    {
        public string Servidor { get; set; }

        public string NomeDoBanco { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }
    }
}
