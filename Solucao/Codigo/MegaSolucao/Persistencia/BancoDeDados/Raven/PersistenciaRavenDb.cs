using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MegaSolucao.Infraestrutura;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace MegaSolucao.Persistencia.BancoDeDados.Raven
{
    public static class PersistenciaRavenDb
    {
        private static DocumentStore DocumentStore { get; }

        static PersistenciaRavenDb()
        {
            DocumentStore = new DocumentStore
            {
                Urls = new[] {@"http://" + Sessao.Configuracao.ConexaoRavenDB.Servidor},
                Database = Sessao.Configuracao.ConexaoRavenDB.NomeDoBanco
            };

            SobrescrevaConventions(DocumentStore);
        }

        private static void SobrescrevaConventions(DocumentStore documentStore)
        {
            DocumentStore.Conventions.FindCollectionName = type =>
                NomenclaturaDeColecaoCustomizada.ContainsKey(type)
                    ? NomenclaturaDeColecaoCustomizada[type]
                    : DocumentConventions.DefaultGetCollectionName(type);
        }

        public static Dictionary<Type, string> NomenclaturaDeColecaoCustomizada =>
            new Dictionary<Type, string>
            {
                //{ typeof(Interacao), "Interacoes" }
            };

        public static IDocumentSession AbraSessao()
        {
            DocumentStore.Initialize();
            return DocumentStore.OpenSession();
        }

        public static IAsyncDocumentSession AbraSessaoAssincrona()
        {
            return DocumentStore.OpenAsyncSession();
        }

        #region Operações de banco

        public static bool VerifiqueSeBancoExiste(string nomeDoBanco)
        {
            var result = DocumentStore.Maintenance.Server.Send(new GetDatabaseRecordOperation(nomeDoBanco));

            return result != null;
        }

        public static bool VerifiqueSeServidorEstahOnline(int timeoutMilliseconds = 5000)
        {
            var conexaoOk = ObtenhaVersaoDoServidor(out _);
            var databaseExists = false;

            if (conexaoOk) databaseExists = VerifiqueSeBancoExiste(DocumentStore.Database);

            return conexaoOk && databaseExists;
        }

        public static void CompactarBancoDeDados(string databaseName = null)
        {
            // Get all index names
            var indexNames = DocumentStore.Maintenance.Send(new GetIndexNamesOperation(0, int.MaxValue));

            var settings = new CompactSettings
            {
                DatabaseName = databaseName ?? DocumentStore.Database,
                Documents = true,
                Indexes = indexNames
            };

            // Compact entire database: documents + all indexes
            var operation = DocumentStore.Maintenance.Server.Send(new CompactDatabaseOperation(settings));
            operation.WaitForCompletion();
        }

        private static bool ObtenhaVersaoDoServidor(out BuildNumber buildNumber, int timeoutMilliseconds = 5000)
        {
            try
            {
                var task = DocumentStore.Maintenance.Server.SendAsync(new GetBuildNumberOperation());
                var success = task.Wait(timeoutMilliseconds);
                buildNumber = task.Result;

                return success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                buildNumber = null;

                return false;
            }
        }

        #endregion
    }
}
