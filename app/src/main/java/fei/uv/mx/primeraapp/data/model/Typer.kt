package fei.uv.mx.primeraapp.data.model

data class Typer (
        val idTyper: String,
        val username: String,
        val estado: String,
        val fotoDePerfil: String,
        val estatus: Int,
//        val contrasenia : ArrayList<Contrasenia>,
        val contrasenia : Array<Contrasenia>,
        val correos : Array<Correo>
    )

