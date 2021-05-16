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
    public class ChatModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;

        public ChatModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
                    
        }

        public void OnPostGetMensajes([FromBody] Grupo grupo)
        {
            Console.Write(grupo.Nombre); 
        }
    }
}