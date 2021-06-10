using System.Collections.Generic;

namespace TypeMeWeb.Models
{
    public class ResultadoAPIContactos : ResultadoAPI 
    {
        public List<ContactoDominio> result { get; set; }
    }
}