using System;
using System.Linq;

using System.Collections.Generic;
using System.Data;
using MegaSolucao.Negocio.DTOs;
using MegaSolucao.Negocio.Objetos;
using MegaSolucao.Persistencia.BancoDeDados.MySql;
using System.Globalization;
using MegaSolucao.Utilitarios;
using Raven.Client.Documents.Linq.Indexing;

namespace MegaSolucao.Negocio.Servicos
{
    public class ServicoDeLigacoes : IDisposable
    {
        public List<DtoLigacao> ObtenhaLigacoes(DtoConsultaLigacoes filtro)
        {
            var filtros = new List<string>();

            var dataHoraInicio = !string.IsNullOrEmpty(filtro.DataInicio + filtro.HoraInicio)
                         ? $"{filtro.DataInicio} {filtro.HoraInicio}"
                         : string.Empty;

            var dataHoraFim = !string.IsNullOrEmpty(filtro.DataFim + filtro.HoraFim)
                         ? $"{filtro.DataFim} {filtro.HoraFim}"
                         : string.Empty;

            if (!string.IsNullOrEmpty(dataHoraInicio))
            {
                filtros.Add($"calldate >= '{dataHoraInicio}'" +
                            $"AND calldate <= '{dataHoraFim}' ");
            }
                

            var filtroFinal = string.Join("AND ,", filtros);
            if (filtros.Count > 0)
            {
                filtroFinal = filtroFinal.Remove(filtroFinal.Length - 5);
            }


            var query = $"SELECT src, dst, calldate, userfield, duration " +
                        $"FROM cdr ";
            if (!string.IsNullOrEmpty(filtroFinal))
            {
                query += $"WHERE {filtroFinal};";
            }

            var dataTable = PersistenciaMySql.ExecuteConsulta(query);
            
            var ligacoes = dataTable.Rows.OfType<DataRow>().Select(MonteObjeto).ToList();
            ligacoes.ForEach(x =>
            {
          
                x.Tipo = x.Origem.Length > 4
                       ? "Recebida"
                       : "Originada";
            });

            var dtos = ligacoes.Select(ConvertaParaDto);

            var dtoLigacoes = dtos.ToList();
            var novasLigacoes = dtoLigacoes
                .Where(dtoLigacao => dtoLigacao.Tipo == "Originada" && dtoLigacao.Numero.Length <= 4)
                .Select(dtoLigacao => dtoLigacao.CloneObjeto(x =>
                {
                    var rml = x.Ramal;

                    x.Ramal = x.Numero;
                    x.Numero = rml;
                    x.Tipo = "Recebida";
                })).ToList();

            dtoLigacoes.AddRange(novasLigacoes);
            dtos = dtoLigacoes;

            if (!string.IsNullOrEmpty(filtro.Tipo))
            {
                dtos = dtoLigacoes.Where(x => x.Tipo == filtro.Tipo);
            }

            if (!string.IsNullOrEmpty(filtro.Numero))
            {
                dtos = dtoLigacoes.Where(x => x.Numero == filtro.Numero);
            }

            if (!string.IsNullOrEmpty(filtro.Ramal))
            {
                dtos = dtoLigacoes.Where(x => x.Ramal == filtro.Ramal);
            }

            int i = 1;
            var dtosPosition = dtos.OrderByDescending(x => x.DataHora).ToList();
            dtosPosition.ForEach(x =>
            {
              x.position = i++.ToString();
          
              });
            return dtosPosition;
    }

        private DtoLigacao ConvertaParaDto(Ligacao ligacao)
        {
      return new DtoLigacao
      {
                position = ligacao.position,
                DataHora = ligacao.Data.ToString("dd/MM/yyyy HH:mm:ss"),
                Duracao = ligacao.Duracao.ToString(),
                Numero = ligacao.Tipo == "Recebida"
                       ? ligacao.Origem
                       : ligacao.Destino,
                Ramal = ligacao.Tipo == "Originada"
                      ? ligacao.Origem
                      : ligacao.Destino,
                Tipo = ligacao.Tipo
            };
        }

        private Ligacao MonteObjeto(DataRow linha)
        {
            return new Ligacao
            {
               
                Origem = linha["src"].ToString(),
                Destino = linha["dst"].ToString(),
                UserField = linha["userfield"].ToString(),
                Data = (DateTime)linha["calldate"],
                Duracao = TimeSpan.FromSeconds((int)linha["duration"])
            };
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ServicoDeLigacoes()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
