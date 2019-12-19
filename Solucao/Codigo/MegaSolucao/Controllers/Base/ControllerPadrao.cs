using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaSolucao.Negocio.Servicos.Base;
using Microsoft.AspNetCore.Mvc;

namespace MegaSolucao.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerPadrao<T> : ControllerBase
        where T: class, new()
    {
        [HttpGet("{id}")]
        public ActionResult<T> Consulte(string id)
        {
            using (var servico = FabricaDeServicos.CriePadrao<T>())
            {
                return servico.Consulte(id);
            }
        }

        [HttpGet]
        public ActionResult<List<T>> ConsulteLista()
        {
            using (var servico = FabricaDeServicos.CriePadrao<T>())
            {
                return servico.ConsulteLista();
            }
        }

        [HttpPost]
        public ActionResult Insira([FromBody] T objeto)
        {
            using (var servico = FabricaDeServicos.CriePadrao<T>())
            {
                servico.Insira(objeto);

                return Ok();
            }
        }

        [HttpPut]
        public ActionResult Atualize([FromBody] T objeto)
        {
            using (var servico = FabricaDeServicos.CriePadrao<T>())
            {
                servico.Atualize(objeto);

                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Exclua(string id)
        {
            using (var servico = FabricaDeServicos.CriePadrao<T>())
            {
                servico.Exclua(id);

                return Ok();
            }
        }
    }
}
