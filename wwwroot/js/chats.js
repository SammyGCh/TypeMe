

function irAGrupo(idGrupo, nombre) {
    //alert("Id: " + idGrupo + " nombre: " + nombre);
    var grupo = {
        idGrupo: idGrupo,
        nombre: nombre
    }
    console.log("Holaaaa")
    $("#vistasContainer").load("Partials/_Chat")
    /*
    $.ajax({
        type: "POST",
        url: "Pages/Partials/Chat?handler=GetMensajes",
        data: grupo,
        sucess: function() {

        },
        failure: function(response) {
            console.log(response)
        },
        error: function(response) {
            console.log(response)
        }
    })
    */
}