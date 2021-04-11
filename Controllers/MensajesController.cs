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
    public class MensajesController : ControllerBase
    {
        private readonly MensajesContext _mensajesContext;
        private readonly ILogger<MensajesController> _logger;
        private const int VALOR_MINIMO_ID = 1;
        private const int NINGUN_CAMBIO_REALIZADO = 0;

        public MensajesController(ILogger<MensajesController> logger)
        {
            _logger = logger;
            _mensajesContext = new MensajesContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuevoMensaje"></param>
        /// <returns></returns>
        [HttpPost("enviar")]
        public async Task<ActionResult<bool>> EnviarMensaje([FromBody] Mensaje nuevoMensaje)
        {
            if (nuevoMensaje == null)
            {
                _logger.LogError("Mensaje no especificado");
                return BadRequest("Mensaje no especificado");
            }

            bool existeGrupo = _mensajesContext.Grupos.Any(grupo => grupo.IdGrupo == nuevoMensaje.IdGrupo);

            if (!existeGrupo)
            {
                _logger.LogError("No existe un grupo con el id especificado");
                return BadRequest("No existe un grupo con el id especificado");
            }

            nuevoMensaje.Fecha = DateTime.Now;
            string horaActual = DateTime.Now.ToString("HH:mm:ss");
            nuevoMensaje.Hora = TimeSpan.Parse(horaActual);
            _mensajesContext.Entry(nuevoMensaje).State = EntityState.Added;

            int resultado;

            try
            {
                resultado = await _mensajesContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("Sucedio una excepcion:\n" + e.Message);
                return BadRequest(e);
            }

            if (resultado == NINGUN_CAMBIO_REALIZADO)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpGet("obtener/{idGrupo}")]
        public async Task<ActionResult<Mensaje>> ObtenerMensajesDeGrupo(int idGrupo)
        {
            if (idGrupo < VALOR_MINIMO_ID)
            {
                return BadRequest("Id del grupo incorrecto ");
            }

            List<Mensaje> mensajes = null;

            try
            {
                mensajes = await _mensajesContext.Mensajes
                    .Where(mensaje => mensaje.IdGrupo == idGrupo)
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

            int noHayMensajes = 0;

            if (mensajes == null || mensajes.Count == noHayMensajes)
            {
                _logger.LogError("No hay mensajes");
                return BadRequest("No hay mensajes en el grupo");
            }

            return Ok(mensajes);
        }
    }
}
