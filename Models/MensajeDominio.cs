using System;
using System.Collections.Generic;
using MSMultimedia.Models;
using System.Text.Json.Serialization;

namespace TypeMeWeb.Models
{
    public class MensajeDominio
    {
        [JsonPropertyName("IdMensaje")]
        public int IdMensaje { get; set; }
        [JsonPropertyName("Contenido")]
        public string Contenido { get; set; }
        [JsonPropertyName("Fecha")]
        public DateTime Fecha { get; set; }
        [JsonIgnore]
        public String Hora { get {return Fecha.ToString("HH:mm");} set {} }
        [JsonPropertyName("IdGrupo")]
        public int IdGrupo { get; set; }
        [JsonPropertyName("Typer")]
        public Typer Typer { get; set; }
        [JsonPropertyName("IdMultimedia")]
        public string IdMultimedia { get; set; }
    }
}
