using MegaSolucao.Persistencia.BancoDeDados.Raven;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MegaSolucao.Negocio.Servicos.Base
{
    public class ServicoPadrao<T> : IDisposable
        where T: class, new()
    {
        public T Consulte(string id)
        {
            using (var sessaoRaven = PersistenciaRavenDb.AbraSessao())
            {
                return sessaoRaven.Load<T>(id);
            }
        }

        public List<T> ConsulteLista(Expression<Func<T, bool>> filtro = null)
        {
            using (var sessaoRaven = PersistenciaRavenDb.AbraSessao())
            {
                return filtro == null
                    ? sessaoRaven.Query<T>().ToList()
                    : sessaoRaven.Query<T>().Where(filtro).ToList();
            }
        }

        public void Insira(T objeto)
        {
            using (var sessaoRaven = PersistenciaRavenDb.AbraSessao())
            {
                var idProperty = objeto.GetType().GetProperty("Id");
                if (idProperty != null)
                {
                    sessaoRaven.Store(objeto, idProperty.GetValue(objeto).ToString());
                }
                else
                {
                    sessaoRaven.Store(objeto);
                }

                sessaoRaven.SaveChanges();
            }
        }

        public void Atualize(T objeto)
        {
            using (var sessaoRaven = PersistenciaRavenDb.AbraSessao())
            {
                var id = objeto.GetType().GetProperty("Id")?.GetValue(objeto).ToString();

                var objetoPersistido = sessaoRaven.Load<T>(id);
                if (objetoPersistido != null)
                {
                    objetoPersistido = objeto;

                    sessaoRaven.SaveChanges();
                }
            }
        }

        public void Exclua(string id)
        {
            using (var sessaoRaven = PersistenciaRavenDb.AbraSessao())
            {
                sessaoRaven.Delete(id);
            }
        }

        public void Dispose()
        {

        }
    }
}
