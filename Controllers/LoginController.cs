using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TypeMeWeb.Controllers
{
    public class LoginController : Controller
    {
        // [HttpGet]
        // public async Task IniciarSesion()
        // {
        //     HttpClient client = new HttpClient();
        //     string url = "https://aca1e37dc2cb.ngrok.io/typer/obtenerCorreos?idTyper=d9b3ef22-016d-4470-a05f-db81a229e035";

        //     string responseBody = "";
        //     try{

        //         responseBody = await client.GetStringAsync(url);
        //     }
        //     catch(Exception e)
        //     {
        //         Console.WriteLine(e);
        //     }
        //     //values = JsonConvert.DeserializeObject(responseBody);
        //     return responseBody.ToString();
        // }
    }
}