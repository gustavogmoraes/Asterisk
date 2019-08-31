using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaSolucao.Negocio.DTOs;
using MegaSolucao.Negocio.Objetos;
using MegaSolucao.Negocio.Servicos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MegaSolucao.Controllers
{
    [Route("api/[controller]")]
    [EnableCors]
    public class LigacoesController : Controller
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
