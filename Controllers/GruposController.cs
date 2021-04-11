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
using System.Text.Json;


namespace MSMensajes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly MensajesContext _mensajesContext;
        private readonly ILogger<GruposController> _logger;
        private const int VALOR_MINIMO_ID = 1;
        private const int NINGUN_CAMBIO_REALIZADO = 0;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        [HttpGet("integrantes/{idGrupo}")]
        public async Task<ActionResult<Grupo>> ObtenerIntegrantesDeGrupo(int idGrupo)
        {
            List<Pertenece> integrantesDeGrupo = null;

            if (idGrupo >= VALOR_MINIMO_ID)
            {
                try
                {
                    integrantesDeGrupo = await _mensajesContext.Perteneces
                        .Where(pertenece => pertenece.IdGrupo == idGrupo)
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

                int noHayIntegrantes = 0;
                if (integrantesDeGrupo == null || integrantesDeGrupo.Count == noHayIntegrantes)
                {
                    return BadRequest("No existe grupo con el id especificado.");
                }

                return Ok(integrantesDeGrupo);
            }

            return BadRequest("Id de grupo incorrecto.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuevoGrupo"></param>
        /// <returns></returns>
        [HttpPost("crearGrupo")]
        public async Task<ActionResult<bool>> CrearGrupo([FromBody] Grupo nuevoGrupo)
        {
            if (nuevoGrupo == null)
            {
                _logger.LogError("El grupo no tiene los datos necesarios");
                return BadRequest("El grupo no tiene los datos necesarios");
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
                return BadRequest(e);
            }

            if (resultado == NINGUN_CAMBIO_REALIZADO)
            {
                return BadRequest(false);
            }

            _logger.LogInformation("Nuevo grupo creado: {0}", nuevoGrupo.Nombre);
            return Ok(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo">Id del grupo a actualizar.</param>
        /// <param name="grupoActualizado">La nueva información de un grupo. Esta información solo puede ser Nombre y/o Descripción.</param>
        /// <returns></returns>
        [HttpPut("actualizar/{idGrupo}")]
        public async Task<ActionResult<bool>> ActualizarGrupo(int idGrupo = 0, [FromBody] Grupo grupoActualizado = null)
        {
            if (grupoActualizado == null)
            {
                _logger.LogError("El grupo no tiene los datos necesarios para actualizarse");
                return BadRequest("El grupo no tiene los datos necesarios para actualizarse");
            }

            if (idGrupo < VALOR_MINIMO_ID)
            {
                return BadRequest("Id del grupo incorrecto o no especificado");
            }

            Grupo grupoAActualizar = _mensajesContext.Grupos.SingleOrDefault(grupo => grupo.IdGrupo == idGrupo);

            if (grupoAActualizar == null)
            {
                return BadRequest("Grupo no encontrado");
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
                return BadRequest(e);
            }

            if (resultado == NINGUN_CAMBIO_REALIZADO)
            {
                return BadRequest(false);
            }

            _logger.LogInformation("Grupo actualizado: {0}", grupoActualizado.Nombre);
            return Ok(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idGrupo"></param>
        /// <param name="integrantes"></param>
        /// <returns></returns>
        [HttpPost("agregarIntegrantes/{idGrupo}")]
        public async Task<ActionResult<bool>> AgregarIntegrantesAGrupo(int idGrupo, [FromBody] List<Pertenece> integrantes)
        {
            if (integrantes == null)
            {
                _logger.LogError("No se especificaron el/los integrantes");
                return BadRequest("No se especificaron el/los integrantes");
            }

            if (idGrupo < VALOR_MINIMO_ID)
            {
                _logger.LogError("Id del grupo incorrecto o no especificado");
                return BadRequest("Id del grupo incorrecto o no especificado");
            }

            bool existeGrupo = _mensajesContext.Grupos.Any(grupo => grupo.IdGrupo == idGrupo);

            if (existeGrupo)
            {
                _mensajesContext.Perteneces.AddRange(integrantes);

                int resultado;

                try
                {
                    resultado = await _mensajesContext.SaveChangesAsync();
                }
                catch (DbUpdateException e) when (e.InnerException is MySqlException mysqlException && (mysqlException.Number == 1062))
                {
                    _logger.LogError("Sucedio una excepcion:\n" + mysqlException.Message);
                    return BadRequest("Ya existe un Typer en el grupo con el mismo id");
                }
                catch (DbUpdateException e)
                {
                    _logger.LogError("Sucedio una excepcion:\n" + e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogError("Sucedio una excepcion:\n" + e.Message);
                    return BadRequest(e.Message);
                }
                

                if (resultado == NINGUN_CAMBIO_REALIZADO)
                {
                    return BadRequest(false);
                }

                return Ok(true);
            }

            return BadRequest("No existe el grupo con el id especificado.");
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
                return BadRequest("Id de grupo no especificado o incorrecto");
            }

            if (idTyper.Equals(""))
            {
                return BadRequest("Id de typer no especificado o incorrecto");
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
                    return BadRequest(e.InnerException.Message);
                }

                if (resultado == NINGUN_CAMBIO_REALIZADO)
                {
                    return BadRequest(false);
                }
            }
            else
            {
                return BadRequest("El id del grupo y/o id del Typer no está registrado");
            }

            return Ok(true);
        }
    }
}