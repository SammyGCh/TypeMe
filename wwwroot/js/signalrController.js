"use strict";

var chatConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

chatConnection.start().then(function () {
  //Agregar los grupos que tiene a la conexión
  
}).catch(function (err) {
  console.log(err)
})

chatConnection.on("RecibirMensaje", function (message) {
  if(grupoSeleccionado.idGrupo && grupoSeleccionado.idGrupo === message.idGrupo) {
    recibirMensaje(message)
  }
  else {
    $("#listaChats").find("#" + message.idGrupo).find("#icono-nuevo-mensaje").show();
  }
});