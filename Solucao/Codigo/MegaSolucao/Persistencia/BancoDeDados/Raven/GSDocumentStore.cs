using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MegaSolucao.Infraestrutura;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace MegaSolucao.Persistencia.BancoDeDados.Raven
{
    public class GSDocumentStore : DocumentStore
    {
        public GSDocumentStore()
        {
            Urls = new[] { Sessao.Configuracao.ConexaoRavenDB.Servidor };
            Database = Sessao.Configuracao.ConexaoRavenDB.NomeDoBanco;

            if (!string.IsNullOrEmpty(Sessao.Configuracao.ConexaoRavenDB.CaminhoCertificado))
            {
                Certificate = new X509Certificate2(Sessao.Configuracao.ConexaoRavenDB.CaminhoCertificado);
            }

            SobrescrevaConventions();
            Initialize();
        }

        public GSDocumentStore(string url, string database)
        {
            Urls = new[] { url };
            Database = database;

            if (!string.IsNullOrEmpty(Sessao.Configuracao.ConexaoRavenDB.CaminhoCertificado))
            {
                Certificate = new X509Certificate2(Sessao.Configuracao.ConexaoRavenDB.CaminhoCertificado);
            }

            SobrescrevaConventions();
            Initialize();
        }

        public sealed override IDocumentStore Initialize()
        {
            return base.Initialize();
        }

        private void SobrescrevaConventions()
        {
            base.Conventions.FindCollectionName = type =>
                NomenclaturaDeColecaoCustomizada.ContainsKey(type)
                    ? NomenclaturaDeColecaoCustomizada[type]
                    : DocumentConventions.DefaultGetCollectionName(type);
        }

        public static Dictionary<Type, string> NomenclaturaDeColecaoCustomizada =>
            new Dictionary<Type, string>
            {
                //{ typeof(Interacao), "Interacoes" }
            };

        #region Operações de banco

        private bool ObtenhaVersaoDoServidor(out BuildNumber buildNumber, int timeoutMilliseconds = 5000)
        {
            try
            {
                var task = Maintenance.Server.SendAsync(new GetBuildNumberOperation());
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

        public bool VerifiqueSeBancoExiste(string nomeDoBanco)
        {
            var result = Maintenance.Server.Send(new GetDatabaseRecordOperation(nomeDoBanco));

            return result != null;
        }

        public bool VerifiqueSeServidorEstahOnline(int timeoutMilliseconds = 5000)
        {
            var conexaoOk = ObtenhaVersaoDoServidor(out _);
            var databaseExists = false;

            if (conexaoOk) databaseExists = VerifiqueSeBancoExiste(Database);

            return conexaoOk && databaseExists;
        }

        public void CompactarBancoDeDados(string databaseName)
        {
            // Get all index names
            var indexNames = Maintenance.Send(new GetIndexNamesOperation(0, int.MaxValue));

            var settings = new CompactSettings
            {
                DatabaseName = databaseName,
                Documents = true,
                Indexes = indexNames
            };

            // Compact entire database: documents + all indexes
            var operation = Maintenance.Server.Send(new CompactDatabaseOperation(settings));
            operation.WaitForCompletion();
        }

        #endregion
    }
}
