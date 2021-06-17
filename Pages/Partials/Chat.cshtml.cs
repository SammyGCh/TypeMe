using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TypeMeWeb.Models;
using System.Text.Json;
using System.Text;
using System.IO;

namespace TypeMeWeb.Pages.Partials
{
    public class ChatModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private static string URL_API = Environment.GetEnvironmentVariable("URL_API");
        private WebClient webClient;
        [ViewData]
        public Grupo Grupo { get; set; }
        [ViewData]
        public List<MensajeDominio> MisMensajes { get; set; }
        [ViewData]
        public Typer TyperEnSesion { get; set; }

        public ChatModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
            webClient = new WebClient();
        }

        public async Task<IActionResult> OnGetAsync(int idGrupo)
        {
            ObtenerInfoGrupo(idGrupo);
            ObtenerTyperEnSesion();
            MisMensajes = await ObtenerMensajesDeGrupo(idGrupo);
            

            return Page();
        }

        private void ObtenerTyperEnSesion()
        {
            byte[] arr;
            HttpContext.Session.TryGetValue("Typer", out arr);

            if (arr != null)
            {
                string cadena = Encoding.UTF8.GetString(arr);
                TyperEnSesion = JsonSerializer.Deserialize<Typer>(cadena);
            }
        }

        private async Task<List<MensajeDominio>> ObtenerMensajesDeGrupo(int idGrupo)
        {
            string mensajesJson;

            try
            {
                mensajesJson =  await webClient.DownloadStringTaskAsync(URL_API + "/mensajes/obtenerMensajes/" + idGrupo);
            }
            catch (System.Exception)
            {
                
                throw;
            }

            ResultadoAPIMensajes resultado = JsonSerializer.Deserialize<ResultadoAPIMensajes>(mensajesJson);
            return resultado.result;
        }

        private void ObtenerInfoGrupo(int idGrupo)
        {
            // string grupoJson;
            // try
            // {
            //     grupoJson = webClient.DownloadString(new Uri("http://localhost:4000/mensajes/obtenerGrupos?idGrupo=" + idGrupo));
                
            // }
            // catch (System.Exception)
            // {
                
            //     throw;
            // }

            // ResultadoAPIGrupos result = JsonSerializer.Deserialize<ResultadoAPIGrupos>(grupoJson);
            // Grupo = result.result[0];

            byte[] arr;
            HttpContext.Session.TryGetValue("MisGrupos", out arr);

            if (arr != null)
            {
                string cadena = Encoding.UTF8.GetString(arr);
                List<Grupo> gruposEnSesion = JsonSerializer.Deserialize<List<Grupo>>(cadena);

                Grupo = gruposEnSesion.Find(grupo => grupo.IdGrupo == idGrupo);
            }
        }
    }
}