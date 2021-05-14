using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TypeMeWeb.Models;

namespace TypeMeWeb.Pages
{
    public class PrincipalModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [ViewData]
        public List<Grupo> MisGrupos { get; set; }
        private WebClient webClient;

        public PrincipalModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            byte[] typerArr;
            HttpContext.Session.TryGetValue("Typer", out typerArr);
            if (typerArr == null)
                return RedirectToPage("./Index");


            // webClient = new WebClient();
            
            // Typer typerEnSesion = null;

            // string cadena = Encoding.UTF8.GetString(typerArr);
            // typerEnSesion = JsonSerializer.Deserialize<Typer>(cadena);
            // string grupoJson;
            // try
            // {
            //     grupoJson = webClient.DownloadString(new Uri("http://localhost:4000/mensajes/misGrupos/" + typerEnSesion.IdTyper));
            // }
            // catch (System.Exception)
            // {
                
            //     throw;
            // }
            
            // ResultadoAPIGrupos result = JsonSerializer.Deserialize<ResultadoAPIGrupos>(grupoJson);

            // MisGrupos = result.result;
            return Page();
        }

    }
}