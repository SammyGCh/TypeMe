package fei.uv.mx.primeraapp

import android.app.Activity
import android.content.Context
import android.content.DialogInterface
import android.content.Intent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.media.Image
import android.os.AsyncTask
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.provider.MediaStore
import android.util.Base64
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.view.WindowManager
import android.view.inputmethod.InputMethodManager
import android.widget.*
import android.widget.CalendarView.OnDateChangeListener
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import fei.uv.mx.primeraapp.data.LoginDataSource
import fei.uv.mx.primeraapp.data.LoginRepository
import fei.uv.mx.primeraapp.data.Result
import fei.uv.mx.primeraapp.data.model.Alumno
import fei.uv.mx.primeraapp.data.model.LoggedInUser
import fei.uv.mx.primeraapp.ui.login.LoginActivity
import java.io.ByteArrayOutputStream
import java.util.*
import java.util.concurrent.Executors


class EditarAlumnoActivity : AppCompatActivity() {

    lateinit var txtMatricula : EditText
    lateinit var txtNombre : EditText
    lateinit var txtDomicilio : EditText
    lateinit var txtTelefono : EditText
    lateinit var txtFecha : CalendarView
    lateinit var foto : ImageView

    lateinit var txtSexo : Spinner
    lateinit var loading : ProgressBar
    private var sexo = "Masculino"
    private var fecha = Date()
    private var alumno : Alumno? = null
    private var id = 0
    private var enviarFoto = false;

