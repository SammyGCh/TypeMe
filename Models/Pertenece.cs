using System;
using System.Collections.Generic;

#nullable disable

namespace MSMensajes.Models
{
    public partial class Pertenece
    {
        public int IdGrupo { get; set; }
        public string IdTyper { get; set; }

        public virtual Grupo IdGrupoNavigation { get; set; }
    }
}
