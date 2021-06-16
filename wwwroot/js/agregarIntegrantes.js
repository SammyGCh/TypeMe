let nuevosIntegrantesInfo = {
    idGrupo: 0,
    integrantes: []
}

function mostrarAgregarIntegrantesDialog(idGrupo, idTyper) {
    $("#agregarIntegrantesDialog").modal("toggle")
    buscarContactos(idTyper)
    .then((response) => {
        if (response.status === true) {
            var contactosDeTyper = response.result
            obtenerIntegrantesDeGrupo(idGrupo)
            .then((result) => {
                if (result.status === true) {
                    var integrantesDelGrupo = result.result
                    nuevosIntegrantesInfo.idGrupo = idGrupo
                    mostrarContactosIntegrantes(contactosDeTyper, integrantesDelGrupo)
                }
            })
            .catch((error) => {
                console.log(error);
            })
        }
    })
    .catch((error) => {
        console.log(error)
    })
}

function mostrarContactosIntegrantes(contactosDeTyper, integrantesDelGrupo) {
    if(contactosDeTyper.length === integrantesDelGrupo.length - 1) {
        $("#listaContactosIntegrantes")
        .html("<p>Actualmente todos tus contactos forman parte de este grupo.</p>")
    }
    else {
        contactosDeTyper.forEach(contacto => {
            if(esIntegrante(contacto, integrantesDelGrupo)) {
                $("#listaContactosIntegrantes").append(
                    '<div id="'+ contacto.contacto.IdTyper +'" class="integranteContainer">'+
                        '<span>' + contacto.contacto.Username + '</span>' +
                        '<i class="bx bxs-check-circle bx-sm" style="color: #D30000;"></i>'
                    +'</div>'
                )
            }
            else {
                
                $("#listaContactosIntegrantes").append(
                    '<div id="'+ contacto.contacto.IdTyper +'" class="contactoContainer" onclick="seleccionarNuevoIntegrante(event)">' +
                        '<span>' + contacto.contacto.Username + '</span>' +
                        '<i class="bx bxs-check-circle bx-sm" style="color:#1bff00; display: none;"></i>'
                    +'</div>'
                )
                
            }
        })
        
    }
}

function esIntegrante(contacto, integrantesDelGrupo) {
    return integrantesDelGrupo.some(integrante => contacto.contacto.IdTyper === integrante.IdTyper)
}

function buscarContactos(idTyper) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "GET",
            url: "http://localhost:4000/typers/obtenerContactos/" + idTyper,
            success: function (response) {
                resolve(response)
            },
            error: function (error) {
                reject(error);
            }
        })
    })
}

function obtenerIntegrantesDeGrupo(idGrupo) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "GET",
            url: "http://localhost:4000/mensajes/integrantesDeGrupo/" + idGrupo,
            success: function (response) {
                resolve(response)
            },
            error: function (error) {
                reject(error);
            }
        })
    })
}

function agregarNuevosIntegrantes() {
    if (nuevosIntegrantesInfo.integrantes.length === 0) {
        $("#mensajeErrorNuevosIntegrantes").text("Por favor selecciona al menos un nuevo integrante.")
        $("#mensajeErrorNuevosIntegrantes").show()
    }
    else {
        $.ajax({
          type: 'POST',
          url: 'http://localhost:4000/mensajes/agregarIntegrantes/' + nuevosIntegrantesInfo.idGrupo,
          dataType: 'json',
          async: true,
          contentType: 'application/json',
          data: JSON.stringify(nuevosIntegrantesInfo.integrantes),
          success: function (response) {
            if (response.status === true) {
                response.result.forEach(nuevoIntegrante => {
                    chatConnection.invoke("AgregarNuevoIntegrante", nuevoIntegrante.IdTyper).catch(function (err) {
                        return console.log(err)
                    })
                })
                
                mostrarMensajeNuevoIntegrante()
            } else {
              mostrarMensajeErrorNuevosIntegrantes(response.message)
            }
          },
          error: function (response) {
            mostrarMensajeErrorNuevosIntegrantes("Ocurrió un error con el servidor. Intente más tarde.")
          },
        });
    }
}

function seleccionarNuevoIntegrante(event) {
    if (!event)
        event = window.event

    
    var sender = event.srcElement || event.target
    var idContenedor = "#" + sender.id.toString()
    var contenedor = $(idContenedor)
    if(contenedor.hasClass("contactoContainer-seleccionado")) {
        contenedor.removeClass("contactoContainer-seleccionado")
        contenedor.find('i').hide()
        nuevosIntegrantesInfo.integrantes = eliminarNuevoIntegranteSeleccionado(nuevosIntegrantesInfo.integrantes, sender.id)
        
    }
    else {
        contenedor.addClass("contactoContainer-seleccionado")
        contenedor.find('i').show()
        nuevosIntegrantesInfo.integrantes.push({
            idTyper: sender.id
        })
        $("#mensajeErrorNuevosIntegrantes").hide()
        
    }
}

function cerrarAgregarIntegranteDialog() {
    $("#listaContactosIntegrantes").html("")
    nuevosIntegrantesInfo = {
        idGrupo: 0,
        integrantes: []
    }
    $("#mensajeErrorNuevosIntegrantes").hide()
    $("#botonCancelarAgregacion").text("Cancelar")
    $("#botonAgregarContacto").show()
    $("#agregarIntegrantesDialog").modal('hide')
}

function eliminarNuevoIntegranteSeleccionado(arr, id) { 
    
    return arr.filter(function(ele){ 
        return ele.idTyper != id; 
    });
}

function mostrarMensajeNuevoIntegrante() {
    $("#botonCancelarAgregacion").text("Aceptar")
    $("#botonAgregarContacto").hide()
    $("#listaContactosIntegrantes").html("<p>Nuevo(s) integrante(s) agregado(s)</p>")
}

function mostrarMensajeErrorNuevosIntegrantes(mensaje) {
    $("#botonCancelarAgregacion").text("Aceptar")
    $("#botonAgregarContacto").hide()
    $("#modal-body").html("<p>" + mensaje +"</p>")
}