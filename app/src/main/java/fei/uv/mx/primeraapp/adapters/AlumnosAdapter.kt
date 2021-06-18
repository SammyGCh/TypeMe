package fei.uv.mx.primeraapp.adapters

import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.recyclerview.widget.RecyclerView
import com.squareup.picasso.Picasso
import fei.uv.mx.primeraapp.R
import fei.uv.mx.primeraapp.data.model.Alumno
import kotlinx.android.synthetic.main.layout_item_alumno.view.*

class AlumnosAdapter(var listaAlumnos : List<Alumno>, val onAlumnoClick: (Alumno) -> Unit) : RecyclerView.Adapter<AlumnosAdapter.AlumnoHolder>() {
    inner class AlumnoHolder(view: View) : RecyclerView.ViewHolder(view) {
        var txtMatricula : TextView = view.findViewById(R.id.txtMatricula)
        var txtNombre : TextView = view.findViewById(R.id.txtNombre)
        var foto : ImageView = view.findViewById(R.id.foto)


    }



    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): AlumnoHolder {
        val itemView = LayoutInflater.from(parent.context)
            .inflate(R.layout.layout_item_alumno, parent, false)
        return AlumnoHolder(itemView)
    }

    override fun onBindViewHolder(holder: AlumnoHolder, position: Int) {
        val alumno = listaAlumnos[position]
        holder.txtMatricula.text = alumno.matricula
        holder.txtNombre.text = alumno.nombre
        holder.itemView.setOnClickListener{
            onAlumnoClick(listaAlumnos[position])
        }
        Picasso.get().load("http://192.168.3.2:5000/fotos/"+alumno.foto).into(holder.foto);
    }

    override fun getItemCount(): Int {
        return listaAlumnos.size
    }
}