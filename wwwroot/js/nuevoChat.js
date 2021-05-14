let contactosYaMostrados = false;
let contactosSeleccionados = []


function prep_modal() {
  $('.modal').each(function () {
    var element = this;
    var pages = $(this).find('.modal-split');

    if (pages.length != 0) {
      pages.hide();
      pages.eq(0).show();

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

      $(this).find('.modal-footer').append(atrasBoton).append(siguienteBoton);

      var page_track = 0;
      

      $(siguienteBoton).click(function () {
        this.blur();
        if (page_track == 0) {
            if ($("#nombreDeGrupo").val().length === 0) {
                $("#mensajeError").show();
            }
            else {
                buscarContactos()
                page_track++;
                pages.hide();
                pages.eq(page_track).show();
                $(atrasBoton).show();
                $("#mensajeError").hide();
                $(siguienteBoton).text('Crear chat');
            }
        }
        else if (page_track == 1) {
            alert("hola")
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

  if (contactosYaMostrados === false) {
    $.ajax({
      type: 'GET',
      url: '/Login/ObtenerTyperEnSesion',
      contentType: 'application/json',
      success: function (result) {
        if (result.status === true) {
          var idTyper = result.typer.idTyper;

          mostrarContactosDeTyper(idTyper);
        }
      },
      error: function () {
        alert('Ocurrió un al intentar conectarse al servidor');
      },
    });
  }

  function mostrarContactosDeTyper(idTyper) {
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
    
}

function manejarSeleccionDeContactos() {
  $('#listaDeContactos tbody').on('click', 'tr', function () {

    /**
     Si está seleccionado, se quita la clase de seleccion y se quita del arreglo de contactos seleccionados
     Si no está seleccionado, se agrega la clase de seleccion y se agrega a los contactos seleccionados
    */
    let id = $(this).find('div').find('p').text()

    if ($(this).find('div').hasClass('contacto-seleccionado')) {
      $(this).find('div').removeClass('contacto-seleccionado')
      contactosSeleccionados = eliminarContactoSeleccionado(contactosSeleccionados, id)
      console.log(contactosSeleccionados)
    }
    else {
      $(this).find('div').addClass('contacto-seleccionado');
      
      contactosSeleccionados.push({
        idTyper: id
      })
      console.log(contactosSeleccionados)
    }
    

    
  });
}

function eliminarContactoSeleccionado(arr, id) { 
    
  return arr.filter(function(ele){ 
      return ele.idTyper != id; 
  });
}