using System;
namespace MegaSolucao.Negocio.DTOs
{
    [Serializable]
    public class DtoLigacao
    {
        public string Numero { get; set; }

        public string Ramal { get; set; }

        public string Data { get; set; }

        public string Hora { get; set; }

        public string Tipo { get; set; }
    }
}