    private var imagen: Bitmap? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_editar_alumno)

        title = "Agregar alumno"

        txtMatricula = findViewById(R.id.txtMatricula)
        txtNombre = findViewById(R.id.txtNombre)
        txtDomicilio = findViewById(R.id.txtDomicilio)
        txtTelefono = findViewById(R.id.txtTelefono)
        txtFecha = findViewById(R.id.txtFecha)

        txtFecha.setOnDateChangeListener(OnDateChangeListener { view, year, month, dayOfMonth -> // TODO Auto-generated method stub
            fecha = Date(year, month, dayOfMonth)
        })

        foto = findViewById(R.id.foto)

        foto.setOnClickListener {
            val intentoFoto = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
            if (intentoFoto.resolveActivity(packageManager) != null) {
                startActivityForResult(intentoFoto, 100)
            }
        }

        val generos = arrayOf<String>("Hombre","Mujer")
        txtSexo = findViewById(R.id.txtSexo)
        txtSexo.adapter = ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, generos)
        txtSexo.onItemSelectedListener = object : AdapterView.OnItemSelectedListener{
            override fun onNothingSelected(parent: AdapterView<*>?) {

            }

            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                sexo = generos[position]
            }

        }

        id = intent.getIntExtra("id",0)
        if (id != 0) {

            val matricula = intent.getStringExtra("matricula")
            val nombre = intent.getStringExtra("nombre")
            val domicilio = intent.getStringExtra("domicilio")
            val sexo = intent.getStringExtra("sexo")
            val telefono = intent.getStringExtra("telefono")
            val fechaN = intent.getStringExtra("fechaNacimiento")
            title = "Editar alumno - "+nombre
            txtMatricula.setText(matricula)
            txtNombre.setText(nombre)
            txtDomicilio.setText(domicilio)
            txtTelefono.setText(telefono)
            if (sexo.equals("H"))
                txtSexo.setSelection(0)
            else
                txtSexo.setSelection(1)

            val st = StringTokenizer(fechaN, "-T")
            val anio = st.nextToken().toInt()
            val mes = st.nextToken().toInt()
            val dia = st.nextToken().toInt()
            val fechaD = Date(anio-1900, mes-1, dia)
            txtFecha.date = fechaD.time
        }

        val view = this.currentFocus
        if (view != null) {
            val imm = getSystemService(Context.INPUT_METHOD_SERVICE) as InputMethodManager
            imm.hideSoftInputFromWindow(view.windowToken, 0)
        }
        // else {
        window.setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN)

        loading = findViewById(R.id.loading)


        val botonEliminar = findViewById<Button>(R.id.eliminarAlumno)
        botonEliminar.setOnClickListener {
            val ejecutor = Executors.newSingleThreadExecutor()
            val handler = Handler(Looper.getMainLooper())

            ejecutor.execute {
                handler.post {
                    loading.visibility = View.VISIBLE
                }

                val repo = LoginRepository(LoginDataSource())
                val resultado  = repo.eliminarAlumno(id)

                handler.post {
                    loading.visibility = View.GONE
                    if (resultado is Result.Success) {

                        val dialogBuilder = AlertDialog.Builder(this@EditarAlumnoActivity)
                        dialogBuilder.setMessage("Éxito. El alumno ha sido eliminado de manera exitosa")
                            .setCancelable(false)
                            .setPositiveButton("Aceptar", DialogInterface.OnClickListener {
                                    dialog, id ->
                                dialog.dismiss()
                                finishActivity(1)
                                finish()
                            })

                        val alert = dialogBuilder.create()
                        alert.setTitle("Datos eliminados")
                        alert.show()



                    } else {
                        val toast = Toast.makeText(this@EditarAlumnoActivity, "No se pudo eliminar el alumno", Toast.LENGTH_LONG)
                        toast.show()
                    }
                }

            }
        }
    }

    inner class GuardarAlumnoTask  : AsyncTask<Alumno, Void, Boolean>() {
        var usuariox : LoggedInUser? = null
        override fun onPreExecute() {
            loading.visibility = View.VISIBLE
        }

        override fun doInBackground(vararg params: Alumno): Boolean {
            val repo = LoginRepository(LoginDataSource())
            val resultado  = repo.guardarAlumno(params[0])
            if (resultado is Result.Success) {
                if (enviarFoto) {
                    val byteStream = ByteArrayOutputStream()
                    imagen?.compress(Bitmap.CompressFormat.JPEG, 60, byteStream)
                    val bytes: ByteArray = byteStream.toByteArray()
                    val cadBase64 = android.util.Base64.encodeToString(bytes, android.util.Base64.DEFAULT)
                    val resultado2 = repo.enviarFoto(params[0].matricula, cadBase64);
                    if (resultado2 is Result.Success)
                        return resultado2.data
                    else
                       return false;
                }
                return resultado.data
            } else
                return  false
        }


        override fun onPostExecute(result: Boolean) {
            loading.visibility = View.GONE
            if (result) {
                LoginActivity.usuario = usuariox

                val dialogBuilder = AlertDialog.Builder(this@EditarAlumnoActivity)
                dialogBuilder.setMessage("Éxito. El alumno ha sido almacenado de manera exitosa")
                    .setCancelable(false)
                    .setPositiveButton("Aceptar", DialogInterface.OnClickListener {
                            dialog, id ->
                        dialog.dismiss()
                        finishActivity(1)
                        finish()
                    })

                val alert = dialogBuilder.create()
                alert.setTitle("Datos almacenados")
                alert.show()



            } else {
                val toast = Toast.makeText(this@EditarAlumnoActivity, "No se pudo almacenar el alumno", Toast.LENGTH_LONG)
                toast.show()
            }
        }

    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        if (requestCode == 100 && resultCode == Activity.RESULT_OK) {
            imagen = data?.extras?.get("data") as Bitmap
            foto.setImageBitmap(imagen)
            enviarFoto = true;
        }
    }


    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.menu_guardar, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            R.id.guardarAlumno -> {
                val matricula = txtMatricula.text.toString()
                val nombre = txtNombre.text.toString()
                val domicilio = txtDomicilio.text.toString()
                val telefono = txtTelefono.text.toString()
                val sfecha = String.format("%04d-%02d-%02d", fecha.year, fecha.month+1, fecha.date)
                var errores = false
                var mensajes = ""
                if (matricula == "") {
                    errores = true
                    mensajes += "* La matrícula no puede estar vacía\n"
                }
                if (nombre == "") {
                    errores = true
                    mensajes += "* El nombre no puede estar vacío\n"
                }
                if (domicilio == "") {
                    errores = true
                    mensajes += "* El domicilio no puede estar vacío\n"
                }
                if (telefono == "") {
                    errores = true
                    mensajes += "* El teléfono no puede estar vacío\n"
                }


                if (errores) {
                    val dialogBuilder = AlertDialog.Builder(this@EditarAlumnoActivity)
                    dialogBuilder.setMessage(mensajes)
                        .setCancelable(false)
                        .setPositiveButton(
                            "Aceptar",
                            DialogInterface.OnClickListener { dialog, id ->
                                dialog.dismiss()

                            })

                    val alert = dialogBuilder.create()
                    alert.setTitle("Error en los datos")
                    alert.show()
                } else {
                    if (alumno == null)
                        alumno = Alumno(
                            id,
                            matricula,
                            nombre,
                            domicilio,
                            telefono,
                            sexo.substring(0, 1),
                            sfecha,
                            ""
                        )
                    GuardarAlumnoTask().execute(alumno)
                }
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }
}