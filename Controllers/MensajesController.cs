using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSMensajes.Models;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using MSMensajes.Converters;

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
        private JObject _resultado;

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
        public async Task<ActionResult<JObject>> EnviarMensaje([FromBody] Mensaje nuevoMensaje)
        {
            if (nuevoMensaje == null)
            {
                _logger.LogError("Mensaje no especificado");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Mensaje no especificado");
                return BadRequest(_resultado);
            }

            bool existeGrupo = _mensajesContext.Grupos.Any(grupo => grupo.IdGrupo == nuevoMensaje.IdGrupo);

            if (!existeGrupo)
            {
                _logger.LogError("No existe un grupo con el id especificado");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No existe un grupo con el id especificado");
                return BadRequest(_resultado);
            }

            bool esTyperIntegranteDelGrupo = _mensajesContext.Perteneces.Any(typer =>
                typer.IdGrupo == nuevoMensaje.IdGrupo && typer.IdTyper.Equals(nuevoMensaje.IdTyper)
            );

            if (!esTyperIntegranteDelGrupo)
            {
                _logger.LogError("El typer no pertenece al grupo");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("El typer no pertenece al grupo");
                return BadRequest(_resultado);
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
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(_resultado);
            }

            if (resultado == NINGUN_CAMBIO_REALIZADO)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No se pudo enviar el mensaje");
                return BadRequest(_resultado);
            }


            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Mensaje enviado", nuevoMensaje);
            return Ok(_resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpGet("obtener/{idGrupo}")]
        public async Task<ActionResult<JObject>> ObtenerMensajesDeGrupo(int idGrupo)
        {
            if (idGrupo < VALOR_MINIMO_ID)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Id del grupo incorrecto");
                return BadRequest(_resultado);
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
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(_resultado);
            }
            catch (MySqlException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(_resultado);
            }

            int noHayMensajes = 0;

            if (mensajes == null || mensajes.Count == noHayMensajes)
            {
                _logger.LogError("No hay mensajes");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No hay mensajes en el grupo");
                return BadRequest(_resultado);
            }

            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Se encontraron mensajes", mensajes);
            return Ok(_resultado);
        }
    }
}
