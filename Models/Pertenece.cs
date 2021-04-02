using System;
using System.Collections.Generic;

#nullable disable

namespace MSMensajes.Models
{
    public partial class Pertenece
    {
        public int IdGrupo { get; set; }
        public string Username { get; set; }

        public virtual Grupo IdGrupoNavigation { get; set; }
    }
}
