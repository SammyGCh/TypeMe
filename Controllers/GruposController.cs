using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSMensajes.Models;
using MySqlConnector;

namespace MSMensajes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly MensajesContext _mensajesContext;
        private readonly ILogger<GruposController> _logger;

        public GruposController(ILogger<GruposController> logger)
        {
            _logger = logger;
            _mensajesContext = new MensajesContext();
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<Grupo>> ObtenerGrupos([FromQuery]string nombre="", [FromQuery]int idGrupo = -1)
        {
            List<Grupo> grupos = null;

            try
            {
                grupos = await _mensajesContext.Grupos
                    .Where(grupo => grupo.Nombre.Contains(nombre))
                    .Where(grupo => (idGrupo >= 0 && grupo.IdGrupo == idGrupo) ||
                                    (idGrupo < 0 && grupo.IdGrupo != idGrupo))
                    .ToListAsync();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                return BadRequest(e.Message);
            }
            catch (MySqlException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                return BadRequest(e.Message);
            }

            if (grupos == null)
            {
                return BadRequest();
            }
            
            return Ok(grupos);
        }
    }
}