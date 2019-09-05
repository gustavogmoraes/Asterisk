using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
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

        [HttpGet("[action]/{uniqueId}")]
        public FileResult ObtenhaGravacao(string uniqueId)
        {
            var result = Servico.ObtenhaGravacao(uniqueId, out var nomeDoArquivo);

            return File(result, "audio/x-wav", nomeDoArquivo);
        }

        [HttpPost("[action]")]
        public FileResult ObtenhaListaDeGravacoes([FromBody]string[] ids)
        {
            var result = Servico.ObtenhaListaDeGravacoes(ids, out var nomeDoArquivo);

            return File(result, MediaTypeNames.Application.Zip, nomeDoArquivo);
        }
    }
}
