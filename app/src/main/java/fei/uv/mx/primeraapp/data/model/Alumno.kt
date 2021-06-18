package fei.uv.mx.primeraapp.data.model

import java.util.*

data class Alumno(val id: Int,
                  val matricula: String,
                  val nombre: String,
                  val domicilio: String,
                  val telefono: String,
                  val sexo: String,
                  val fechaNac: String,
                  val foto: String
)