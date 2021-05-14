using System;
using System.Collections.Generic;

namespace TypeMeWeb.Models
{
    public partial class Pertenece
    {
        public int IdGrupo { get; set; }
        public string IdTyper { get; set; }

        public virtual Grupo IdGrupoNavigation { get; set; }
    }
}
