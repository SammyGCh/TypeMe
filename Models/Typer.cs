using System;
using System.Collections.Generic;

#nullable disable

namespace MSTypers.Models
{
    public partial class Typer
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

        public virtual ICollection<Contrasenia> Contrasenia { get; set; }
        public virtual ICollection<Correo> Correos { get; set; }
    }
}
