package fei.uv.mx.primeraapp

import android.content.Intent
import android.os.AsyncTask
import android.os.Bundle
import android.view.View
import android.widget.LinearLayout
import android.widget.Toast
import com.google.android.material.floatingactionbutton.FloatingActionButton
import com.google.android.material.snackbar.Snackbar
import androidx.appcompat.app.AppCompatActivity
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.recyclerview.widget.DefaultItemAnimator
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import fei.uv.mx.primeraapp.adapters.AlumnosAdapter
import fei.uv.mx.primeraapp.data.LoginDataSource
import fei.uv.mx.primeraapp.data.LoginRepository
import fei.uv.mx.primeraapp.data.Result
import fei.uv.mx.primeraapp.data.model.Alumno
import fei.uv.mx.primeraapp.data.model.LoggedInUser
import fei.uv.mx.primeraapp.ui.login.LoginActivity

class ConsultaAlumnosActivity : AppCompatActivity() {

    var listaAlumnos = ArrayList<Alumno>()
    var adaptadorAlumnos : AlumnosAdapter? = null
    lateinit var lista : RecyclerView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_consulta_alumnos)
        setSupportActionBar(findViewById(R.id.toolbar))

        lista = findViewById<RecyclerView>(R.id.lista)
        adaptadorAlumnos = AlumnosAdapter(listaAlumnos, {

        } )
        lista.layoutManager = LinearLayoutManager(applicationContext)
        lista.itemAnimator = DefaultItemAnimator()
        lista.adapter = adaptadorAlumnos


        findViewById<FloatingActionButton>(R.id.fab).setOnClickListener { view ->
            Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                .setAction("Action", null).show()
        }

        AlumnosTask().execute()
    }

    inner class AlumnosTask  : AsyncTask<String, Void, Boolean>() {

        override fun onPreExecute() {
            //loading.visibility = View.VISIBLE
        }

        override fun doInBackground(vararg params: String): Boolean {
            val repo = LoginRepository(LoginDataSource())
            val resultado  = repo.cargarAlumnos(listaAlumnos)
            if (resultado is Result.Success) {
                //listaAlumnos = resultado.data
                return true
            } else
                return   false
        }


        override fun onPostExecute(result: Boolean) {
            //loading.visibility = View.GONE
            if (result) {


                adaptadorAlumnos?.notifyDataSetChanged()


            } else {
                val toast = Toast.makeText(this@ConsultaAlumnosActivity, "Error al cargar los alumnos", Toast.LENGTH_LONG)
                toast.show()
            }
        }

    }
}