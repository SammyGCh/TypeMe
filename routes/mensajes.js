import express from 'express';
import { microservicioMultimedia } from '../clients/http/multimedia.js';

const router = express.Router();

router.post("/registrarMultimedia", async (req, res) => {

    microservicioMultimedia.RegistrarMultimedia(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("mensajes/registrarMultimedia", error);
    })
})

router.get("/obtenerMultimedia", async (req, res) => {
    const { idMultimedia } = req.query;

    microservicioMultimedia.obtenerMultimedia(idMultimedia)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("mensajes/infoTyper", error);
    })

})

export default router;