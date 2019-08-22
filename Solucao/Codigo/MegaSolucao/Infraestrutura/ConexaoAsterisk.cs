using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaSolucao.Infraestrutura
{
    public class ConexaoAsterisk
    {
        public string HostDaAplicacao { get; set; }

        public string HostDoAsterisk { get; set; }

        public int PortaDoAsterisk { get; set; }

        public string LoginDoAsterisk { get; set; }

        public string SenhaDoAsterisk { get; set; }
    }
}
