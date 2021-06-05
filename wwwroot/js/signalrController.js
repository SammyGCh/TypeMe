"use strict";

var chatConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

chatConnection.start().then(function () {
  //Agregar los grupos que tiene a la conexión
  
}).catch(function (err) {
  console.log(err)
})

chatConnection.on("RecibirMensaje", function (message) {
  console.log(message)
  if(grupoSeleccionado.idGrupo && grupoSeleccionado.idGrupo === message.idGrupo) {
    console.log("estoy en el grupo " + grupoSeleccionado.nombre)
    recibirMensaje(message)
  }
  else {
    console.log("no estoy en el grupo XD")
    $("#listaChats").find("#" + message.idGrupo).find("#icono-nuevo-mensaje").show();
  }
});