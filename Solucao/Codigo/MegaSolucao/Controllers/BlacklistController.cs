using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaSolucao.Controllers.Base;
using MegaSolucao.Negocio.DTOs;
using MegaSolucao.Negocio.Objetos;
using MegaSolucao.Persistencia.BancoDeDados.Raven;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MegaSolucao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlacklistController : ControllerPadrao<ItemBlacklist>
    {
        
    }
}