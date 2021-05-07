using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TypeMeWeb.Pages
{
    public class RegistrarCuentaModel : PageModel
    {
        [BindProperty]
        public string EmailPrincipal { get; set; }
        [BindProperty]
        public string EmailSecundario { get; set; }
        [BindProperty]
        public string Usuario { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string ConfirmacionPassword { get; set; }
        [ViewData]
        public string Resultado { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public RegistrarCuentaModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            bool sonPasswordsIguales = VerificarConfirmacionDePassword();

            if(sonPasswordsIguales)
            {
                return new RedirectToPageResult("/Index");
            }
            else
            {
                
            }

            return Page();
        }

        private bool VerificarConfirmacionDePassword()
        {
            return Password.Equals(ConfirmacionPassword);
        }
    }
}