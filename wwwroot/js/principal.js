

function irAMiPerfil() {
    $("#vistasContainer").load("Partials/_MiPerfil")
}

function cerrarSesion() {
    $.ajax({
        type: 'POST',
        url: '/Login/CerrarSesion',
        contentType: 'application/json',
        success: function () {
            window.location.pathname = '/login'
        },
        error: function () {
          
        },
    });    
}

function abrirListaMisContactos() {
    $("#misContactosDialog").modal('show')

    obtenerTyperEnSesion()
    .then((result) => {
        if(result.status === true) {
            var idTyper = result.typer.idTyper
            buscarContactos(idTyper)
            .then((resultado) => {
                $("#loader2").hide()
                var contactos = Array.from(resultado.result)
                
                if(resultado.status === true) {
                   
                    mostrarMisContactos(contactos)
                    
                }
                else {
                    $("#mensajeErrorMisContactos").text("Aun no tiene contactos. Agregue uno nuevo.")
                    $("#mensajeErrorMisContactos").show()
                }
            })
            .catch((err) => {
                console.log(err)
                $("#loader2").hide()
                $("#mensajeErrorMisContactos").text("Ocurrió un error con el servidor. Intente más tarde.")
                $("#mensajeErrorMisContactos").show()
            })
        } else {
            $("#loader2").hide()
            $("#mensajeErrorMisContactos").text(result.message)
            $("#mensajeErrorMisContactos").show()
        }
    })
    .catch((err) => {
        console.log(err)
        $("#loader2").hide()
        $("#mensajeErrorMisContactos").text("Ocurrió un error con el servidor. Intente más tarde.")
        $("#mensajeErrorMisContactos").show()
    })
}

function obtenerTyperEnSesion() {
    return new Promise((resolve, reject) => {
        $.ajax({
          type: 'GET',
          url: '/Login/ObtenerTyperEnSesion',
          contentType: 'application/json',
          success: function (result) {
            resolve(result)
          },
          error: function(err) {
            reject(err)
          }
        });
    })
}

function mostrarMisContactos(misContactos) {
    misContactos.forEach(contacto => {
        $("#listaMisContactos").append(
            '<div id="'+ contacto.contacto.IdTyper +'" class="integrante">' +
                '<span class="nombreIntegrante">' + contacto.contacto.Username + '</span>' +
                '<span class="estadoIntegrante">' + contacto.contacto.Estado + '</span>'
            +'</div>'
        )
    })
}

function cerrarMisContactosDialog() {
    $("#mensajeErrorMisContactos").hide()
    $("#listaMisContactos").html("")
    $("#misContactosDialog").modal('hide')
    $("#loader2").show()
}