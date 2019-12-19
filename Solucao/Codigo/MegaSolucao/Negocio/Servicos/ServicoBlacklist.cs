using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using MegaSolucao.Infraestrutura;
using MegaSolucao.Negocio.DTOs;
using MegaSolucao.Negocio.Objetos;
using MegaSolucao.Negocio.Servicos.Base;
using MegaSolucao.Persistencia.BancoDeDados.MySql;
using MegaSolucao.Persistencia.BancoDeDados.Raven;
using MegaSolucao.Utilitarios;

namespace MegaSolucao.Negocio.Servicos
{
    public class ServicoBlacklist : ServicoPadrao<ItemBlacklist>
    {

    }
}
