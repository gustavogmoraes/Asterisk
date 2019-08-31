using System;
using System.Linq;

using System.Collections.Generic;
using System.Data;
using MegaSolucao.Negocio.DTOs;
using MegaSolucao.Negocio.Objetos;
using MegaSolucao.Persistencia.BancoDeDados.MySql;
using System.Globalization;

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
                filtros.Add($"calldate >= '{dataHoraInicio}'" +
                            $"AND calldate <= '{dataHoraFim}' ");

            var filtroFinal = string.Join("AND ,", filtros);
            filtroFinal = filtroFinal.Remove(filtroFinal.Length - 5);

            var query = $"SELECT src, dst, calldate, userfield " +
                        $"FROM cdr " +
                        $"WHERE {filtroFinal};";

            var dataTable = PersistenciaMySql.ExecuteConsulta(query);

            var ligacoes = dataTable.Rows.OfType<DataRow>().Select(MonteObjeto).ToList();
            ligacoes.ForEach(x =>
            {
                x.Tipo = x.Origem.Length > 4
                       ? "Recebida"
                       : "Originada";
            });

            var dtos = ligacoes.Select(ConvertaParaDto);

            if (!string.IsNullOrEmpty(filtro.Tipo))
            {
                dtos = dtos.Where(x => x.Tipo == filtro.Tipo);
            }

            if (!string.IsNullOrEmpty(filtro.Numero))
            {
                dtos = dtos.Where(x => x.Numero == filtro.Numero);
            }

            if (!string.IsNullOrEmpty(filtro.Ramal))
            {
                dtos = dtos.Where(x => x.Ramal == filtro.Ramal);
            }

            return dtos.ToList();
        }

        private DtoLigacao ConvertaParaDto(Ligacao ligacao)
        {
            return new DtoLigacao
            {
                Data = ligacao.Data.ToString("dd/MM/yyyy"),
                Hora = ligacao.Data.ToString("HH:mm:SS"),
                Numero = ligacao.Tipo == "Recebida"
                       ? ligacao.Origem
                       : ligacao.Destino,
                Ramal = ligacao.Tipo == "Originada"
                      ? ligacao.Origem
                      : ligacao.Destino
            };
        }

        private Ligacao MonteObjeto(DataRow linha)
        {
            return new Ligacao
            {
                Origem = linha["src"].ToString(),
                Destino = linha["dst"].ToString(),
                UserField = linha["userfield"].ToString(),
                Data = DateTime.ParseExact(linha["calldate"].ToString(), "yyyy-MM-dd HH:mm:SS", CultureInfo.InvariantCulture)
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
