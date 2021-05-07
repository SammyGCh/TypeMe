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
    public class PrincipalModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PrincipalModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.Get("Typer") == null)
                return RedirectToPage("./Index");

            return Page();
        }

    }
}