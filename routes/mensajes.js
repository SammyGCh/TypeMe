import express, { response } from 'express';
import { microservicioMultimedia } from '../clients/http/multimedia.js';
import { microservicioMensaje } from '../clients/http/mensaje.js';
import { microservicioTypers } from '../clients/http/typer.js';

const router = express.Router();

router.post("/registrarMultimedia", async (req, res) => {

    microservicioMultimedia.RegistrarMultimedia(req.body)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send(error);
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

router.get("/obtenerGrupos", async (req, res) => {
    const { nombre, idGrupo} = req.query;

    microservicioMensaje.ObtenerGrupos(nombre, idGrupo)
    .then(values => {
        res.send(values)
    })
    .catch(error => {
        res.send(error)
    })
})

router.get("/integrantesDeGrupo/:idGrupo", async (req, res) => {
    let idGrupo = req.params.idGrupo || -1;

    if (idGrupo > 0)
    {
        let response = await microservicioMensaje.ObtenerIntegrantesDeGrupo(idGrupo)

        if (response.status == true)
        {
            let typers = [];
            let integrantesDeGrupo = response.data.result;

            typers = await Promise.all(
                integrantesDeGrupo.map((integrante) =>
                    microservicioTypers.ObtenerTyperPorId(integrante.IdTyper)
                )
            )

            let resultado = {
                status: true,
                result: {
                    message: 'Integrantes encontrados',
                    result: typers,
                },
            };
        
            res.send(resultado)
        }
        else
        {
            res.send(response)
        }
    }
})

router.post("/crearGrupo", async (req, res) => {
    let nuevoGrupo = req.body;

    microservicioMensaje.CrearGrupo(nuevoGrupo)
    .then(response => {
        res.send(response)
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/actualizarGrupo/:idGrupo", async (req, res) => {
    let idGrupo = req.params.idGrupo || -1;
    let grupoActualizado = req.body;

    microservicioMensaje.ActualizarGrupo(idGrupo, grupoActualizado)
    .then(response => {
        res.send(response)
    })
    .catch(error => {
        res.send(error)
    })
})

router.post("/agregarIntegrantes/:idGrupo", async (req, res) => {
    let idGrupo = parseInt(req.params.idGrupo) || -1;
    let integrantesBody = req.body;

    let nuevosIntegrantes = []

    integrantesBody.forEach(integrante => {
        nuevosIntegrantes.push({
            idGrupo: idGrupo,
            idTyper: integrante.idTyper
        })
    });

    microservicioMensaje.AgregarIntegrantesAGrupo(idGrupo, nuevosIntegrantes)
    .then(response => {
        res.send(response)
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/salirDeGrupo", async (req, res) => {
    const idGrupo = req.query.idGrupo || -1;
    const idTyper = req.query.idTyper || "";
    console.log("idGrupo: ", idGrupo)
    console.log("idTyper: ", idTyper)

    microservicioMensaje.SalirDeGrupo(idGrupo, idTyper)
    .then(response => {
        res.send(response)
    })
    .catch(error => {
        res.send(error)
    })
    
})

router.post("/enviarMensaje", async (req, res) => {
    const nuevoMensajeBody = req.body

    if (nuevoMensajeBody.multimedia)
    {
        microservicioMultimedia.RegistrarMultimedia(nuevoMensajeBody.multimedia)
        .then(result => {

            if (result.status == true)
            {
                let nuevoMensaje = {
                    contenido: nuevoMensajeBody.contenido,
                    idGrupo: nuevoMensajeBody.idGrupo,
                    idTyper: nuevoMensajeBody.idTyper,
                    idMultimedia: result.data.result.IdMultimedia
                }
            
                microservicioMensaje.EnviarMensaje(nuevoMensaje)
                .then(() => {

                    microservicioTypers.ObtenerTyperPorId(nuevoMensaje.idTyper)
                    .then(typer => {
                        let respuesta = {
                            status: true,
                            data: {
                                message: "Mensaje enviado",
                                result: {
                                    contenido: nuevoMensajeBody.contenido,
                                    idGrupo: nuevoMensajeBody.idGrupo,
                                    typer: typer,
                                    multimedia: result.data.result
                                }
                            }
                        }

                        res.send(respuesta)
                    })
                    
                })
                .catch(error => {
                    res.send(error)
                })
            }
        })
        .catch(error => {
            res.send(error)
        })
        
    }
    else
    {
        let nuevoMensaje = {
            contenido: nuevoMensajeBody.contenido,
            idGrupo: nuevoMensajeBody.idGrupo,
            idTyper: nuevoMensajeBody.idTyper,
            idMultimedia: ""
        }
    
        microservicioMensaje.EnviarMensaje(nuevoMensaje)
        .then(() => {

            microservicioTypers.ObtenerTyperPorId(nuevoMensaje.idTyper)
            .then(typer => {
                let resultado = {
                    status: true,
                    data: {
                        message: "Mensaje enviado",
                        result: {
                            contenido: nuevoMensajeBody.contenido,
                            idGrupo: nuevoMensajeBody.idGrupo,
                            typer: typer,
                            multimedia: {}
                        }
                    }
                    
                }

                res.send(resultado)
            })
            
        })
        .catch(error => {
            res.send(error)
        })
    }
})

router.get("/obtenerMensajes/:idGrupo", async (req, res) => {
    let idGrupo = req.params.idGrupo || -1;

    let response = await microservicioMensaje.ObtenerMensajesDeGrupo(idGrupo);

    if (response.status == true) {
        Promise.all(
            response.data.result.map(async (mensaje) => {
                const typer = await microservicioTypers.ObtenerTyperPorId(mensaje.IdTyper);
                if (mensaje.IdMultimedia) {
                    return microservicioMultimedia
                        .obtenerMultimedia(mensaje.IdMultimedia)
                        .then((values) => {
                            return {
                                idMensaje: mensaje.IdMensaje,
                                contenido: mensaje.Contenido,
                                fecha: mensaje.Fecha,
                                hora: mensaje.Hora,
                                idGrupo: mensaje.IdGrupo,
                                typer: typer,
                                multimedia: values.data.result,
                            };
                        });
                } else {
                    return {
                        idMensaje: mensaje.IdMensaje,
                        contenido: mensaje.Contenido,
                        fecha: mensaje.Fecha,
                        hora: mensaje.Hora,
                        idGrupo: mensaje.IdGrupo,
                        typer: typer,
                        multimedia: {},
                    };
                }
                
            })
        ).then(resultado => res.send({
            status: true,
            data: {
                message: "Mensajes encontrados",
                result: resultado
            }
        }));   
    }
    else
    {
        res.send(response)
    }
})

export default router;