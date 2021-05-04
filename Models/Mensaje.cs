using System;
using System.Collections.Generic;

#nullable disable

namespace MSMensajes.Models
{
    public partial class Mensaje
    {
        public int IdMensaje { get; set; }
        public string Contenido { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int IdGrupo { get; set; }
        public string IdTyper { get; set; }
        public string IdMultimedia { get; set; }

        public virtual Grupo IdGrupoNavigation { get; set; }
    }
}
