const chatBtn = document.getElementById("chat")

chatBtn.addEventListener("click", () => {
    var idGrupo = $("#idGrupo").text()
    var nombreGrupo = $("#nombreGrupo").text()
    
    alert("Id: " + idGrupo + " nombre: " + nombreGrupo);
    //$("#vistasContainer").load("Partials/_MiPerfil")
});