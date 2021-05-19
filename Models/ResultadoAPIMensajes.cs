using System.Collections.Generic;

namespace TypeMeWeb.Models
{
    public class ResultadoAPIMensajes : ResultadoAPI 
    {
        public List<MensajeDominio> result { get; set; }
    }
}