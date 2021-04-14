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
    [Produces("application/json")]
    public class GruposController : ControllerBase
    {
        private readonly MensajesContext _mensajesContext;
        private readonly ILogger<GruposController> _logger;
        private const int VALOR_MINIMO_ID = 1;
        private const int NINGUN_CAMBIO_REALIZADO = 0;
        private JObject _resultado;

        public GruposController(ILogger<GruposController> logger)
        {
            _logger = logger;
            _mensajesContext = new MensajesContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpGet("buscar")]
        public async Task<ActionResult<JObject>> ObtenerGrupos([FromQuery]string nombre="", [FromQuery]int idGrupo = -1)
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
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(_resultado);
            }
            catch (MySqlException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(_resultado);
            }

            if (grupos == null || grupos.Count == 0)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Grupo(s) no encontrado(s) o inexistente(s)");
                return BadRequest(_resultado);
            }

            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Grupos encontrados", grupos);

            return Ok(_resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpGet("integrantes/{idGrupo}")]
        public async Task<ActionResult<JObject>> ObtenerIntegrantesDeGrupo(int idGrupo)
        {
            List<Pertenece> integrantesDeGrupo = null;

            if (idGrupo <= VALOR_MINIMO_ID)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Id de grupo incorrecto.");
                return BadRequest(_resultado);
            }

            try
            {
                integrantesDeGrupo = await _mensajesContext.Perteneces
                    .Where(pertenece => pertenece.IdGrupo == idGrupo)
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

            int noHayIntegrantes = 0;
            if (integrantesDeGrupo == null || integrantesDeGrupo.Count == noHayIntegrantes)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No existe grupo con el id especificado.");
                return BadRequest(_resultado);
            }

            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Integrantes encontrados", integrantesDeGrupo);
            return Ok(_resultado);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuevoGrupo"></param>
        /// <returns></returns>
        [HttpPost("crearGrupo")]
        public async Task<ActionResult<JObject>> CrearGrupo([FromBody] Grupo nuevoGrupo)
        {
            if (nuevoGrupo == null)
            {
                _logger.LogError("El grupo no tiene los datos necesarios");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("El grupo no tiene los datos necesarios");
                return BadRequest(_resultado);
            }

            nuevoGrupo.FechaCreacion = DateTime.Now;
            _mensajesContext.Entry(nuevoGrupo).State = EntityState.Added;
            _mensajesContext.AddRange(nuevoGrupo.Perteneces);

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
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Grupo no creado");
                return BadRequest(_resultado);
            }

            _logger.LogInformation("Nuevo grupo creado: {0}", nuevoGrupo.Nombre);
            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Nuevo grupo creado", nuevoGrupo);
            return Ok(_resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo">Id del grupo a actualizar.</param>
        /// <param name="grupoActualizado">La nueva información de un grupo. Esta información solo puede ser Nombre y/o Descripción.</param>
        /// <returns></returns>
        [HttpPut("actualizar/{idGrupo}")]
        public async Task<ActionResult<JObject>> ActualizarGrupo(int idGrupo = 0, [FromBody] Grupo grupoActualizado = null)
        {
            if (grupoActualizado == null)
            {
                _logger.LogError("El grupo no tiene los datos necesarios para actualizarse");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("El grupo no tiene los datos necesarios para actualizarse");
                return BadRequest(_resultado);
            }

            if (idGrupo < VALOR_MINIMO_ID)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Id del grupo incorrecto o no especificado");
                return BadRequest(_resultado);
            }

            Grupo grupoAActualizar = _mensajesContext.Grupos.SingleOrDefault(grupo => grupo.IdGrupo == idGrupo);

            if (grupoAActualizar == null)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Grupo no encontrado");
                return BadRequest(_resultado);
            }

            grupoAActualizar.Nombre = grupoActualizado.Nombre;
            grupoAActualizar.Descripcion = grupoActualizado.Descripcion;

            _mensajesContext.Entry(grupoAActualizar).State = EntityState.Modified;

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
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No se pudo actualizar el grupo");
                return BadRequest(_resultado);
            }

            _logger.LogInformation("Grupo actualizado: {0}", grupoAActualizar.Nombre);
            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Grupo actualizado", grupoAActualizar);
            return Ok(_resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="integrantes"></param>
        /// <returns></returns>
        [HttpPost("agregarIntegrantes/{idGrupo}")]
        public async Task<ActionResult<JObject>> AgregarIntegrantesAGrupo(int idGrupo, [FromBody] List<Pertenece> integrantes)
        {
            if (integrantes == null)
            {
                _logger.LogError("No se especificaron el/los integrantes");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No se especificaron el/los integrantes");
                return BadRequest(_resultado);
            }

            if (idGrupo < VALOR_MINIMO_ID)
            {
                _logger.LogError("Id del grupo incorrecto o no especificado");
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Id del grupo incorrecto o no especificado");
                return BadRequest(_resultado);
            }

            bool existeGrupo = _mensajesContext.Grupos.Any(grupo => grupo.IdGrupo == idGrupo);

            if (!existeGrupo)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No existe el grupo con el id especificado.");
                return BadRequest(_resultado);
            }

            _mensajesContext.Perteneces.AddRange(integrantes);

            int resultado;

            try
            {
                resultado = await _mensajesContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is MySqlException mysqlException && (mysqlException.Number == 1062))
            {
                _logger.LogError("Sucedio una excepcion:\n" + mysqlException.Message);
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Ya existe un Typer en el grupo con el mismo id");
                return BadRequest(_resultado);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError("Sucedio una excepcion:\n" + e.InnerException.Message);
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.InnerException.Message);
                return BadRequest(_resultado);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError("Sucedio una excepcion:\n" + e.Message);
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(_resultado);
            }


            if (resultado == NINGUN_CAMBIO_REALIZADO)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No se pudo agregar el nuevo integrante");
                return BadRequest(_resultado);
            }

            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Integrante(s) agregado(s)", integrantes);
            return Ok(_resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="idTyper"></param>
        /// <returns></returns>
        [HttpPut("salir")]
        public async Task<ActionResult<bool>> SalirDeGrupo([FromQuery] int idGrupo = -1, [FromQuery] string idTyper = "")
        {
            if (idGrupo < VALOR_MINIMO_ID)
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Id de grupo no especificado o incorrecto");
                return BadRequest(_resultado);
            }

            if (idTyper.Equals(""))
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("Id de typer no especificado o incorrecto");
                return BadRequest(_resultado);
            }

            Pertenece perteneceAEliminar = _mensajesContext.Perteneces.SingleOrDefault(pertenece => 
                pertenece.IdGrupo == idGrupo && pertenece.IdTyper.Equals(idTyper)
            );

            if (perteneceAEliminar != null)
            {
                _mensajesContext.Perteneces.Remove(perteneceAEliminar);

                int resultado;

                try
                {
                    resultado = await _mensajesContext.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    _logger.LogError("Sucedio una excepcion:\n" + e.InnerException.Message);
                    _resultado = ConvertidorDeJson.ConvertirResultadoFallido(e.InnerException.Message);
                    return BadRequest(_resultado);
                }

                if (resultado == NINGUN_CAMBIO_REALIZADO)
                {
                    _resultado = ConvertidorDeJson.ConvertirResultadoFallido("No se pudo eliminar el typer del grupo");
                    return BadRequest(_resultado);
                }
            }
            else
            {
                _resultado = ConvertidorDeJson.ConvertirResultadoFallido("El id del grupo y/o id del Typer no está registrado");
                return BadRequest(_resultado);
            }

            _resultado = ConvertidorDeJson.ConvertirResultadoExitoso("Typer eliminado del grupo", perteneceAEliminar);
            return Ok(_resultado);
        }
    }
}