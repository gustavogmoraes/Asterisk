using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace MegaSolucao.Persistencia.BancoDeDados.Raven
{
    public static class PersistenciaRaven
    {
        private static readonly GSDocumentStore DocumentStore = new GSDocumentStore();

        public static IDocumentSession AbraSessao()
        {
            return DocumentStore.OpenSession();
        }

        public static IAsyncDocumentSession AbraSessaoAssincrona()
        {
            return DocumentStore.OpenAsyncSession();
        }
    }
}
