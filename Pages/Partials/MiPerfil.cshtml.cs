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
    public class MiPerfilModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private WebClient webClient;

        [ViewData]
        public Typer Typer { get; set; }

        public MiPerfilModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            webClient = new WebClient();
            byte[] typerArr;
            HttpContext.Session.TryGetValue("Typer", out typerArr);

            string cadena = Encoding.UTF8.GetString(typerArr);
            Typer = JsonSerializer.Deserialize<Typer>(cadena);            
        }
    }
}