import express from 'express';
import { microservicioTypers } from '../clients/http/typer.js';

const router = express.Router();

router.post("/registrarTyper", async (req, res) => {

    microservicioTypers.RegistrarNuevoTyper(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("typers/registrarTyper", error);
    })
})

router.post("/loginTyper", async (req, res) => {

    microservicioTypers.Login(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("typers/loginTyper", error);
    })

})

router.get("/infoTyper", async (req, res) => {

    microservicioTypers.ObtenerInfoTyper(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/infoTyper", error);
    })

})


router.post("/obtenerCorreos", async (req, res) => {

    microservicioTypers.ObtenerCorreosTyper(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/obtenerCorreos", error);
    })

})

router.put("/actualizarCorreo", async (req, res) => {

    microservicioTypers.ActualizarCorreo(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/actualizarCorreo", error);
    })

})


router.put("/actualizarInfoTyper", async (req, res) => {

    microservicioTypers.ActualizarInfoDeUsuario(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/actualizarInfoTyper", error);
    })

})

router.put("/actualizarContrasenia", async (req, res) => {

    microservicioTypers.ActualizarContraseniaTyper(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/actualizarContrasenia", error);
    })

})

router.post("/agregarCorreo", async (req, res) => {

    microservicioTypers.AgregarNuevoCorreo(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/agregarCorreo", error);
    })

})

router.delete("/eliminarTyper", async (req, res) => {

    microservicioTypers.EliminarTyper(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        ressend("typers/eliminarTyper", error);
    })

})


export default router;
