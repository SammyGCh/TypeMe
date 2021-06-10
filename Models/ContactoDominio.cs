using System.Text.Json.Serialization;

namespace TypeMeWeb.Models
{
    public class ContactoDominio
    {
        [JsonPropertyName("bloqueado")]
        public bool Bloqueado { get; set; }

        [JsonPropertyName("esFavorito")]
        public bool EsFavorito { get; set; }

        [JsonPropertyName("contacto")]
        public Typer Contacto { get; set; }
    }
}