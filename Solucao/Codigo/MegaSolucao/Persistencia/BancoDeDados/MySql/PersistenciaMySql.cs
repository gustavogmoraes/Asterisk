using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MegaSolucao.Infraestrutura;

namespace MegaSolucao.Persistencia.BancoDeDados.MySql
{
    public static class PersistenciaMySql
    {
        #region Constantes

        #endregion

        #region Propriedades

        private static string _stringDeConexao;

        #endregion

        #region Construtores

        static PersistenciaMySql()
        {
            // Configuração
            DefinaStringDeConexao(Sessao.Configuracao.ConexaoMySql);

        }

        private static SqlConnection AbraConexao()
        {
            SqlConnection conexao = null;
            try
            {
                conexao = new SqlConnection(_stringDeConexao);
                conexao.Open();
            }
            catch (Exception e)
            {
                // ignored
            }

            return conexao;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Define a string de conexão com o banco de dados.
        /// </summary>
        private static void DefinaStringDeConexao(ConexaoMySql conexao)
        {
            _stringDeConexao = $@"Server = {conexao.Servidor}; Database = {conexao.NomeDoBanco}; Uid = {conexao.Usuario}; Pwd = {conexao.Senha};";
        }

        ///// <summary>
        ///// Define a string de conexão com o banco de dados para o computador do desenvolvedor
        ///// </summary>
        //public void DefinaStringDeConexao()
        //{
        //    _stringDeConexao = @"Server = RAGNOS\SQLEXPRESS; Database = CopiaProducao; Trusted_Connection = Yes";
        //}

        #region Métodos de query

        /// <summary>
        /// Executa diretamente a query SQL enviada.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        public static void ExecuteComando(string comandoSQL)
        {
            try
            {
                using (var conexao = AbraConexao())
                using (var cmd = new SqlCommand(comandoSQL, conexao))
                {
                    //Executa a query
                    cmd.ExecuteReader();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Executa diretamente a query SQL enviada e retorna o resultado retorno tupla única.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        /// <returns>
        /// DataReader com o resultado da query.
        /// </returns>
        public static string ExecuteConsultaRetornoUnico(string comandoSQL)
        {
            try
            {
                var resultadoConsulta = string.Empty;

                using (var conexao = AbraConexao())
                using (var cmd = new SqlCommand(comandoSQL, conexao))
                {
                    resultadoConsulta = cmd.ExecuteScalar().ToString();
                }

                return !string.IsNullOrEmpty(resultadoConsulta)
                    ? resultadoConsulta
                    : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Executa diretamente a query SQL enviada e retorna o resultado retorno tupla única.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        /// <returns>
        /// DataReader com o resultado da query.
        /// </returns>
        public static TipoRetorno ExecuteConsultaRetornoUnico<TipoRetorno>(string comandoSQL)
        {
            var resultadoConsulta = ExecuteConsultaRetornoUnico(comandoSQL);

            return (TipoRetorno)Convert.ChangeType(resultadoConsulta, typeof(TipoRetorno));
        }

        /// <summary>
        /// Executa diretamente a query SQL enviada e retorna o resultado retorno tupla única.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        /// <returns>
        /// DataReader com o resultado da query.
        /// </returns>
        public static dynamic ExecuteConsultaRetornoUnico(string comandoSQL, Type tipoRetorno)
        {
            var resultadoConsulta = ExecuteConsultaRetornoUnico(comandoSQL);

            return !string.IsNullOrEmpty(resultadoConsulta)
                ? tipoRetorno.GetMethod("Parse", new[] { typeof(string) })?.Invoke(null, new object[] { resultadoConsulta })
                : null;
        }

        /// <summary>
        /// Executa diretamente a query SQL enviada e retorna o resultado.
        /// </summary>
        /// <param name="comandoSQL">A query SQL que será executada.</param>
        /// <returns>
        /// DataReader com o resultado da query.
        /// </returns>
        public static DataTable ExecuteConsulta(string comandoSQL)
        {
            try
            {
                var dataTable = new DataTable();

                using (var conexao = AbraConexao())
                using (var cmd = new SqlCommand(comandoSQL, conexao))
                {
                    //Executa a query
                    var reader = cmd.ExecuteReader();
                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        #endregion


        #region Métodos auxiliares

        public static bool VerifiqueStatusDaConexao()
        {
            try
            {
                using (var conexao = AbraConexao())
                using (var cmd = new SqlCommand("SELECT 1", conexao))
                {
                    //Executa a query
                    var retorno = cmd.ExecuteScalar();

                    return (int)retorno == 1;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        #endregion

        #endregion
    }
}
