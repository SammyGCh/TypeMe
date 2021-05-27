namespace MSTypers.Utilidades
{
        /// <summary>
        /// Clase con la que se recibira la informacion de las peticiones entrantes, se requiere un identificador como ID o username 
        /// junto con la informacion complementario como contraseña y un dato extra en caso de necesitarlo
        /// </summary>
    public class InformacionDePeticiones
    {

        public string IdentificadorTyper { get; set; }
        public string InformacionComplementaria { get; set; }
        public string InformacionActualizada { get; set; }
        public string ModificadorDeMetodo { get; set; }

        public  InformacionDePeticiones()
       {
           this.IdentificadorTyper = "";
           this.InformacionComplementaria = "";
           this.InformacionActualizada = "";
           this.ModificadorDeMetodo = "";
       }
    }
}
