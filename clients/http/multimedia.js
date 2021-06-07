import axios from 'axios';
import FormData from 'form-data';
import fs from 'fs';


const URL_MS_MULTIMEDIA = process.env.URL_MS_MULTIMEDIA
const URL_MULTIMEDIA_DIRECTA = process.env.URL_AMISTOSA;
class MicroservicioMultimedia{
    
    async RegistrarMultimedia(filePath, idTyper){
        let url = URL_MS_MULTIMEDIA + `/multimedia/registrarMultimedia?idTyper=${idTyper}`;

        var form = new FormData();
        form.append('file', fs.createReadStream(filePath));

        return axios({
            method: "post",
            url: url,
            data: form,
            headers: form.getHeaders()
          })
        .then(response => {return response.data})
        .catch(error => {console.error("Error en axios " + error);return error.response.data})
    }

    async obtenerMultimedia(idMultimedia){
        let url = URL_MULTIMEDIA_DIRECTA + `/multimedia/obtenerMultimedia?idMultimedia=${idMultimedia}`;
        return url;
    }

}

let microservicioMultimedia = new MicroservicioMultimedia();
export { microservicioMultimedia }