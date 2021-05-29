using System;
using System.Collections.Generic;

#nullable disable

namespace MSMultimedia.Models
{
    public partial class TipoMultimedium
    {
        public TipoMultimedium()
        {
            Multimedia = new HashSet<Multimedia>();
        }

        public int IdTipoMultimedia { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Multimedia> Multimedia { get; set; }
    }
}
