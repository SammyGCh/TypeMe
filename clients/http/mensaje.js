import axios from 'axios';

const MS_MENSAJES_URL = process.env.URL_MS_MENSAJES;

class MicroservicioMensaje {
    async ObtenerGrupos(nombre, idGrupo)
    {
        let url = MS_MENSAJES_URL + "/grupos/buscar?";

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
        let url = MS_MENSAJES_URL + "/grupos/integrantes/" + idGrupo;

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async CrearGrupo(nuevoGrupo)
    {
        let url = MS_MENSAJES_URL + "/grupos/crearGrupo";

        return axios.post(url, nuevoGrupo)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ActualizarGrupo(idGrupo, grupoActualizado)
    {
        let url = MS_MENSAJES_URL + "/grupos/actualizar/" + idGrupo;

        return axios.put(url, grupoActualizado)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async AgregarIntegrantesAGrupo(idGrupo, nuevosIntegrantes)
    {
        let url = MS_MENSAJES_URL + "/grupos/agregarIntegrantes/" + idGrupo;

        return axios.post(url, nuevosIntegrantes)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async SalirDeGrupo(idGrupo, idTyper)
    {
        let url = MS_MENSAJES_URL + "/grupos/salir?idGrupo=" + idGrupo + "&idTyper=" + idTyper;

        return axios.put(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async EnviarMensaje(nuevoMensaje)
    {
        let url = MS_MENSAJES_URL + "/mensajes/enviar";

        return axios.post(url, nuevoMensaje)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ObtenerMensajesDeGrupo(idGrupo)
    {
        let url = MS_MENSAJES_URL + "/mensajes/obtener/" + idGrupo;

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ObtenerMisGrupos(idTyper) {
        let url = MS_MENSAJES_URL + "/grupos/misGrupos/" + idTyper;

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})

    }
}

let microservicioMensaje = new MicroservicioMensaje();

export { microservicioMensaje }