using System;
using System.Collections.Generic;

namespace TypeMeWeb.Models
{
    public class Typer
    {
        public Typer()
        {
            Contrasenia = new HashSet<Contrasenia>();
            Correos = new HashSet<Correo>();
        }

        public string IdTyper { get; set; }
        public string Username { get; set; }
        public string Estado { get; set; }
        public string FotoDePerfil { get; set; }
        public sbyte Estatus { get; set; }

        public ICollection<Contrasenia> Contrasenia { get; set; }
        public ICollection<Correo> Correos { get; set; }
    }
}
