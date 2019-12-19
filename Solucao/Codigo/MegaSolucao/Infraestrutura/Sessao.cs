using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;

namespace MegaSolucao.Infraestrutura
{
    public static class Sessao
    {
        static Sessao()
        {
            var ipGoogleCloudMachine = "35.199.90.33";
            var ipAuthMachine = "34.95.253.242";

            Configuracao = new Configuracao
            {
                ConexaoAsterisk = new ConexaoAsterisk
                {
                    HostDaAplicacao = "172.16.2.132",
                    HostDoAsterisk = ipGoogleCloudMachine,
                    PortaDoAsterisk = 5038,
                    LoginDoAsterisk = "snep",
                    SenhaDoAsterisk = "sneppass"
                },
                ConexaoRavenDB = new ConexaoRavenDB
                {
                    Servidor = @"https://a.free.gsoftware.ravendb.cloud/",
                    CaminhoCertificado = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "free.gsoftware.client.certificate.pfx"),
                    NomeDoBanco = "Mega.Aplicacao"
                },
                ConexaoMySql = new ConexaoMySql
                {
                    Servidor = ipGoogleCloudMachine,
                    Porta = 3306,
                    Usuario = "root",
                    Senha = "sneppass",
                    NomeDoBanco = "snep"
                },
                HostApiAutenticacao = $@"https://{ipAuthMachine}:5003/api/Auth/",
                CooldownExecutarLigacoes = TimeSpan.FromSeconds(10)
            };
        }

        public static Configuracao Configuracao { get; set; }

        public static Uri ObtenhaUriBase()
        {
            return new Uri($"http://{Configuracao.ConexaoAsterisk.HostDoAsterisk}/");
        }
    }
}
