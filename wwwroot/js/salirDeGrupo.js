let infoSalidaDeGrupo = {
    idGrupo: 0,
    idTyper: ""
}

function salirDelGrupo(nombreGrupo, idGrupo, idTyper) {
    
    infoSalidaDeGrupo.idGrupo = idGrupo
    infoSalidaDeGrupo.idTyper = idTyper

    console.log(infoSalidaDeGrupo)
    $("#salirDeGrupoDialog").modal("show")
    $("#nombreGrupoASalir").text(nombreGrupo)    
}

function confirmarSalidaDeGrupo() {
    $("#salirDeGrupoDialog").modal("hide")
    //Llamar a la API para salir del grupo
    var url = 'http://localhost:4000/mensajes/salirDeGrupo?idGrupo=' + infoSalidaDeGrupo.idGrupo 
    + "&idTyper=" + infoSalidaDeGrupo.idTyper
    $.ajax({
        type: 'PUT',
        url: url,
        success: function (response) {
          if (response.status === true) {
            
            $("#listaChats").load("Partials/ChatsAdmin")
            $("#vistasContainer").load("Partials/_Bienvenida")
          }
          else {
            
          }
        },
        error: function (response) {
          console.log(response);
        },
      });
    //Si es exitoso, cerrar la ventana y luego refrescar los grupos.

    
}