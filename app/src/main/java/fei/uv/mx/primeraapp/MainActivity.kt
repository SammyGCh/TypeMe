package fei.uv.mx.primeraapp

import android.content.DialogInterface
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.*
import androidx.appcompat.app.AlertDialog
import fei.uv.mx.primeraapp.ui.login.LoginActivity

class MainActivity : AppCompatActivity() {
    lateinit var boton : Button
    val operadores = arrayOf<String>("Sumar","Restar","Multiplicar","Dividir")

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val saludo = findViewById<TextView>(R.id.saludo)
        if (LoginActivity.usuario != null)
            saludo.text = "Bienvenido "+LoginActivity.usuario?.status

        var listaOperadores : Spinner


        boton = findViewById(R.id.boton)
        boton.setOnClickListener {
            sumar(it)
        }
        listaOperadores = findViewById(R.id.listaOperadores)
        listaOperadores.adapter = ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, operadores)
        listaOperadores.onItemSelectedListener = object : AdapterView.OnItemSelectedListener{
            override fun onNothingSelected(parent: AdapterView<*>?) {

            }

            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                boton.text = operadores[position]
            }

        }
    }

    public fun sumar(view: View) {
        val op1 : EditText  = findViewById(R.id.op1)
        val op2 : EditText  = findViewById(R.id.op2)
        val resultado : EditText  = findViewById(R.id.resultado)
        val dresultado : Double
        if (boton.text.equals(operadores[0]))
            dresultado = op1.text.toString().toDouble() + op2.text.toString().toDouble()
        else if (boton.text.equals(operadores[1]))
            dresultado = op1.text.toString().toDouble() - op2.text.toString().toDouble()
        else if (boton.text.equals(operadores[2]))
            dresultado = op1.text.toString().toDouble() * op2.text.toString().toDouble()
        else {
            if (op2.text.toString().toDouble() == 0.0) {

                val dialogBuilder = AlertDialog.Builder(this)
                dialogBuilder.setMessage("El divisor no puede ser 0")
                    .setCancelable(false)

                    .setNegativeButton("Aceptar", DialogInterface.OnClickListener {
                            dialog, id ->

                        dialog.cancel()
                    })

                val alert = dialogBuilder.create()
                alert.setTitle("Error en los datos")
                alert.show()
                dresultado = 0.0
            } else
                dresultado = op1.text.toString().toDouble() / op2.text.toString().toDouble()
        }

        resultado.setText(dresultado.toString())
    }
}