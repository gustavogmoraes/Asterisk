using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static TTipo CloneObjeto<TTipo>(this TTipo objeto, Action<TTipo> objectInitializer = null)
            where TTipo : class, new()
        {
            var novoObjeto = new TTipo();

            objeto.GetType().GetProperties().ToList().ForEach(propriedade =>
            {
                var valorProp = propriedade.GetValue(objeto);
                propriedade.SetValue(novoObjeto, valorProp, null);
            });

            objectInitializer?.Invoke(novoObjeto);

            return novoObjeto;
        }

        public static string ConvertaStringDateTimePtBrParaEnUs(this string stringDateTime)
        {
            var datetime = Convert.ToDateTime(stringDateTime, CultureInfo.GetCultureInfo("pt-BR"));

            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
