import axios from 'axios';
import e, { response } from 'express';

export const login = async ({username, password}) => {
    let url = "http://localhost:5000/grupos/buscar";

    return axios.get(url, {
        params: {
            idGrupo: 1
        }
    })
    .then(response => {return response.data.data})
    .catch(error => {return error.response.data})
}