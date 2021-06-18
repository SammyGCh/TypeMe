package fei.uv.mx.primeraapp.ui.home

import android.content.Intent
import android.os.AsyncTask
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import androidx.recyclerview.widget.DefaultItemAnimator
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import fei.uv.mx.primeraapp.EditarAlumnoActivity
import fei.uv.mx.primeraapp.R
import fei.uv.mx.primeraapp.adapters.AlumnosAdapter
import fei.uv.mx.primeraapp.data.LoginDataSource
import fei.uv.mx.primeraapp.data.LoginRepository
import fei.uv.mx.primeraapp.data.Result
import fei.uv.mx.primeraapp.data.model.Alumno

class HomeFragment : Fragment() {

    private lateinit var homeViewModel: HomeViewModel
    var listaAlumnos = ArrayList<Alumno>()
    var adaptadorAlumnos : AlumnosAdapter? = null
    lateinit var lista : RecyclerView

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        homeViewModel =
            ViewModelProviders.of(this).get(HomeViewModel::class.java)
        val root = inflater.inflate(R.layout.fragment_home, container, false)

        lista = root.findViewById<RecyclerView>(R.id.lista)
        adaptadorAlumnos = AlumnosAdapter(listaAlumnos, {
            val intento = Intent(activity, EditarAlumnoActivity::class.java)
            intento.putExtra("id", it.id)
            intento.putExtra("matricula", it.matricula)
            intento.putExtra("nombre", it.nombre)
            intento.putExtra("domicilio", it.domicilio)
            intento.putExtra("sexo", it.sexo)
            intento.putExtra("telefono", it.telefono)
            intento.putExtra("fechaNacimiento", it.fechaNac)
            alumno = it
            startActivityForResult(intento, 1)
        })
        lista.layoutManager = LinearLayoutManager(activity?.applicationContext)
        lista.itemAnimator = DefaultItemAnimator()
        lista.adapter = adaptadorAlumnos

        AlumnosTask().execute()
        return root

    }

    companion object {
        var alumno : Alumno? = null
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
                val toast = Toast.makeText(activity, "Error al cargar los alumnos", Toast.LENGTH_LONG)
                toast.show()
            }
        }

    }
}