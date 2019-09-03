using System;
namespace MegaSolucao.Negocio.DTOs
{
    [Serializable]
    public class DtoLigacao
    {
        public string position { get; set; }

        public string Numero { get; set; }

        public string Ramal { get; set; }

        public string DataHora { get; set; }

        public string Duracao { get; set; }

        public string Tipo { get; set; }

        public string UniqueId { get; set; }
    }
}
