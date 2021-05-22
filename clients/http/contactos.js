import axios from 'axios';

const CONTACTOS_URL = process.env.URL_MS_CONTACTOS;

class MicroservicioContactos {
    async ObtenerContactos(idTyper) {
        let url = CONTACTOS_URL + "/obtenerContactos/" + idTyper

        return axios.get(url)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async AgregarContacto(infoContacto) {
        let url = CONTACTOS_URL + "/agregar"

        return axios.post(url, infoContacto)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async BloquearContacto(infoContacto) {
        let url = CONTACTOS_URL + "/bloquear"

        return axios.put(url, infoContacto)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async DesbloquearContacto(infoContacto) {
        let url = CONTACTOS_URL + "/desbloquear"

        return axios.put(url, infoContacto)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async AgregarAFavorito(infoContacto) {
        let url = CONTACTOS_URL + "/agregarAFavorito"

        return axios.put(url, infoContacto)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async QuitarFavorito(infoContacto) {
        let url = CONTACTOS_URL + "/quitarFavorito"

        return axios.put(url, infoContacto)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async EliminarContacto(infoContacto) {
        let url = CONTACTOS_URL + "/eliminarContacto"

        return axios.put(url, infoContacto)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}

let microservicioContactos = new MicroservicioContactos();

export { microservicioContactos }