using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using MegaSolucao.Infraestrutura;
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
            using (var servico = new ServicoDeLigacoes())
            {
                return Ok(servico.ObtenhaLigacoes(filtro));
            }
        }

        [HttpGet("[action]/{idsStringified}")]
        public FileResult BaixeGravacoes(string idsStringified)
        {
            using (var servico = new ServicoDeLigacoes())
            {
                var ids = idsStringified.Split('|');

                return ids.Length == 1
                    ? File(servico.ObtenhaGravacao(ids.FirstOrDefault(), out var nomeDoArquivo), "audio/x-wav", nomeDoArquivo)
                    : File(servico.ObtenhaListaDeGravacoes(ids, out var nomeArquivo), MediaTypeNames.Application.Zip, nomeArquivo);
            }
        }

        [HttpGet("[action]/{uniqueId}")]
        public FileStreamResult TransmitaGravacao(string uniqueId)
        {
            using (var servico = new ServicoDeLigacoes())
            {
                Response.Headers.Add("Content-Disposition", "inline; filename=\"" + $"{uniqueId}.wav" + "\"");
                return File(servico.ObtenhaGravacaoParaPlayer(uniqueId), "audio/x-wav", true);
            }
        }
    }
}
