using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaSolucao.Infraestrutura
{
    public class Configuracao
    {
        public ConexaoAsterisk ConexaoAsterisk { get; set; }

        public ConexaoRavenDB ConexaoRavenDB { get; set; }

        public ConexaoMySql ConexaoMySql { get; set; }

        public TimeSpan CooldownExecutarLigacoes { get; set; }
    }
}
