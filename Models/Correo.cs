using System;
using System.Collections.Generic;

namespace TypeMeWeb.Models
{
    public class Correo
    {
        public int IdCorreo { get; set; }
        public string Direccion { get; set; }
        public sbyte EsPrincipal { get; set; }
        public string IdTyper { get; set; }
    }
}
