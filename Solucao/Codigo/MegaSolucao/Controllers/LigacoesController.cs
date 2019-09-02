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
        [HttpPost("[action]")]
        public ActionResult<List<DtoLigacao>> ConsulteLigacoes([FromBody]DtoConsultaLigacoes filtro)
        {
            using(var servico = new ServicoDeLigacoes())
            {
                return servico.ObtenhaLigacoes(filtro);
            }
        }
    }
}
