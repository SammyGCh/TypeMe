let nuevosIntegrantesSeleccionados = []

function mostrarAgregarIntegrantesDialog(idGrupo, idTyper) {
    console.log(idGrupo, " - ", idTyper)
    $("#agregarIntegrantesDialog").modal("show")
    $("#listaContactosIntegrantes").load("Partials/_ListaIntegrantesContactos?idGrupo=" + idGrupo + "&idTyper=" + idTyper)
}

function seleccionarNuevoIntegrante(idTyper) {
    console.log(idTyper)
    var contenedor = $("#" + idTyper)
    if(contenedor.hasClass("contactoContainer-seleccionado")) {
        contenedor.removeClass("contactoContainer-seleccionado")
        contenedor.find('i').hide()
    }
    else {
        contenedor.addClass("contactoContainer-seleccionado")
        contenedor.find('i').show()
    }
}