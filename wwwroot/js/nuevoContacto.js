const mensaje = $("#mensajeNuevoContacto")


function agregarContacto() {
    var campoUsuario = $("#usuarioContacto")

    if (!campoUsuario.val()) {
        mensaje.text("Ingresa el usuario.")
        mensaje.show()
    }
    else {

        agregarNuevoContacto(campoUsuario.val())
    }
}

async function agregarNuevoContacto(nuevoContacto) {
    $.ajax({
        type: 'GET',
        url: '/Login/ObtenerTyperEnSesion',
        contentType: 'application/json',
        success: function (result) {
            if (result.status == true) {
                let infoNuevoContacto = {
                    idTyper: result.typer.idTyper,
                    contacto: nuevoContacto
                }
                $.ajax({
                    type: 'POST',
                    url: 'http://localhost:4000/typers/agregarContacto',
                    dataType: 'json',
                    async: true,
                    contentType: 'application/json',
                    data: JSON.stringify(infoNuevoContacto),
                    success: function (response) {
                      if (response.status === true) {
                        mostrarMensajeContactoAgregado(nuevoContacto)
                        $("#agregarContactoDialog").modal("hide")
                      }
                      else {
                        mensaje.text(response.message)
                        mensaje.show();
                      }  
                    },
                    error: function (response) {
                      console.log(response);
                    },
                });
            }
        },
        error: function () {
          mensaje.text('Ocurrió un al intentar conectarse al servidor');
          mensaje.show();
        },
    });
}

$("#usuarioContacto").keypress(function () {
    mensaje.hide()
})

function mostrarMensajeContactoAgregado(nuevoContacto) {
    $("#modalDialogTitle").text("Contacto agregado")
    $("#nuevoContactoUsername").text(nuevoContacto)
    $("#mensajeContactoAgregado").show()
    $("#dialog").modal("show")
}

$('#agregarContactoDialog').on('hidden.bs.modal', function (e) {
    mensaje.hide()
    $("#mensajeContactoAgregado").hide()
    $("#mensajeContactoNoAgregado").hide()
    $("#mensajeGrupoCreado").hide()
    $("#mensajeErrorCreacionGrupo").hide()
    $("#mensajeErrorServidor").hide()
    $("#usuarioContacto").val("")
})