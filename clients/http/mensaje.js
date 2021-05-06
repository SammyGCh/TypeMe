import axios from 'axios';

class MicroservicioMensaje {
    async ObtenerGrupos(nombre, idGrupo)
    {
        let url = "http://localhost:3326/grupos/buscar?";

        let pa = false;
        if (nombre) 
        {
            url += "nombre=" + nombre;
            pa = true;
        }
        if (idGrupo >= 0) 
        {
            if (pa == true) {
                pa = false;
                url += "&";
            }
            url += "idGrupo=" + idGrupo;
        }

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ObtenerIntegrantesDeGrupo(idGrupo)
    {
        let url = "http://localhost:3326/grupos/integrantes/" + idGrupo;

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async CrearGrupo(nuevoGrupo)
    {
        let url = "http://localhost:3326/grupos/crearGrupo";

        return axios.post(url, nuevoGrupo)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async ActualizarGrupo(idGrupo, grupoActualizado)
    {
        let url = "http://localhost:3326/grupos/actualizar/" + idGrupo;

        return axios.put(url, grupoActualizado)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async AgregarIntegrantesAGrupo(idGrupo, nuevosIntegrantes)
    {
        let url = "http://localhost:3326/grupos/agregarIntegrantes/" + idGrupo;

        return axios.post(url, nuevosIntegrantes)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async SalirDeGrupo(idGrupo, idTyper)
    {
        let url = "http://localhost:3326/grupos/salir?idGrupo=" + idGrupo + "&idTyper=" + idTyper;

        return axios.put(url)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async EnviarMensaje(nuevoMensaje)
    {
        let url = "http://localhost:3326/mensajes/enviar";

        return axios.post(url, nuevoMensaje)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ObtenerMensajesDeGrupo(idGrupo)
    {
        let url = "http://localhost:3326/mensajes/obtener/" + idGrupo;

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}

let microservicioMensaje = new MicroservicioMensaje();

export { microservicioMensaje }