function confirmarActualizacionUsername(idTyper) {
    let nombreDeUsuario = $("#nombreDeUsuario")

    if (nombreDeUsuario.hasClass("campo_invalido"))
        nombreDeUsuario.removeClass("campo_invalido")

    if(nombreDeUsuario.val().length < 4) {
        nombreDeUsuario.addClass("campo_invalido")
    }
    else {
        let infoTyper = {
            identificadorTyper: idTyper,
            informacionActualizada: nombreDeUsuario.val(),
            modificadorDeMetodo: "usuario"
        }
        $.ajax({
            type: 'PUT',
            url: 'http://localhost:4000/typers/actualizarInfoTyper',
            dataType: 'json',
            async: true,
            contentType: 'application/json',
            data: JSON.stringify(infoTyper),
            success: function (response) {
              if (response.status === true) {
                actualizarTyperEnSesion(response.result)
                
              }
              else {
                mostrarErrorActualizacion(response.message)
              }  
            },
            error: function (response) {
              mostrarErrorServidor()
            },
        });
    }
}

function editarMiUsername() {
    $("#editarUsernameBtn").hide()
    $("#actualizarUsernameBtn").show()
    $("#cancelarEdicionUsernameBtn").show()
    $("#nombreDeUsuario").prop('disabled', false)
    $("#nombreDeUsuario").focus()
}

function cancelarEdicion() {
    $("#vistasContainer").load("Partials/_MiPerfil")
}

function confirmarEdicionCorreoPrincipal(idTyper, correoAActualizar) {
    var correoPrincipal = $("#correoPrincipal")

    if (correoPrincipal.hasClass("campo_invalido"))
        correoPrincipal.removeClass("campo_invalido")

    if(!esCorreoCorrecto(correoPrincipal.val())) {
        correoPrincipal.addClass("campo_invalido")
    }
    else {
        let infoCorreo = {
            identificadorTyper: idTyper,
            informacionComplementaria: correoAActualizar,
            informacionActualizada: correoPrincipal.val()
        }
        $.ajax({
            type: 'PUT',
            url: 'http://localhost:4000/typers/actualizarCorreo',
            dataType: 'json',
            async: true,
            contentType: 'application/json',
            data: JSON.stringify(infoCorreo),
            success: function (response) {
              if (response.status === true) {
                actualizarTyperEnSesion(response.result)
                
              }
              else {
                mostrarErrorActualizacion(response.message)
              }  
            },
            error: function (response) {
              mostrarErrorServidor()
            },
        });
    }
}

function editarCorreoPrincipal() {
    $("#editarCorreoPrincipalBtn").hide()
    $("#actualizarCorreoPrincipalBtn").show()
    $("#cancelarCorreoPrincipalBtn").show()
    $("#correoPrincipal").prop('disabled', false)
    $("#correoPrincipal").focus()
}

function confirmarEdicionCorreoSecundario(idTyper, correoAActualizar) {
    var correoSecundario = $("#correoSecundario")

    if (correoSecundario.hasClass("campo_invalido"))
        correoSecundario.removeClass("campo_invalido")

    if(!esCorreoCorrecto(correoSecundario.val())) {
        correoSecundario.addClass("campo_invalido")
    }
    else {
        let infoCorreo = {
            identificadorTyper: idTyper,
            informacionComplementaria: correoAActualizar,
            informacionActualizada: correoSecundario.val()
        }
        $.ajax({
            type: 'PUT',
            url: 'http://localhost:4000/typers/actualizarCorreo',
            dataType: 'json',
            async: true,
            contentType: 'application/json',
            data: JSON.stringify(infoCorreo),
            success: function (response) {
                console.log(response)
              if (response.status === true) {
                actualizarTyperEnSesion(response.result)
                
              }
              else {
                mostrarErrorActualizacion(response.message)
              }  
            },
            error: function (response) {
              mostrarErrorServidor()
            },
        });
    }
}

function editarCorreoSecundario() {
    $("#editarCorreoSecundarioBtn").hide()
    $("#actualizarCorreoSecundarioBtn").show()
    $("#cancelarCorreoSecundarioBtn").show()
    $("#correoSecundario").prop('disabled', false)
    $("#correoSecundario").focus()
}

function confirmarEdicionEstado(idTyper) {
    var estado = $("#estado")

    if (estado.hasClass("campo_invalido"))
        estado.removeClass("campo_invalido")

    if(estado.val().length === 0) {
        estado.addClass("campo_invalido")
    }
    else {
        let infoTyper = {
            identificadorTyper: idTyper,
            informacionActualizada: estado.val(),
            modificadorDeMetodo: "estado"
        }
        $.ajax({
            type: 'PUT',
            url: 'http://localhost:4000/typers/actualizarInfoTyper',
            dataType: 'json',
            async: true,
            contentType: 'application/json',
            data: JSON.stringify(infoTyper),
            success: function (response) {
              if (response.status === true) {
                actualizarTyperEnSesion(response.result)
                
              }
              else {
                mostrarErrorActualizacion(response.message)
              }  
            },
            error: function (response) {
              mostrarErrorServidor()
            },
        });
    }
}

function editarEstado() {
    $("#editarEstadoBtn").hide()
    $("#actualizarEstadoBtn").show()
    $("#cancelarEstadoBtn").show()
    $("#estado").prop('disabled', false)
    $("#estado").focus()
}

function mostrarMensajeUsernameActualizo(username) {
    $("#titulo").text("Usuario actualizado")
    $("#dialogMensajePrincipal").text("¡Tu usuario fue actualizado exitosamente!")
    $("#dialogMensaje").text("Tu nuevo usuario es: " + username)
    $("#dialogActualizacion").modal('show')
}

function mostrarErrorActualizacion(mensaje) {
    $("#titulo").text("Error de actualización")
    $("#dialogMensajePrincipal").text("¡Lo sentimos! Ocurrio un error de actualización.")
    $("#dialogMensaje").text(mensaje)
    $("#dialogActualizacion").modal('show')
}

function mostrarErrorServidor() {
    $("#titulo").text("Error")
    $("#dialogMensajePrincipal").text("¡Ups! Ocurrio un error.")
    $("#dialogMensaje").text("No podemos atender tu solicitud ahora mismo. Intenta más tarde.")
    $("#dialogActualizacion").modal('show')
}

function actualizarTyperEnSesion(typer) {
    $.ajax({
        type: "PUT",
        url: "/Login/ActualizarSesion",
        contentType: "application/json",
        data: JSON.stringify(typer),
        success: function(result) {
            if (result.status === true)
            {
                $("#vistasContainer").load("Partials/_MiPerfil")
            }
            else {
                alert("Ocurrió un error")
            }
        },
        error: function() {
            alert("Ocurrió un al intentar conectarse al servidor")
        }
    })
}

function cerrarModal() {
    $("#dialogActualizacion").modal('hide')
}

function esCorreoCorrecto(email) {
    const regex = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

    return regex.test(email)
}
