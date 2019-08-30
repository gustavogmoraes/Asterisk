using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AsterNET.Manager;
using MegaSolucao.Infraestrutura;
using MegaSolucao.Persistencia.BancoDeDados.Raven;
using Newtonsoft.Json;

namespace MegaSolucao.Utilitarios
{
    public static class MegaUtilitarios
    {
        public static ManagerConnection ToManagerConnection(
            this ConexaoAsterisk conexaoAsterisk, Action<ManagerConnection> objectInitializer = null)
        {
            var manager = new ManagerConnection(
                conexaoAsterisk.HostDoAsterisk, conexaoAsterisk.PortaDoAsterisk, conexaoAsterisk.LoginDoAsterisk, conexaoAsterisk.SenhaDoAsterisk);
            objectInitializer?.Invoke(manager);

            return manager;
        }

        public static TObjeto ObtenhaObjetoCongelado<TObjeto>(this object objeto)
        {
            var json = JsonConvert.SerializeObject(objeto);

            return JsonConvert.DeserializeObject<TObjeto>(json);
        }

        public static void SalveNoBanco(this object objeto)
        {
            var method = typeof(MegaUtilitarios).GetMethod("ObtenhaObjetoCongelado")
                                                .MakeGenericMethod(new[] { objeto.GetType() });

            var objetoCongelado = method.Invoke(null, new[] { objeto });
            using (var sessaoRaven = PersistenciaRavenDb.AbraSessao())
            {
                sessaoRaven.Store(objetoCongelado);
                sessaoRaven.SaveChanges();
            }
        }

        public static void RegistreTodosOsEventos(this ManagerConnection manager)
        {
            var events = manager.GetType()
                                .GetEvents();

            foreach(var evento in events)
            {
                evento.AddEventHandler(
                manager,
                Delegate.CreateDelegate(evento.EventHandlerType,
                typeof(Program).GetMethod("EventSaveDelegate")));
            }
            
        }
    }
}
