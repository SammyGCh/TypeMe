package fei.uv.mx.primeraapp

import android.content.Intent
import android.os.AsyncTask
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.ProgressBar
import android.widget.Toast
import fei.uv.mx.primeraapp.data.LoginDataSource
import fei.uv.mx.primeraapp.data.LoginRepository
import fei.uv.mx.primeraapp.data.Result
import fei.uv.mx.primeraapp.data.model.Contrasenia
import fei.uv.mx.primeraapp.data.model.Correo
import fei.uv.mx.primeraapp.data.model.LoggedInUser
import fei.uv.mx.primeraapp.data.model.Typer
import fei.uv.mx.primeraapp.ui.login.LoginActivity

class RegistrarCuenta : AppCompatActivity() {
    lateinit var loading : ProgressBar

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        title = "Registrar cuenta"
        setContentView(R.layout.activity_registrar_cuenta)
        loading = findViewById<ProgressBar>(R.id.spinerRegistro)

        val username = findViewById<EditText>(R.id.campoUsuario)
        val contrasenia = findViewById<EditText>(R.id.campoContrasenia)
        val correoPrincipal = findViewById<EditText>(R.id.campoCorreoPrincipal)
        val correoSecundario = findViewById<EditText>(R.id.campoCorreoSecundario)

        val botonRegistro = findViewById<Button>(R.id.botonAccionRegistrar)
        botonRegistro.setOnClickListener {
            if (!username.text.isNullOrEmpty() &&
                !contrasenia.text.isNullOrEmpty() &&
                !correoPrincipal.text.isNullOrEmpty() &&
                !correoSecundario.text.isNullOrEmpty()){

                val nuevoTyper : Typer = Typer(
                    idTyper = "",
                    username = username.text.toString(),
                    estado = "",
                    fotoDePerfil = "",
                    estatus = 1,
                    contrasenia = arrayOf(Contrasenia(contrasenia1 = contrasenia.text.toString())),
                    correos = arrayOf(Correo(direccion = correoPrincipal.text.toString(), esPrincipal = 1),
                                    Correo(direccion = correoSecundario.text.toString(), esPrincipal = 0))
                    )

                RegistroTask().execute(nuevoTyper)
            }else{
                val toast = Toast.makeText(this@RegistrarCuenta, "Por favor ingresa los campos solicitados", Toast.LENGTH_LONG)
                toast.show()
            }
        }
    }

    inner class RegistroTask  : AsyncTask<Typer, Void, Boolean>() {
        override fun onPreExecute() {
            loading.visibility = View.VISIBLE
        }

        override fun doInBackground(vararg params: Typer): Boolean {
            val repo = LoginRepository(LoginDataSource())
            val resultado  = repo.registrarTyper(params[0])
            if (resultado is Result.Success) {
                return resultado.data.status
            } else
                return   false
        }


        override fun onPostExecute(result: Boolean) {
            loading.visibility = View.GONE
            if (result) {
                val toast = Toast.makeText(this@RegistrarCuenta, "Registro exitoso", Toast.LENGTH_LONG)
                toast.show()

                val intento = Intent(this@RegistrarCuenta, LoginActivity::class.java)
                startActivity(intento)
            } else {
                val toast = Toast.makeText(this@RegistrarCuenta, "Ocurrio un error en el registro, intente mas tarde", Toast.LENGTH_LONG)
                toast.show()
            }
        }

    }
}