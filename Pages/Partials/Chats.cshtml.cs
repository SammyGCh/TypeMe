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
    public class ChatsModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private WebClient webClient;

        [ViewData]
        public List<Grupo> MisGrupos { get; set; }

        public ChatsModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            webClient = new WebClient();
            byte[] typerArr;
            HttpContext.Session.TryGetValue("Typer", out typerArr);
            
            Typer typerEnSesion = null;

            string cadena = Encoding.UTF8.GetString(typerArr);
            typerEnSesion = JsonSerializer.Deserialize<Typer>(cadena);
            string grupoJson;
            try
            {
                grupoJson = webClient.DownloadString(new Uri("http://localhost:4000/mensajes/misGrupos/" + typerEnSesion.IdTyper));
                
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
            ResultadoAPIGrupos result = JsonSerializer.Deserialize<ResultadoAPIGrupos>(grupoJson);

            MisGrupos = result.result;
        }
    }
}
