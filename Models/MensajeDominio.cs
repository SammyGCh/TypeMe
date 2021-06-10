using System;
using System.Collections.Generic;
using MSMultimedia.Models;
using System.Text.Json.Serialization;

namespace TypeMeWeb.Models
{
    public class MensajeDominio
    {
        [JsonPropertyName("idMensaje")]
        public int IdMensaje { get; set; }
        [JsonPropertyName("contenido")]
        public string Contenido { get; set; }
        [JsonPropertyName("fecha")]
        public DateTime Fecha { get; set; }
        [JsonIgnore]
        public String Hora { get {return Fecha.ToString("HH:mm");} set {} }
        [JsonPropertyName("idGrupo")]
        public int IdGrupo { get; set; }
        [JsonPropertyName("typer")]
        public Typer Typer { get; set; }
        [JsonPropertyName("idMultimedia")]
        public string IdMultimedia { get; set; }
    }
}
