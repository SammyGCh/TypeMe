package fei.uv.mx.primeraapp.data.model

/**
 * Data class that captures user information for logged in users retrieved from LoginRepository
 */
data class LoggedInUser(
    val status: Boolean,
    val message: String,
    val result: Any
)