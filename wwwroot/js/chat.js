const ningunArchivoSeleccionado = 0;
const archivoSeleccionado = 1;
const imagenSeleccionada = 2;
const videoSeleccionado = 3;
var scrollTopInicial;


$(document).ready(irAlFinalDelChat());


//var tipoSeleccionado = ningunArchivoSeleccionado;

function irAlFinalDelChat() {
//     console.log($('#chat-messages').scrollTop())
//     console.log($('#chat-messages')[0].scrollHeight)
    var chatMessages = $('#chat-messages')

    if(chatMessages) {
      chatMessages.scrollTop(chatMessages[0].scrollHeight);
      var tamañoScrollSobrante = 100;
      scrollTopInicial = chatMessages.scrollTop() - tamañoScrollSobrante;
    }
    
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

function enviarMensaje(idTyper) {
  idTyperEnSesion = idTyper
  var contenido = $('#mensaje').val();
  var fecha = new Date();
  var mensaje = {
    contenido: contenido,
    idGrupo: grupoSeleccionado.idGrupo,
    idTyper: idTyper
  }

  $.ajax({
    type: 'POST',
    url: 'http://localhost:4000/mensajes/enviarMensaje',
    dataType: 'json',
    async: true,
    contentType: 'application/json',
    data: JSON.stringify(mensaje),
    success: function (response) {
      console.log(response)
      if (response.status === true) {
        chatConnection.invoke("EnviarMensaje", response.result).catch(function (err) {
          return console.log(err)
        })
      } else {
        
      }
    },
    error: function (response) {
      console.log(response)
    },
  });

  
}

function recibirMensaje(mensaje) {
  var fecha = mensaje.fecha.split("T")
  //var fechaCompuesta = fecha.getDate() + fecha.getHours() + ":" + fecha.getMinutes()
  var fechaCompuesta = fecha.getHours() + ":" + fecha.getMinutes()
  console.log(fecha)

  if (idTyperEnSesion === mensaje.typer.idTyper) {
    $('#listaMensajes')
    .append($('<li>')
      .append($('<div>')
        .append($('<h1>').append("Sammy"))
        .append($('<p>').text(mensaje.contenido))
        .addClass('chat-sent-message')
        .append($('<span>').text(fechaCompuesta))
      )
    );
  }
  else {
    $('#listaMensajes')
    .append($('<li>')
      .append($('<div>')
        .append($('<h1>').append("Sammy"))
        .append($('<p>').text(mensaje.contenido))
        .addClass('chat-received-message')
        .append($('<span>').text(fechaCompuesta))
      )
    );
  }
  
}

function verificarScroll() {
  var scrollTop = $('#chat-messages').scrollTop();
  var scrollHeight = $('#chat-messages')[0].scrollHeight;
  //console.log(scrollTop)
  if (scrollTop < scrollTopInicial) {
    $('#irAbajoIcon').show('swing');
  } else {
    $('#irAbajoIcon').hide('swing');
  }
}
