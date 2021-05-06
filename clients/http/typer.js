import axios from 'axios';

class MicroservicioTypers{
    
    async RegistrarNuevoTyper(nuevoTyper){
        let url = "http://localhost:3324/Typer/registrarTyper";

        return axios.post(url, nuevoTyper)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    
    async Login(infoLogin){
        let url = "http://localhost:3324/Typer/loginTyper";

        return axios.post(url, infoLogin)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async ObtenerInfoTyper(idTyper){
        let url = "http://localhost:3324/Typer/infoTyper";
        return axios.post(url, idTyper)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }


    async ObtenerCorreosTyper(idTyper){
        let url = "http://localhost:3324/Typer/obtenerCorreos";

        return axios.post(url, idTyper)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async ActualizarCorreo(infoDeActualizacion){
        let url = `http://localhost:3324/Typer/actualizarCorreo`;

        return axios.put(url, infoDeActualizacion)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})

    }

    async ActualizarInfoDeUsuario(nuevaInfo){
        let url = "http://localhost:3324/Typer/actualizarInfoTyper";

        return axios.put(url, nuevaInfo)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async ActualizarContraseniaTyper(nuevaContrasenia){
        let url = "http://localhost:3324/Typer/actualizarContrasenia";

        return axios.put(url, nuevaContrasenia)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async AgregarNuevoCorreo(nuevoCorreo){
        let url = "http://localhost:3324/Typer/agregarCorreo";

        return axios.post(url, nuevoCorreo)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async EliminarTyper(idTyper){
        let url = "http://localhost:3324/Typer/eliminarTyper";

        return axios.delete(url, {data: idTyper})
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }


    async ObtenerTyperPorId(idTyper){
        let url = "http://localhost:3324/Typer/infoTyper";

        let busqueda = {identificadorTyper: idTyper}
        return axios.post(url, busqueda)
        .then(response => {return response.data.data.result})
        .catch(error => {return error.response.data})
    }

}

let microservicioTypers = new MicroservicioTypers();
export { microservicioTypers }
