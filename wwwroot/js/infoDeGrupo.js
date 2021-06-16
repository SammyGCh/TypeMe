
function consultarInfoDeGrupo(idGrupo) {
    $("#infoDeGrupoDialog").modal("show")
    obtenerInfoDeGrupo(idGrupo)
    .then((resultado) => {
        $("#loader3").hide()
        if(resultado.status === true) {
            $("#nombreDelGrupo").text(resultado.grupo.nombre)
            $("#descripcionDelGrupo").text(resultado.grupo.descripcion)
            var fecha = new Date(resultado.grupo.fechaCreacion.toString())
            $("#fechaCreacionDelGrupo").text(fecha.getDate() + "/" + (fecha.getMonth() + 1).toLocaleString("en-US", {minimumIntegerDigits: 2,
                useGrouping: false}) + "/" + fecha.getFullYear())
            obtenerIntegrantesDeGrupo(idGrupo)
            .then((infoIntegrantes) => {
                if (infoIntegrantes.status === true) {
                    $("#loader4").hide()
                    mostrarIntegrantesDeGrupo(infoIntegrantes.result)
                }
            })
            .catch((err) => {
                console.log(err)
            })
        }
        
    })
    .catch((err) => {
        console.log(err)
    })
}

function obtenerInfoDeGrupo(idGrupo) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: 'GET',
            url: '/Login/ObtenerGrupo?idGrupo=' + idGrupo,
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

function mostrarIntegrantesDeGrupo(integrantes) {
    integrantes.forEach(integrante => {
        $("#listaIntegrantesDeGrupo").append(
            '<div id="'+ integrante.IdTyper +'" class="integrante">'+
                '<span class="nombreIntegrante">' + integrante.Username + '</span>' +
                '<span class="estadoIntegrante">' + integrante.Estado + '</span>'
            +'</div>'
        )
    })
}

function cerrarInfoGrupoDialog() {
    $("#nombreDelGrupo").text("")
    $("#descripcionDelGrupo").text("")
    $("#fechaCreacionDelGrupo").text("")
    $("#listaIntegrantesDeGrupo")
    .html("<span><b>Integrantes del grupo:</b></span><div class='loader4' id='loader4'></div>")
    $("#loader3").show()
    $("#loader4").show()
    $("#infoDeGrupoDialog").modal("hide")
}