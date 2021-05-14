using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TypeMeWeb.Models;

namespace TypeMeWeb.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public IActionResult AgregarSesion([FromBody]Typer typer)
        {
            if(typer == null)
            {
                return new JsonResult(new {status = false});
            }

            string cad;

            try
            {
                cad = JsonSerializer.Serialize(typer);
            }
            catch (NotSupportedException)
            {
                return new JsonResult(new {status = false});
            }

            HttpContext.Session.Set("Typer", Encoding.UTF8.GetBytes(cad));

            return new JsonResult(new {status = true});
        }

        [HttpGet]
        public IActionResult ObtenerTyperEnSesion()
        {
            byte[] arr;
            HttpContext.Session.TryGetValue("Typer", out arr);

            if (arr == null)
                return new JsonResult(new {status = false});

            string cadena = Encoding.UTF8.GetString(arr);
            Typer typerEnSesion = JsonSerializer.Deserialize<Typer>(cadena);

            return new JsonResult(new {
                status = true,
                typer = typerEnSesion
            });
        }
    }
}