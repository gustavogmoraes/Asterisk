using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaSolucao.Negocio.DTOs;
using MegaSolucao.Negocio.Objetos;
using MegaSolucao.Negocio.Servicos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MegaSolucao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LigacoesController : ControllerBase
    {
        private ServicoDeLigacoes _servico;
        private ServicoDeLigacoes Servico => _servico ?? (_servico = new ServicoDeLigacoes());

        [HttpPost("[action]")]
        public ActionResult<List<DtoLigacao>> ConsulteLigacoes([FromBody]DtoConsultaLigacoes filtro)
        {
            return Servico.ObtenhaLigacoes(filtro);
        }
      /*
        [HttpGet("[action]?{uniqueId}")]
        public FileStreamResult ObtenhaGravacao(string uniqueId)
        {
            return new FileStreamResult(Servico.ObtenhaGravacao(uniqueId).Result, ".wav");
        }
        */
    }
}
