const ningunArchivoSeleccionado = 0;
const archivoSeleccionado = 1;
const imagenSeleccionada = 2;
const videoSeleccionado = 3;
var scrollTopInicial;


$(document).ready(irAlFinalDelChat());

function irAlFinalDelChat() {
    // var chatMessages = $('#chat-messages')

    // if(chatMessages) {
    //   chatMessages.scrollTop(chatMessages[0].scrollHeight);
    //   var tamañoScrollSobrante = 100;
    //   scrollTopInicial = chatMessages.scrollTop() - tamañoScrollSobrante;
    // }
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
      var file = $('#images');
      
      grupoSeleccionado.archivoSeleccionado = file

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
  grupoSeleccionado.archivoSeleccionado = undefined;
  tipoSeleccionado = ningunArchivoSeleccionado;
}

async function enviarMensaje(idTyper) {
  
  idTyperEnSesion = idTyper
  var contenido = $('#mensaje').val();

  if (!contenido && grupoSeleccionado.archivoSeleccionado === undefined) {
    return
  }

  var mensaje = {
    contenido: contenido,
    idGrupo: grupoSeleccionado.idGrupo,
    idTyper: idTyper,
    idMultimedia: ""
  }

  if ((grupoSeleccionado.archivoSeleccionado != undefined && contenido) || 
      (grupoSeleccionado.archivoSeleccionado != undefined && !contenido)) 
  {
    enviarMultimedia()
    .then((response) => {
      if(response.status === true) {
        mensaje.idMultimedia = response.result.IdMultimedia

        $.ajax({
          type: 'POST',
          url: 'http://localhost:4000/mensajes/enviarMensaje',
          dataType: 'json',
          async: true,
          contentType: 'application/json',
          data: JSON.stringify(mensaje),
          success: function (response) {
            
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
    })
  } 
  else {
    $.ajax({
      type: 'POST',
      url: 'http://localhost:4000/mensajes/enviarMensaje',
      dataType: 'json',
      async: true,
      contentType: 'application/json',
      data: JSON.stringify(mensaje),
      success: function (response) {
        
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
  
}

function enviarMultimedia() {
  var form = new FormData();
  form.append(
    'file',
    grupoSeleccionado.archivoSeleccionado[0].files[0],
    grupoSeleccionado.archivoSeleccionado.val()
  );

  return new Promise((resolve, reject) => {
    $.ajax({
      url:
        'http://localhost:4000/mensajes/registrarMultimedia?idTyper=' + idTyper,
      method: 'POST',
      timeout: 0,
      processData: false,
      mimeType: 'multipart/form-data',
      contentType: false,
      data: form,
      success: function (response) {
        var respuesta = JSON.parse(response.toString());
        resolve(respuesta)
  
      },
      error: function (error) {
        reject(error)
      },
    });
  })
}

function recibirMensaje(mensaje) {
  var fechaCompleta = obtenerFechaCompleta(mensaje.fecha)
  if (idTyperEnSesion === mensaje.typer.idTyper) {

    if (mensaje.idMultimedia) {
      $('#listaMensajes').append(
        $('<li>').append(
          $('<div>')
            .append($('<h1>').append(mensaje.typer.username))
            .append($('<img>').attr('src', mensaje.idMultimedia))
            .append($('<p>').text(mensaje.contenido))
            .addClass('chat-sent-message')
            .append($('<span>').text(fechaCompleta))
        )
      );
    }
    else {
      $('#listaMensajes').append(
        $('<li>').append(
          $('<div>')
            .append($('<h1>').append(mensaje.typer.username))
            .append($('<p>').text(mensaje.contenido))
            .addClass('chat-sent-message')
            .append($('<span>').text(fechaCompleta))
        )
      );
    }

    
    $('#mensaje').val("");
    eliminarArchivoSeleccionado()
  }
  else {

    if (mensaje.idMultimedia) {
      $('#listaMensajes').append(
        $('<li>').append(
          $('<div>')
            .append($('<h1>').append(mensaje.typer.username))
            .append($('<img>').attr('src', mensaje.idMultimedia))
            .append($('<p>').text(mensaje.contenido))
            .addClass('chat-received-message')
            .append($('<span>').text(fechaCompleta))
        )
      );
    }
    else{
      $('#listaMensajes').append(
        $('<li>').append(
          $('<div>')
            .append($('<h1>').append(mensaje.typer.username))
            .append($('<p>').text(mensaje.contenido))
            .addClass('chat-received-message')
            .append($('<span>').text(fechaCompleta))
        )
      );
    }
    
  }
  
}

function obtenerFechaCompleta(fecha) {
  var fechaObj = new Date(fecha.toString())
  return fechaObj.getDate() + "/" + (fechaObj.getMonth() + 1).toLocaleString("en-US", {minimumIntegerDigits: 2,
    useGrouping: false}) + "/" + fechaObj.getFullYear() + " " + 
    fechaObj.getHours() + ":" + fechaObj.getMinutes()
}

function verificarScroll() {
  var scrollTop = $('#chat-messages').scrollTop();
  var scrollHeight = $('#chat-messages')[0].scrollHeight;
  if (scrollTop < scrollTopInicial) {
    $('#irAbajoIcon').show('swing');
  } else {
    $('#irAbajoIcon').hide('swing');
  }
}

function cerrarChat() {
  $("#vistasContainer").load("Partials/_Bienvenida")
}
