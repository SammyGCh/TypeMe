import axios from 'axios';

class MicroservicioMultimedia{
    
    async RegistrarMultimedia(nuevaMultimedia){
        let url = "http://localhost:3325/Multimedia/registrarMultimedia";

        return axios.post(url, nuevaMultimedia)
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

    async obtenerMultimedia(idMultimedia){
        let url = "http://localhost:3325/Multimedia/obtenerMultimedia";

        return axios.get(url,  {
            params: {
                idMultimedia: idMultimedia
            }
        })
        .then(response => {return response.data.data})
        .catch(error => {return error.response.data})
    }

}

let microservicioMultimedia = new MicroservicioMultimedia();
export { microservicioMultimedia }