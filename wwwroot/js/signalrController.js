"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
  //Agregar los grupos que tiene a la conexión
}).catch(function (err) {
  console.log(err)
})

connection.on("RecibirMensaje", function (user, message) {
  console.log(user, message)
});