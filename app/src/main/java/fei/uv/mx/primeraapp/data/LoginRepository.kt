package fei.uv.mx.primeraapp.data

import fei.uv.mx.primeraapp.data.model.Alumno
import fei.uv.mx.primeraapp.data.model.LoggedInUser
import fei.uv.mx.primeraapp.data.model.Typer

/**
 * Class that requests authentication and user information from the remote data source and
 * maintains an in-memory cache of login status and user credentials information.
 */

class LoginRepository(val dataSource: LoginDataSource) {

    var user: LoggedInUser? = null
        private set

    val isLoggedIn: Boolean
        get() = user != null

    init {
        user = null
    }

    fun logout() {
        user = null
        dataSource.logout()
    }

    fun login(username: String, password: String): Result<LoggedInUser> {
        val result = dataSource.login(username, password)

        if (result is Result.Success) {
            setLoggedInUser(result.data)
        }
        return result
    }

    fun registrarTyper(nuevoTyper : Typer): Result<LoggedInUser> {
        val result = dataSource.registrarNuevoTyper(nuevoTyper)

        return result
    }

    fun cargarAlumnos(listaAlumnos: ArrayList<Alumno>) : Result<List<Alumno>> {
        val result = dataSource.cargarAlumnos(listaAlumnos)
        return result
    }

    fun guardarAlumno(alumno: Alumno) : Result<Boolean> {
        val result = dataSource.guardarAlumno(alumno)
        return result
    }

    fun eliminarAlumno(idAlumno: Int) : Result<Boolean> {
        val result = dataSource.eliminarAlumno(idAlumno)
        return result
    }

    private fun setLoggedInUser(loggedInUser: LoggedInUser) {
        this.user = loggedInUser
    }

    fun enviarFoto(matricula: String, cadBase64: String): Result<Boolean> {
        val result = dataSource.enviarFoto(matricula, cadBase64)
        return result
    }
}