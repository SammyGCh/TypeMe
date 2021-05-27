using System;
using System.Collections.Generic;

#nullable disable

namespace MSTypers.Models
{
    public partial class Correo
    {
        public int IdCorreo { get; set; }
        public string Direccion { get; set; }
        public sbyte EsPrincipal { get; set; }
        public string IdTyper { get; set; }

        public virtual Typer IdTyperNavigation { get; set; }
    }
}
