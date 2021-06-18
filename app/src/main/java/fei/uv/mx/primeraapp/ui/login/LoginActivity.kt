package fei.uv.mx.primeraapp.ui.login

import android.app.Activity
import android.content.Intent
import android.icu.text.CaseMap
import android.os.AsyncTask
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import android.os.Bundle
import androidx.annotation.StringRes
import androidx.appcompat.app.AppCompatActivity
import android.text.Editable
import android.text.TextWatcher
import android.view.View
import android.view.inputmethod.EditorInfo
import android.widget.Button
import android.widget.EditText
import android.widget.ProgressBar
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.core.content.ContextCompat.startActivity
import fei.uv.mx.primeraapp.*

import fei.uv.mx.primeraapp.data.LoginDataSource
import fei.uv.mx.primeraapp.data.LoginRepository
import fei.uv.mx.primeraapp.data.Result
import fei.uv.mx.primeraapp.data.model.LoggedInUser

class LoginActivity : AppCompatActivity() {
    lateinit var loading : ProgressBar
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        title = "Type Me";
        setContentView(R.layout.activity_login)

        val username = findViewById<EditText>(R.id.username)
        val password = findViewById<EditText>(R.id.password)

        val login = findViewById<Button>(R.id.login)
        loading = findViewById<ProgressBar>(R.id.loading)
        login.setOnClickListener {

            LoginTask().execute(username.text.toString(), password.text.toString())
        }

        val botonRegistrar = findViewById<Button>(R.id.botonRegistrar);
        botonRegistrar.setOnClickListener{
            val intentoRegistrarCuenta = Intent(this, RegistrarCuenta::class.java)
            startActivity(intentoRegistrarCuenta)
        }
    }

    companion object {
        var usuario: LoggedInUser? = null

    }

    inner class LoginTask  : AsyncTask<String, Void, Boolean>() {
        var usuariox : LoggedInUser? = null
        override fun onPreExecute() {
            loading.visibility = View.VISIBLE
        }

        override fun doInBackground(vararg params: String): Boolean {
            val repo = LoginRepository(LoginDataSource())
            val resultado  = repo.login(params[0], params[1])
            if (resultado is Result.Success) {
                usuariox = resultado.data
                return resultado.data.status
            } else
             return   false
        }


        override fun onPostExecute(result: Boolean) {
            loading.visibility = View.GONE
            if (result) {
                LoginActivity.usuario = usuariox
                val intento = Intent(this@LoginActivity, MenuActivity::class.java)
                startActivity(intento)
            } else {
                val toast = Toast.makeText(this@LoginActivity, "Nombre de usuario y/o contraseña inválida", Toast.LENGTH_LONG)
                toast.show()
            }
        }

    }

}
