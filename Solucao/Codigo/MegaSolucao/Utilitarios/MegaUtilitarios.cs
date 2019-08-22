using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AsterNET.Manager;
using MegaSolucao.Infraestrutura;

namespace MegaSolucao.Utilitarios
{
    public static class MegaUtilitarios
    {
        public static ManagerConnection ToManagerConnection(this ConexaoAsterisk conexaoAsterisk, Action<ManagerConnection> objectInitializer = null)
        {
            var manager = new ManagerConnection(conexaoAsterisk.HostDoAsterisk, conexaoAsterisk.PortaDoAsterisk, conexaoAsterisk.LoginDoAsterisk, conexaoAsterisk.SenhaDoAsterisk);
            objectInitializer?.Invoke(manager);

            return manager;
        }
    }
}
