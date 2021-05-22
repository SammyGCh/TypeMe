import axios from 'axios';

const URL_MS_MULTIMEDIA = process.env.URL_MS_MULTIMEDIA
class MicroservicioMultimedia{
    
    async RegistrarMultimedia(nuevaMultimedia){
        let url = URL_MS_MULTIMEDIA + "/Multimedia/registrarMultimedia";

        return axios.post(url, nuevaMultimedia)
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

    async obtenerMultimedia(idMultimedia){
        let url = URL_MS_MULTIMEDIA + "/Multimedia/obtenerMultimedia";

        return axios.get(url,  {
            params: {
                idMultimedia: idMultimedia
            }
        })
        .then(response => {return response.data})
        .catch(error => {return error.response.data})
    }

}

let microservicioMultimedia = new MicroservicioMultimedia();
export { microservicioMultimedia }