using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MSTypers.Models;
using MySqlConnector;
using MSTypers.Utilidades;
using Newtonsoft.Json.Linq;

namespace MSTypers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TyperController : ControllerBase
    {
        private readonly ILogger<TyperController> _logger;
        private TypersContext baseDeDatos;
        private int NUM_CORREOS_MAXIMO = 2;

        public TyperController(ILogger<TyperController> logger)
        {
            _logger = logger;
            baseDeDatos = new TypersContext();
        }

        /// <summary>
        /// Método con el cual se realiza el registro de un nuevo Typer que cuenta con toda la información inicial para utilizar la aplicación
        /// </summary>
        /// <param name="nuevoTyper">Objeto que cuenta con la información necesaria para registrar un Typer</param>
        /// <returns>Retorna un valor booleano para identificar el exito del registro junto con un mensaje</returns>
        [HttpPost("registrarTyper")]
        public async Task<ActionResult<JObject>> RegistrarNuevoTyper([FromBody]Typer nuevoTyper = null )
        {
            JObject resultadoAEnviar;
            if(nuevoTyper == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);

            }else{
                Typer usuario = null;
                
                usuario = await baseDeDatos.Typers
                .Where(registro => registro.Username.Equals(nuevoTyper.Username))
                .SingleOrDefaultAsync(); 

                if (usuario != null)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El nombre de usuario ya existe");
                    return BadRequest(resultadoAEnviar);
                }

            }

            nuevoTyper.IdTyper = Guid.NewGuid().ToString();
            nuevoTyper.Contrasenia.Single().Contrasenia1 = Encrypt.GetSHA256(nuevoTyper.Contrasenia.Single().Contrasenia1);
            try
            {
                baseDeDatos.Entry(nuevoTyper).State = EntityState.Added;
                baseDeDatos.AddRange(nuevoTyper.Contrasenia);
                baseDeDatos.AddRange(nuevoTyper.Correos);
                await baseDeDatos.SaveChangesAsync();

                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("El usuario se registro correctamente",nuevoTyper);
                return Ok(resultadoAEnviar);
            }
            catch (Exception ex)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(ex.Message);
                return BadRequest(resultadoAEnviar);
            }
        }

        /// <summary>
        /// Método con el cual se realiza la eliminación logica de un usuario dentro de la aplicación
        /// </summary>
        /// <param name="idTyper">Identificador unico del Typer a eliminar</param>
        /// <returns>Valor booleano identificando el exito de la operación junto con un mensaje</returns>
        [HttpDelete("eliminarTyper")]
        public async Task<ActionResult<JObject>> EliminarTyper([FromBody]InformacionDePeticiones peticion = null)
        {
            JObject resultadoAEnviar;
            Typer usuario = null;

            if(peticion == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);

            }else{
                try
                {
                    usuario = await baseDeDatos.Typers
                    .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper))
                    .SingleOrDefaultAsync();                
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                catch (MySqlException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }

                if (usuario == null)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro el usuario buscado");
                    return BadRequest(resultadoAEnviar);
                }else{
                    usuario.Estatus = 0;
                    baseDeDatos.Entry(usuario).State = EntityState.Modified;
                    await baseDeDatos.SaveChangesAsync();
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("El usuario se elimino correctamente", usuario);
                }

                return Ok(resultadoAEnviar);
            }

                
        }
        
        /// <summary>
        /// Método para realizar login verificando el usuario y la contraseña del usuario
        /// </summary>
        /// <param name="username">Uusario unico del Typer</param>
        /// <param name="contrasenia">Contraseña ingresada por el usuario</param>
        /// <returns>Mensaje de exito en caso de que los campos sean correctos junto con un mensaje</returns>
        [HttpPost("loginTyper")]
        public async Task<ActionResult<JObject>> Login([FromBody]InformacionDePeticiones peticion = null)
        {
            Typer usuario = null;
            Contrasenia password =null;
            JObject resultadoAEnviar;

            if(peticion == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);
            }else{
                try
                {
                    usuario = await baseDeDatos.Typers
                    .Where(registro => registro.Username.Equals(peticion.IdentificadorTyper))
                    .SingleOrDefaultAsync();

                    if (usuario == null)
                    {
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El usuario no existe");
                        return BadRequest(resultadoAEnviar);
                    }else if(usuario.Estatus == 0){
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El usuario fue elimidado");
                        return BadRequest(resultadoAEnviar);
                    }

                    password = await baseDeDatos.Contrasenias
                    .Where(registro => registro.IdTyper.Contains(usuario.IdTyper))
                    .SingleOrDefaultAsync();

                    usuario.Correos = await baseDeDatos.Correos
                    .Where(registro => registro.IdTyper.Equals(usuario.IdTyper))
                    .ToListAsync(); 
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                catch (MySqlException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }

                if (password.Contrasenia1.Equals(Encrypt.GetSHA256(peticion.InformacionComplementaria)))
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Inicio de sesión exitoso", usuario);
                    return Ok(resultadoAEnviar);
                }else
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Contraseña incorrecta");
                    return BadRequest(resultadoAEnviar);
                }
            }
        }

        /// <summary>
        /// Método para obtener la información basica de un Typper basado en su nombre de usuario
        /// </summary>
        /// <param name="username">Usuario unico del Typer</param>
        /// <returns>Información basica del Typer buscado</returns>
        [HttpPost("infoTyper")]
        public async Task<ActionResult<JObject>> ObtenerInfoTyper([FromBody]InformacionDePeticiones peticion = null)
        {
            JObject resultadoAEnviar;
            Typer usuario = null;
            
            try
            {
                switch (peticion.ModificadorDeMetodo)
                {
                    case "usuario":
                        usuario = await baseDeDatos.Typers
                        .Where(registro => registro.Username.Equals(peticion.IdentificadorTyper)).SingleOrDefaultAsync();
                    break;

                    case "id":
                        usuario = await baseDeDatos.Typers
                        .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper)).SingleOrDefaultAsync();
                    break;
                    
                    default:
                        usuario = null;
                    break;
                }
                
                if (usuario == null)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro el usuario buscado");
                    return BadRequest(resultadoAEnviar);
                }

                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Typer encontrado", usuario);
                return Ok(resultadoAEnviar); 
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(resultadoAEnviar);
            }
            catch (MySqlException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(resultadoAEnviar);
            }
        }


        /// <summary>
        /// Método para realizar actualizaciones sobre el perfil de un Typer, con el argumento "operacion" se especifica el tipo de actualización que se desea realizar
        /// 1.Username
        /// 2.Estado
        /// 3. Foto de perfil
        /// </summary>
        /// <param name="operacion">Tipo de actualización a realizar</param>
        /// <param name="idTyper">Identificador unico del Typer a eliminar</param>
        /// <param name="nuevaInfo">Nueva información que será registrada en la base de datos</param>
        /// <returns>Valor booleano para identificar el exito de la actualización de información junto con un mensaje</returns>
        [HttpPut("actualizarInfoTyper")]
        public async Task<ActionResult<JObject>> ActualizarInfoDeUsuario([FromBody]InformacionDePeticiones peticion = null)
        {
            Typer usuario = null;
            JObject resultadoAEnviar;

            if(peticion == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);

            }else{
                try
                {
                    usuario = await baseDeDatos.Typers
                    .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper))
                    .Include("Correos")
                    .SingleOrDefaultAsync();    

                    if (usuario == null)
                    {
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro el usuario buscado");
                        return BadRequest(resultadoAEnviar);
                    }            
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                catch (MySqlException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }

                switch (peticion.ModificadorDeMetodo)
                {
                    case "usuario":
                        Typer usuarioExistente = null;
                        
                        usuarioExistente = await baseDeDatos.Typers
                        .Where(registro => registro.Username.Equals(peticion.InformacionActualizada))
                        .SingleOrDefaultAsync(); 

                        if (usuarioExistente != null)
                        {
                            resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El nombre de usuario ya existe");
                            return BadRequest(resultadoAEnviar);
                        }
                        
                        usuario.Username = peticion.InformacionActualizada;
                        baseDeDatos.Entry(usuario).State = EntityState.Modified;
                        await baseDeDatos.SaveChangesAsync();
                    break;

                    case "estado":
                        usuario.Estado =  peticion.InformacionActualizada;
                        baseDeDatos.Entry(usuario).State = EntityState.Modified;
                        await baseDeDatos.SaveChangesAsync();
                    break;

                    case "fotoDePerfil":
                        usuario.FotoDePerfil = peticion.InformacionActualizada;
                        baseDeDatos.Entry(usuario).State = EntityState.Modified;
                        await baseDeDatos.SaveChangesAsync();
                    break;

                    default:
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El metodo seleccionado no es valido");
                        return BadRequest(resultadoAEnviar);
                }

                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Actualización realizada correctamente",usuario);
                return Ok(resultadoAEnviar); 
            }

        }

        /// <summary>
        /// Método para agregar un correo alterno al perfil del Typer
        /// </summary>
        /// <param name="nuevoCorreo">Objeto que contiene la información del nuevo correo a agregar</param>
        /// <returns>Valor booleano para identificar el exito de la agregación del correo junto con un mensaje </returns>
        [HttpPost("agregarCorreo")]
        public async Task<ActionResult<JObject>> AgregarNuevoCorreo([FromBody]Correo nuevoCorreo)
        {
            List<Correo> correosDeUsuario = null;
            JObject resultadoAEnviar;
            try
            {
                correosDeUsuario = await baseDeDatos.Correos
                .Where(registro => registro.IdTyper.Equals(nuevoCorreo.IdTyper))
                .ToListAsync();

                if (correosDeUsuario.Count == 0)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro con el usuario");
                    return BadRequest(resultadoAEnviar);
                }else if (correosDeUsuario.Count == NUM_CORREOS_MAXIMO)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El usuario ya cuenta con dos correos registrados");
                    return BadRequest(resultadoAEnviar);
                }else if(nuevoCorreo.EsPrincipal == 1)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se puede tener dos correos principales");
                    return BadRequest(resultadoAEnviar);
                }

                baseDeDatos.Entry(nuevoCorreo).State = EntityState.Added;
                await baseDeDatos.SaveChangesAsync();
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(resultadoAEnviar);
            }
            catch (MySqlException e)
            {
                _logger.LogError("Sucedió una excepción:\n" + e.Message);
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                return BadRequest(resultadoAEnviar);
            }

            resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Correo agregado correctamente",nuevoCorreo);
            return Ok(resultadoAEnviar);    
        }

        /// <summary>
        /// Método para actualizar un correo del Typer
        /// </summary>
        /// <param name="idTyper">Identificador unico del Typer</param>
        /// <param name="correoActual">Nombre del correo a actualizar</param>
        /// <param name="nuevoCorreo">Correo nuevo del Typer que sera reemplazo del anterior</param>
        /// <returns>Valor booleano para identificar el exito de la actualización del correo junto con un mensaje</returns>
        [HttpPut("actualizarCorreo")]
        public async Task<ActionResult<JObject>> ActualizarCorreo([FromBody]InformacionDePeticiones peticion = null)
        {
            JObject resultadoAEnviar;
            Correo correoAActualizar = null;
            
            if(peticion == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);

            }else{
                try
                {
                    List<Correo> correosDeUsuario = await baseDeDatos.Correos
                    .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper))
                    .ToListAsync();

                    if (correosDeUsuario.Count == 0)
                    {
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro el usuario especificado");
                        return BadRequest(resultadoAEnviar);    
                    }

                    foreach (Correo correo in correosDeUsuario)
                    {
                        if (correo.Direccion.Equals(peticion.InformacionComplementaria))
                        {
                            correoAActualizar = correo;
                        }        
                    }

                    if (correoAActualizar != null)
                    {
                        correoAActualizar.Direccion = peticion.InformacionActualizada;
                        baseDeDatos.Entry(correoAActualizar).State = EntityState.Modified;
                        await baseDeDatos.SaveChangesAsync();
                    }else{
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro el correo a actualizar");
                        return BadRequest(resultadoAEnviar);    
                    }
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                catch (MySqlException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                
                Typer usuario = await baseDeDatos.Typers
                        .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper)).SingleOrDefaultAsync();

                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Correo actualizado correctamente", usuario);
                return Ok(resultadoAEnviar);    
            }
        }

        /// <summary>
        /// Método que obtiene todos los correos vinculados al Typer especificado
        /// </summary>
        /// <param name="idTyper">Identificador unico del Typer</param>
        /// <returns>Mensaje de información junto con el listado de los correos pertenecientes al Typer</returns>
        [HttpPost("obtenerCorreos")]
        public async Task<ActionResult<JObject>> ObtenerCorreosTyper([FromBody]InformacionDePeticiones peticion = null)
        {
            List<Correo> correos = null;
            JObject resultadoAEnviar;

            if(peticion == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);
            }else{
                
                try
                {
                    correos = await baseDeDatos.Correos
                    .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper))
                    .ToListAsync(); 

                    if (correos.Count == 0)
                    {
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El usuario no se encontro");
                        return BadRequest(resultadoAEnviar);
                    }
                
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                catch (MySqlException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }

                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Lista de correos encontrada", correos);
                return Ok(resultadoAEnviar);    
            }
        }

        /// <summary>
        /// Método que actualiza la contraseña del Typer especificado por una nueva
        /// </summary>
        /// <param name="idTyper">Identificador unico del Typer</param>
        /// <param name="nuevaContrasenia">Nueva contraseña del Typer</param>
        /// <returns>Valor booleano para identificar el exito de la actualización junto con un mensaje</returns>
        [HttpPut("actualizarContrasenia")]
        public async Task<ActionResult<JObject>> ActualizarContraseniaTyper([FromBody]InformacionDePeticiones peticion = null)
        {
            JObject resultadoAEnviar;
            Contrasenia contraseniaActual;

            if(peticion == null)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);
            }else{

                try
                {
                    contraseniaActual = await baseDeDatos.Contrasenias
                    .Where(registro => registro.IdTyper.Equals(peticion.IdentificadorTyper))
                    .SingleOrDefaultAsync();

                    if (contraseniaActual == null)
                    {
                        resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("El usuario no se encontro");
                        return BadRequest(resultadoAEnviar);
                    }

                    contraseniaActual.Contrasenia1 = Encrypt.GetSHA256(peticion.InformacionActualizada);

                    baseDeDatos.Entry(contraseniaActual).State = EntityState.Modified;
                    await baseDeDatos.SaveChangesAsync();
                }
                catch (ArgumentNullException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }
                catch (MySqlException e)
                {
                    _logger.LogError("Sucedió una excepción:\n" + e.Message);
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(e.Message);
                    return BadRequest(resultadoAEnviar);
                }

                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("Contraseña actualizada correctamente", contraseniaActual);
                return Ok(resultadoAEnviar);    
            }
        }
    }
}
