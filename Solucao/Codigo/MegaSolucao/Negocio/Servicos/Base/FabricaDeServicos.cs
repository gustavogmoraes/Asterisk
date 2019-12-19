using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MegaSolucao.Negocio.Servicos.Base
{
    public static class FabricaDeServicos
    {
        public static ServicoPadrao<T> CriePadrao<T>()
            where T: class, new()
        {
            return Activator.CreateInstance<ServicoPadrao<T>>();
        }

        public static TServico Crie<TServico>()
            where TServico : ServicoPadrao<object>
        {
            return Activator.CreateInstance<TServico>();
        }
    }
}
