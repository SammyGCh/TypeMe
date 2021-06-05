

function irAMiPerfil() {
    $("#vistasContainer").load("Partials/_MiPerfil")
}

function cerrarSesion() {
    $.ajax({
        type: 'POST',
        url: '/Login/CerrarSesion',
        contentType: 'application/json',
        success: function () {
            window.location.pathname = '/login'
        },
        error: function () {
          
        },
    });    
}