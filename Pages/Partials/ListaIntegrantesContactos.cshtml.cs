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

namespace TypeMeWeb.Pages.Partials
{
    public class ListaIntegrantesContactosModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private WebClient webClient;

        [ViewData]
        public List<Typer> IntegrantesGrupos { get; set; }

        public List<ContactoDominio> Contactos { get; set; } 

        public ListaIntegrantesContactosModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
            webClient = new WebClient();
        }

        public async Task<IActionResult> OnGetAsync(int idGrupo, string idTyper)
        {
            Contactos = await ObtenerContactos(idTyper);
            IntegrantesGrupos = await ObtenerIntegrantesDeGrupo(idGrupo);
            

            return Page();
        }

        private async Task<List<Typer>> ObtenerIntegrantesDeGrupo(int idGrupo)
        {
            string integrantesJson;

            try
            {
                integrantesJson =  await webClient.DownloadStringTaskAsync("http://localhost:4000/mensajes/integrantesDeGrupo/" + idGrupo);
                
            }
            catch (System.Exception)
            {
                
                throw;
            }

            ResultadoAPIIntegrantes resultado = JsonSerializer.Deserialize<ResultadoAPIIntegrantes>(integrantesJson);
            return resultado.result;
        
        }

        private async Task<List<ContactoDominio>> ObtenerContactos(string idTyper)
        {
            string contactosJson;

            try
            {
                contactosJson =  await webClient.DownloadStringTaskAsync("http://localhost:4000/typers/obtenerContactos/" + idTyper);
                
            }
            catch (System.Exception)
            {
                
                throw;
            }

            ResultadoAPIContactos resultado = JsonSerializer.Deserialize<ResultadoAPIContactos>(contactosJson);
            return resultado.result;
        }
    }
}