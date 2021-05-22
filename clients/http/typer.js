import axios from 'axios';

const URL_MS_TYPERS = process.env.URL_MS_TYPERS;

class MicroservicioTypers{
    
    async RegistrarNuevoTyper(nuevoTyper){
        let url = URL_MS_TYPERS + "/Typer/registrarTyper";

        return axios.post(url, nuevoTyper)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    
    async Login(infoLogin){
        let url = URL_MS_TYPERS + "/Typer/loginTyper";

        return axios.post(url, infoLogin)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ObtenerInfoTyper(idTyper){
        let url = URL_MS_TYPERS + "/Typer/infoTyper";
        return axios.post(url, idTyper)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }


    async ObtenerCorreosTyper(idTyper){
        let url = URL_MS_TYPERS + "/Typer/obtenerCorreos";

        return axios.post(url, idTyper)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ActualizarCorreo(infoDeActualizacion){
        let url = URL_MS_TYPERS + `/Typer/actualizarCorreo`;

        return axios.put(url, infoDeActualizacion)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})

    }

    async ActualizarInfoDeUsuario(nuevaInfo){
        let url = URL_MS_TYPERS + "/Typer/actualizarInfoTyper";

        return axios.put(url, nuevaInfo)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async ActualizarContraseniaTyper(nuevaContrasenia){
        let url = URL_MS_TYPERS + "/Typer/actualizarContrasenia";

        return axios.put(url, nuevaContrasenia)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async AgregarNuevoCorreo(nuevoCorreo){
        let url = URL_MS_TYPERS + "/Typer/agregarCorreo";

        return axios.post(url, nuevoCorreo)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async EliminarTyper(idTyper){
        let url = URL_MS_TYPERS + "/Typer/eliminarTyper";

        return axios.delete(url, {data: idTyper})
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }


    async ObtenerTyperPorId(idTyper){
        let url = URL_MS_TYPERS + "/Typer/infoTyper";

        let busqueda = {identificadorTyper: idTyper, modificadorDeMetodo: "id"}
        return axios.post(url, busqueda)
        .then(response => {return response.data.data.result})
        .catch(error => {return error.response.data})
    }

    async ObtenerTyperPorUsuario(username){
        let url = URL_MS_TYPERS + "/Typer/infoTyper";

        let busqueda = {identificadorTyper: username, modificadorDeMetodo: "usuario"}
        return axios.post(url, busqueda)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }
}

let microservicioTypers = new MicroservicioTypers();
export { microservicioTypers }
