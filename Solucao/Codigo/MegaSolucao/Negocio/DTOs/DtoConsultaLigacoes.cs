using System;
namespace MegaSolucao.Negocio.DTOs
{
    [Serializable]
    public class DtoConsultaLigacoes
    {
        public string Numero { get; set; }

        public string Ramal { get; set; }

        public string DataInicio { get; set; }

        public string HoraInicio { get; set; }

        public string DataFim { get; set; }

        public string HoraFim { get; set; }

        public string Tipo { get; set; }
    }
}
