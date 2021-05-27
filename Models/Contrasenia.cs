using System;
using System.Collections.Generic;

#nullable disable

namespace MSTypers.Models
{
    public partial class Contrasenia
    {
        public int IdContrasenia { get; set; }
        public string Contrasenia1 { get; set; }
        public string IdTyper { get; set; }

        public virtual Typer IdTyperNavigation { get; set; }
    }
}
