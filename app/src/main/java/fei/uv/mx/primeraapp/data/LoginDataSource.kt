package fei.uv.mx.primeraapp.data

import android.icu.lang.UCharacter.LineBreak.LINE_FEED
import fei.uv.mx.primeraapp.data.model.Alumno
import fei.uv.mx.primeraapp.data.model.LoggedInUser
import fei.uv.mx.primeraapp.data.model.Typer
import org.json.JSONArray
import org.json.JSONObject
import java.io.BufferedWriter
import java.io.DataOutputStream
import java.io.IOException
import java.net.HttpURLConnection
import java.net.URL
import java.net.URLEncoder


/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class LoginDataSource {

    fun login(username: String, password: String): Result<LoggedInUser> {
        try {
            var url: URL? = null
            url = URL( "http://192.168.100.100:4000/typers/loginTyper")

            try {
                with(url.openConnection() as HttpURLConnection) {
                    requestMethod = "POST"

                    setRequestProperty("Accept", "application/json");
                    setRequestProperty("Content-Type", "application/json");
                    doInput = true
                    doOutput = true
                    outputStream.bufferedWriter().use {
                        val jsonBody = JSONObject()
                        jsonBody.put("identificadorTyper", username)
                        jsonBody.put("informacionComplementaria", password)

                        it.write(jsonBody.toString());
                        it.flush();
                    }

                    inputStream.bufferedReader().use {
                        val respuesta = it.readLine();
                        var jsonRespuesta : JSONObject? = null
                        jsonRespuesta = JSONObject(respuesta)
                        var usuario  = LoggedInUser(jsonRespuesta.getBoolean("status"),
                            jsonRespuesta.getString("message"),
                            jsonRespuesta.getString("result"))

                        return Result.Success(usuario);
                    }
                }

            } catch (e: Exception) {
                System.out.println(e);
                return Result.Error(IOException("Error de Inicio de sesión", e))
            }

        } catch (e: Throwable) {
            return Result.Error(IOException("Error de inicio de sesión", e))
        }
    }

    fun registrarNuevoTyper(nuevoTyper:Typer) : Result<LoggedInUser> {
        try {
            var url: URL? = null
            url = URL( "http://192.168.100.100:4000/typers/registrarTyper")

            try {
                with(url.openConnection() as HttpURLConnection) {
                    requestMethod = "POST"

                    setRequestProperty("Accept", "application/json");
                    setRequestProperty("Content-Type", "application/json");
                    doInput = true
                    doOutput = true
                    outputStream.bufferedWriter().use {
                        val jsonBody = JSONObject()
                        val arrayCorreos = JSONArray()
                        val arrayContrasenias = JSONArray()
                        val jsonCorreoPrincipal = JSONObject()
                        val jsonCorreoSecundario = JSONObject()
                        val jsonContrasenias = JSONObject()

                        jsonBody.put("username", nuevoTyper.username)
                        jsonBody.put("estado", nuevoTyper.estado)
                        jsonBody.put("fotoDePerfil", nuevoTyper.fotoDePerfil)
                        jsonBody.put("estatus", nuevoTyper.estatus)

                        jsonContrasenias.put("contrasenia1", nuevoTyper.contrasenia[0].contrasenia1)

                        jsonCorreoPrincipal.put("direccion", nuevoTyper.correos[0].direccion)
                        jsonCorreoPrincipal.put("esPrincipal", nuevoTyper.correos[0].esPrincipal)
                        jsonCorreoSecundario.put("direccion", nuevoTyper.correos[1].direccion)
                        jsonCorreoSecundario.put("esPrincipal", nuevoTyper.correos[1].esPrincipal)

                        arrayCorreos.put(jsonCorreoPrincipal)
                        arrayCorreos.put(jsonCorreoSecundario)
                        arrayContrasenias.put(jsonContrasenias)

                        jsonBody.put("contrasenia", arrayContrasenias)
                        jsonBody.put("correos", arrayCorreos)

                        it.write(jsonBody.toString());
                        it.flush();
                    }

                    inputStream.bufferedReader().use {
                        val respuesta = it.readLine();
                        var jsonRespuesta : JSONObject? = null
                        jsonRespuesta = JSONObject(respuesta)
                        var usuario  = LoggedInUser(jsonRespuesta.getBoolean("status"),
                            jsonRespuesta.getString("message"),
                            jsonRespuesta.getString("result"))

                        return Result.Success(usuario);
                    }
                }

            } catch (e: Exception) {
                System.out.println(e);
                return Result.Error(IOException("Error de registro de cuenta", e))
            }

        } catch (e: Throwable) {
            return Result.Error(IOException("Error de registro de cuenta", e))
        }
    }

    fun cargarAlumnos(listaAlumnos: ArrayList<Alumno>): Result<List<Alumno>> {
        try {
            var url: URL? = null
            url = URL( "http://192.168.3.2:5000/AlumnosServ/listaAlumnos")

            try {
                with(url.openConnection() as HttpURLConnection) {
                    requestMethod = "POST"

                    setRequestProperty("Accept", "application/json");
                    doInput = true
                    doOutput = true
                    outputStream.bufferedWriter().use {
                        it.flush();
                    }
                    inputStream.bufferedReader().use {
                        val respuesta = it.readLine();
                        val jsonRespuesta = JSONObject(respuesta)
                        val jsonAlumnos = jsonRespuesta.getJSONArray("data")
                        for(i in 0..jsonAlumnos.length()-1) {
                            val jsonAlumno  = jsonAlumnos[i] as JSONObject
                            var alumno  = Alumno(
                                jsonAlumno.getInt("id"),
                                jsonAlumno.getString("matricula"),
                                jsonAlumno.getString("nombre"),
                                jsonAlumno.getString("domicilio"),
                                jsonAlumno.getString("telefono"),
                                jsonAlumno.getString("sexo"),
                                jsonAlumno.getString("fecha"),
                                jsonAlumno.getString("matricula")+".jpg")
                            listaAlumnos.add(alumno)
                        }



                        return Result.Success(listaAlumnos)
                    }
                }
            } catch (e: Exception) {
                System.out.println(e);
                return Result.Error(IOException("Error en servicio Web", e))
            }

        } catch (e: Throwable) {
            return Result.Error(IOException("Error en servicio web", e))
        }
    }

    fun guardarAlumno(alumno: Alumno): Result<Boolean> {
        try {
            var url: URL? = null
            url = URL( "http://192.168.3.2:5000/AlumnosServ/GuardarDB")

            try {
                with(url.openConnection() as HttpURLConnection) {
                    requestMethod = "POST"

                    setRequestProperty("Accept", "application/json");
                    setRequestProperty("Content-Type", "application/json");
                    doInput = true
                    doOutput = true
                    outputStream.bufferedWriter().use {
                        var sb = StringBuilder()
                        sb.append("{");
                        sb.append("\"ID\": ");
                        sb.append(alumno.id);
                        sb.append(",");
                        sb.append("\"Matricula\": ");
                        sb.append("\"");
                        sb.append(alumno.matricula);
                        sb.append("\"");
                        sb.append(",");
                        sb.append("\"Nombre\": ");
                        sb.append("\"");
                        sb.append(alumno.nombre);
                        sb.append("\"");
                        sb.append(",");
                        sb.append("\"Domicilio\": ");
                        sb.append("\"");
                        sb.append(alumno.domicilio);
                        sb.append("\"");
                        sb.append(",");
                        sb.append("\"Telefono\": ");
                        sb.append("\"");
                        sb.append(alumno.telefono);
                        sb.append("\"");
                        sb.append(",");
                        sb.append("\"Sexo\": ");
                        sb.append("\"");
                        sb.append(alumno.sexo);
                        sb.append("\"");
                        sb.append(",");
                        sb.append("\"Fecha\": ");
                        sb.append("\"");
                        sb.append(alumno.fechaNac);
                        sb.append("\"");
                        sb.append("}");

                        var data: String = sb.toString()
                        it.write(data);
                        it.flush();
                    }
                    inputStream.bufferedReader().use {
                        val respuesta = it.readLine();
                        var jsonRespuesta : JSONObject? = null
                        jsonRespuesta = JSONObject(respuesta)
                        if (jsonRespuesta.getBoolean("correcto"))
                            return Result.Success(true)
                        else
                            return Result.Error(IOException(jsonRespuesta.getString("mensaje")))
                    }
                }
            } catch (e: Exception) {
                System.out.println(e);
                return Result.Error(IOException("Error de Inicio de sesión", e))
            }

        } catch (e: Throwable) {
            return Result.Error(IOException("Error de inicio de sesión", e))
        }
    }

    fun eliminarAlumno(idAlumno: Int): Result<Boolean> {
        try {
            var url: URL? = null
            url = URL( "http://192.168.3.2:5000/AlumnosServ/EliminarDB")

            try {
                with(url.openConnection() as HttpURLConnection) {
                    requestMethod = "POST"

                    setRequestProperty("Accept", "application/json");

                    doInput = true
                    doOutput = true
                    outputStream.bufferedWriter().use {


                        var data: String = "id="+idAlumno
                        it.write(data);
                        it.flush();
                    }
                    inputStream.bufferedReader().use {
                        val respuesta = it.readLine();
                        var jsonRespuesta : JSONObject? = null
                        jsonRespuesta = JSONObject(respuesta)
                        if (jsonRespuesta.getBoolean("correcto"))
                            return Result.Success(true)
                        else
                            return Result.Error(IOException(jsonRespuesta.getString("mensaje")))
                    }
                }
            } catch (e: Exception) {
                System.out.println(e);
                return Result.Error(IOException("Error de Inicio de sesión", e))
            }

        } catch (e: Throwable) {
            return Result.Error(IOException("Error de inicio de sesión", e))
        }
    }

    fun logout() {
        // TODO: revoke authentication
    }

    fun addFormField(name: String, value: String?, writer: BufferedWriter, boundary: String) {
        writer.append("--$boundary").append("\r\n")
        writer.append("Content-Disposition: form-data; name=\"$name\"")
            .append("\r\n")


        writer.append("\r\n")
        writer.append(value).append("\r\n")
        writer.flush()
    }

    fun enviarFoto(matricula: String, cadBase64: String): Result<Boolean> {
        try {
            var url: URL? = null
            url = URL( "http://192.168.3.2:5000/AlumnosServ/GuardarFoto")

            try {
                with(url.openConnection() as HttpURLConnection) {
                    requestMethod = "POST"

                    val bound = "==="+System.currentTimeMillis()+"==="
                    setRequestProperty("Accept", "application/json");
                    setRequestProperty("Content-Type", "multipart/form-data; boundary="+bound);
                    doInput = true
                    doOutput = true
                    outputStream.bufferedWriter().use {
                        //addFormField("matricula", matricula, it, bound)
                        //addFormField("foto", cadBase64, it, bound)

                        var data = String.format("matricula=%s&foto=%s", matricula, cadBase64)
                        it.write(data);
                        it.flush();
                    }
                    inputStream.bufferedReader().use {
                        val respuesta = it.readLine();
                        var jsonRespuesta : JSONObject? = null
                        jsonRespuesta = JSONObject(respuesta)
                        if (jsonRespuesta.getBoolean("correcto"))
                            return Result.Success(true)
                        else
                            return Result.Error(IOException(jsonRespuesta.getString("mensaje")))
                    }
                }
            } catch (e: Exception) {
                 System.out.println(e);
                return Result.Error(IOException("Error de Inicio de sesión", e))
            }

        } catch (e: Throwable) {
            return Result.Error(IOException("Error de inicio de sesión", e))
        }
    }
}