using System;
namespace MegaSolucao.Negocio.Objetos
{
    public class Ligacao
    {
        public string Origem { get; set; }

        public string Destino { get; set; }

        public string Tipo { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan Duracao { get; set; }

        public string UserField { get; set; }

        public string position { get; set; }
  }
}
