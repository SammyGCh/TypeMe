package fei.uv.mx.primeraapp.data.model

data class Correo (
    val idCorreo : Int? = 0,
    val direccion : String,
    val esPrincipal : Int,
    val idTyper : String? = ""
        )