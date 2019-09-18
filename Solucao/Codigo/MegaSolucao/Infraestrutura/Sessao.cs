using System;
using System.Collections.Generic;
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
            Configuracao = new Configuracao
            {
                ConexaoAsterisk = new ConexaoAsterisk
                {
                    HostDaAplicacao = "172.16.2.132",
                    HostDoAsterisk = "35.199.90.33",
                    PortaDoAsterisk = 5038,
                    LoginDoAsterisk = "snep",
                    SenhaDoAsterisk = "sneppass"
                },
                ConexaoRavenDB = new ConexaoRavenDB
                {
                    Servidor = @"localhost:32772",
                    NomeDoBanco = "Test"
                },
                ConexaoMySql = new ConexaoMySql
                {
                    Servidor = "35.199.90.33",
                    Porta = 3306,
                    NomeDoBanco = "snep",
                    Usuario = "root",
                    Senha = "sneppass"
                },
                HostApiAutenticacao = "https://localhost:5003/api/Auth/",
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
