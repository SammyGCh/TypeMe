using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using MSMultimedia.Models;
using MSMultimedia.Utilidades;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MSMultimedia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ImagenController : ControllerBase
    {
        private readonly ILogger<ImagenController> _logger;
        private MultimediasContext baseDeDatos;
        private const int TIPO_MULTIMEDIA_FOTO = 1;

        public ImagenController(ILogger<ImagenController> logger)
        {
            _logger = logger;
            baseDeDatos = new MultimediasContext();
        }

        [HttpPost("registrarImagen")]
        public async Task<ActionResult<JObject>> RegistrarMultimedia([FromForm]FileUploadAPI objFile, [FromQuery]string idTyper = "")
        {
            JObject resultadoAEnviar;
            Multimedia nuevaMultimedia = new Multimedia();
            string directorioDeSalida = Directory.GetCurrentDirectory() + "/imagenes/";

            if(objFile.file.Length <= 0)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("Los datos estan incompletos");
                return BadRequest(resultadoAEnviar);
            }

            try
            {
                if (!Directory.Exists(directorioDeSalida))
                {
                    Directory.CreateDirectory(directorioDeSalida);
                }

                using(FileStream fileStream = System.IO.File.Create(directorioDeSalida + idTyper + objFile.file.FileName)){
                    objFile.file.CopyTo(fileStream);
                    fileStream.Flush();    
                }

                nuevaMultimedia.Ruta = directorioDeSalida + objFile.file.FileName;
                nuevaMultimedia.IdMultimedia = Guid.NewGuid().ToString();
                nuevaMultimedia.IdTipoMultimedia = TIPO_MULTIMEDIA_FOTO;
          
                baseDeDatos.Entry(nuevaMultimedia).State = EntityState.Added;
                await baseDeDatos.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(ex.InnerException.Message);
                return BadRequest(resultadoAEnviar);
            }

            resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoExitoso("El archivo multimedia se registro correctamente", nuevaMultimedia);
            return Ok(resultadoAEnviar);
        }


        [HttpGet("obtenerImagen")]
        public async Task<ActionResult<JObject>> ObtenerMultimedia([FromQuery]string idImagen = "")
        {
            JObject resultadoAEnviar;
            Multimedia multimedia = null;

            try
            {
                multimedia = await  baseDeDatos.Multimedias
                .Where(registro => registro.IdMultimedia.Equals(idImagen)).SingleOrDefaultAsync();
                
                if (multimedia == null)
                {
                    resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido("No se encontro el archivo buscado");
                    return BadRequest(resultadoAEnviar);
                }

                string direccionDeImagen = multimedia.Ruta;
                var bytes = await System.IO.File.ReadAllBytesAsync(direccionDeImagen);
                    
                return File(bytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                resultadoAEnviar = ConvertidorDeJson.ConvertirResultadoFallido(ex.Message);
                return BadRequest(resultadoAEnviar);
            }
        }

        public class FileUploadAPI{
            public IFormFile file { get; set; }
        }
    }
}
