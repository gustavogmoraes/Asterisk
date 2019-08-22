using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaSolucao.Infraestrutura
{
    public class Configuracao
    {
        public ConexaoAsterisk ConexaoAsterisk { get; set; }

        public TimeSpan CooldownExecutarLigacoes { get; set; }
    }
}
