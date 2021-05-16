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


$(document).ready(irAlFinalDelChat());

const ningunArchivoSeleccionado = 0;
const archivoSeleccionado = 1;
const imagenSeleccionada = 2;
const videoSeleccionado = 3;
var scrollTopInicial;
var tipoSeleccionado = ningunArchivoSeleccionado;

function irAlFinalDelChat() {
//     console.log($('#chat-messages').scrollTop())
//     console.log($('#chat-messages')[0].scrollHeight)
//    $('#chat-messages').scrollTop($('#chat-messages')[0].scrollHeight);
    var tamañoScrollSobrante = 100;
    scrollTopInicial = $('#chat-messages').scrollTop() - tamañoScrollSobrante;
//     var chatMessages = $('#chat-messages')
//     chatMessages[0].scrollIntoView();
}

function seleccionarArchivo() {
  $('#files').trigger('click');

  $('#files').change(function () {
    if ($('#files')[0].files.length > 0) {
      var filename = $('#files')[0].files[0].name;

      $('#fileInformationContainer').show('swing', function () {
        $('#fileInformationContainer').addClass('fileSelected');
      });
      $('#fileName').text(filename);
      tipoSeleccionado = archivoSeleccionado;
    }
  });
}

function seleccionarImagen() {
  $('#images').trigger('click');

  $('#images').change(function () {
    if ($('#images')[0].files.length > 0) {
      var filename = $('#images')[0].files[0].name;
      var file = $('#images')[0].files;

      $('#fileInformationContainer').show('swing', function () {
        $('#fileInformationContainer').addClass('fileSelected');
      });
      $('#fileName').text(filename);
      tipoSeleccionado = imagenSeleccionada;
    }
  });
}

function seleccionarVideo() {
  $('#videos').trigger('click');

  $('#videos').change(function () {
    if ($('#videos')[0].files.length > 0) {
      var filename = $('#videos')[0].files[0].name;

      $('#fileInformationContainer').show('swing', function () {
        $('#fileInformationContainer').addClass('fileSelected');
      });
      $('#fileName').text(filename);
      tipoSeleccionado = videoSeleccionado;
      console.log(filename);
    }
  });
}

function eliminarArchivoSeleccionado() {
  $('#files').val(null);
  $('#images').val(null);
  $('#videos').val(null);
  $('#fileName').text('');
  $('#fileInformationContainer').hide('swing');
  tipoSeleccionado = ningunArchivoSeleccionado;
}

function enviarMensaje() {
  var mensaje = $('#mensaje').val();
  var fecha = new Date();
  var hora = fecha.getHours();
  var minutos = fecha.getMinutes();
  var usuario = 'Sammy';
  connection.invoke("EnviarMensaje", usuario, mensaje).catch(function (err) {
    return console.log(err)
  })

  /*
            $('#listaMensajes > tbody:last-child').append(
                "<tr>" +
                    "<div class=\"chat-sent-message\">" +
                        "<h1>Sammy</h1>" +
                        `<p>${mensaje}</p>` +
                    "</div>" + 
                "</tr>"
            );
            
            
            $('#listaMensajes').find('tbody')
            .append($('<tr>')
                .append(
                    $('<div>')
                        .append($('<h1>').append("Sammy"))
                        
                        .addClass('chat-sent-message')
                    )
            );
            */

  $('#listaMensajesBody').append(`<tr id="${usuario}">

                <td>
                <div class="chat-sent-message">
                    <h1>${usuario}</h1>
                </div>
                </td>
           </tr>`);

  irAlFinalDelChat();
}

function verificarScroll() {
  var scrollTop = $('#chat-messages').scrollTop();
  var scrollHeight = $('#chat-messages')[0].scrollHeight;
  console.log(scrollTop)
  if (scrollTop < scrollTopInicial) {
    $('#irAbajoIcon').show('swing');
  } else {
    $('#irAbajoIcon').hide('swing');
  }
}
