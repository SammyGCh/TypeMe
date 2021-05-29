using System;
using System.Collections.Generic;

#nullable disable

namespace MSMultimedia.Models
{
    public partial class Multimedia
    {
        public string IdMultimedia { get; set; }
        public string Ruta { get; set; }
        public int IdTipoMultimedia { get; set; }

        public virtual TipoMultimedium IdTipoMultimediaNavigation { get; set; }
    }
}
