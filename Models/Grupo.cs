using System;
using System.Collections.Generic;

#nullable disable

namespace MSMensajes.Models
{
    public partial class Grupo
    {
        public Grupo()
        {
            Mensajes = new HashSet<Mensaje>();
            Perteneces = new HashSet<Pertenece>();
        }

        public int IdGrupo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Mensaje> Mensajes { get; set; }
        public virtual ICollection<Pertenece> Perteneces { get; set; }
    }
}
