let contactosYaMostrados = false;
let contactosSeleccionados = []
var idTyper;
var atrasBoton = document.createElement('button');
atrasBoton.setAttribute('type', 'button');
atrasBoton.setAttribute('class', 'btn btn-secondary btn-sm');
atrasBoton.setAttribute('style', 'display: none; width:150px;');
atrasBoton.innerHTML = 'Atrás';

var siguienteBoton = document.createElement('button');
siguienteBoton.setAttribute('type', 'button');
siguienteBoton.setAttribute('class', 'btn btn-secondary btn-sm');
siguienteBoton.setAttribute('style', 'width: 150px')
siguienteBoton.innerHTML = 'Siguiente';


function prep_modal() {
  $('.modal').each(function () {
    const paginaDatosGrupo = 0;
    const paginaSeleccionContactos = 1;
    var element = this;
    var pages = $(this).find('.modal-split');

    if (pages.length != 0) {
      pages.hide();
      pages.eq(0).show();

      $(this).find('.modal-footer').append(atrasBoton).append(siguienteBoton);

      var page_track = 0;
      

      $(siguienteBoton).click(function () {
        this.blur();
        if (page_track == paginaDatosGrupo) {
            if ($("#nombreDeGrupo").val().length === 0 || $("#descripcionDeGrupo").val().length === 0) {
                $("#mensajeError").show()
            }
            else {
                //buscarContactos()
                page_track++;
                pages.hide();
                pages.eq(page_track).show();
                $(atrasBoton).show();
                $("#mensajeError").hide();
                $(siguienteBoton).text('Crear chat');
                buscarContactos()
                console.log("entré aquí, al click")
            }
        }
        else if (page_track == paginaSeleccionContactos) {
            if(contactosSeleccionados.length === 0) {
              $("#mensajeErrorContactos").show()
            }
            else {
              crearGrupo()
            }
        }
      });

      $(atrasBoton).click(function () {
        if (page_track == 1) {
          $(atrasBoton).hide();
        }

        if (page_track == pages.length - 1) {
          $(siguienteBoton).text('Siguiente');
        }

        if (page_track > 0) {
          page_track--;

          pages.hide();
          pages.eq(page_track).show();
        }
      });
    }
  });
}

function buscarContactos() {
  console.log("entré al metodo 1")
  if (contactosYaMostrados === false) {
    $.ajax({
      type: 'GET',
      url: '/Login/ObtenerTyperEnSesion',
      contentType: 'application/json',
      success: function (result) {
        console.log(result)
        if (result.status === true) {
          idTyper = result.typer.idTyper;

          mostrarContactosDeTyper(idTyper);
        }
      },
      error: function () {
        alert('Ocurrió un al intentar conectarse al servidor');
      },
    });
  }
}

function mostrarContactosDeTyper(idTyper) {
  console.log("entré al metodo 2")
    $.ajax({
      type: 'GET',
      url: 'http://localhost:4000/typers/obtenerContactos/' + idTyper,
      dataType: 'json',
      async: true,
      contentType: 'application/json',
      success: function (response) {
        console.log(response);
        if (response.status === true) {
          var contactosDeTyper = response.result;

          contactosDeTyper.forEach((contacto) => {
            $('#listaDeContactos')
              .find('tbody')
              .append(
                $('<tr>').append(
                  $('<div>')
                    .addClass('contacto')
                    .append($('<span>').text(contacto.contacto.Username))
                    .append($('<p>').text(contacto.contacto.IdTyper)
                      .css('display', 'none')
                    )
                )
              );
          });
        }

        contactosYaMostrados = true;
      },
      error: function (response) {
        console.log(response);
      },
    });
}
    


function manejarSeleccionDeContactos() {
  $('#listaDeContactos tbody').on('click', 'tr', function () {
    let id = $(this).find('div').find('p').text()

    if ($(this).find('div').hasClass('contacto-seleccionado')) {
      
      $(this).find('div').removeClass('contacto-seleccionado')
      contactosSeleccionados = eliminarContactoSeleccionado(contactosSeleccionados, id)
    }
    else {
      $("#mensajeErrorContactos").hide()
      $(this).find('div').addClass('contacto-seleccionado');
      
      contactosSeleccionados.push({
        idTyper: id
      })
    }
    

    
  });
}

function eliminarContactoSeleccionado(arr, id) { 
    
  return arr.filter(function(ele){ 
      return ele.idTyper != id; 
  });
}

function crearGrupo() {
  contactosSeleccionados.push({
    idTyper: idTyper
  })
  let infoGrupo = {
    nombre: $("#nombreDeGrupo").val(),
    descripcion: $("#descripcionDeGrupo").val(),
    perteneces: contactosSeleccionados
  }

  $.ajax({
    type: 'POST',
    url: 'http://localhost:4000/mensajes/crearGrupo',
    dataType: 'json',
    async: true,
    contentType: 'application/json',
    data: JSON.stringify(infoGrupo),
    success: function (response) {
      if (response.status === true) {
          mostrarMensajeExito()

          $('#nuevoChatModal').modal('hide')
          $("#listaChats").load("Partials/ChatsAdmin")
      }
      else {
        mostrarMensajeError()
      }
    },
    error: function (response) {
      console.log(response);
      mostrarErrorServer()
    },
  });
}

function mostrarMensajeExito() {
  $("#modalDialogTitle").text("Grupo creado")
  $("#mensajeGrupoCreado").show()
  $("#dialog").modal("show")
}

function mostrarMensajeError() {
  $("#modalDialogTitle").text("Error")
  $("#mensajeErrorCreacionGrupo").show()
  $("#dialog").modal("show")
}

function mostrarErrorServer() {
  $("#modalDialogTitle").text("Error")
  $("#mensajeErrorServidor").show()
  $("#dialog").modal("show")
}