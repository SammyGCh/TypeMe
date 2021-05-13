import express, { response } from 'express';
import { microservicioTypers } from '../clients/http/typer.js';
import { microservicioContactos } from '../clients/http/contactos.js';

const router = express.Router();

router.post("/registrarTyper", async (req, res) => {

    microservicioTypers.RegistrarNuevoTyper(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        res.send("typers/registrarTyper", error);
    })
})

router.post("/loginTyper", async (req, res) => {
    
    microservicioTypers.Login(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        res.send("typers/loginTyper", error);
    })

})

router.post("/infoTyper", async (req, res) => {

    microservicioTypers.ObtenerInfoTyper(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/infoTyper", error);
    })

})


router.post("/obtenerCorreos", async (req, res) => {

    microservicioTypers.ObtenerCorreosTyper(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/obtenerCorreos", error);
    })

})

router.put("/actualizarCorreo", async (req, res) => {

    microservicioTypers.ActualizarCorreo(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/actualizarCorreo", error);
    })

})


router.put("/actualizarInfoTyper", async (req, res) => {

    microservicioTypers.ActualizarInfoDeUsuario(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/actualizarInfoTyper", error);
    })

})

router.put("/actualizarContrasenia", async (req, res) => {

    microservicioTypers.ActualizarContraseniaTyper(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/actualizarContrasenia", error);
    })

})

router.post("/agregarCorreo", async (req, res) => {

    microservicioTypers.AgregarNuevoCorreo(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/agregarCorreo", error);
    })

})

router.delete("/eliminarTyper", async (req, res) => {

    microservicioTypers.EliminarTyper(req.body)
    .then(values => {
        if(values.status == true){
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }else{
            let resultado = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            
            res.send(resultado);
        }
    })
    .catch(error => {
        ressend("typers/eliminarTyper", error);
    })

})

router.get("/obtenerContactos/:idTyper", async (req, res) => {
    let idTyper = req.params.idTyper || "";

    microservicioContactos.ObtenerContactos(idTyper)
    .then(response => {
        let resultado = {
            status: response.status,
            message: response.data.message,
            result: response.data.result
        }
        res.send(resultado)
    })
    .catch(error => {
        res.send(error)
    })
})

router.post("/agregarContacto", async (req, res) => {
    const infoContacto = req.body || {}

    microservicioTypers.ObtenerTyperPorUsuario(infoContacto.contacto)
    .then(values => {
        
        if(values.status === true) {
            let contactoBody = {
                idTyper: infoContacto.idTyper,
                idContacto: values.data.result.IdTyper
            }
            
            microservicioContactos.AgregarContacto(contactoBody)
            .then(response => {
                let resultado;
                if(response.status === true) {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: {
                            bloqueado: response.data.result.bloqueado,
                            esFavorito: response.data.result.esFavorito,
                            contacto: values.data.result
                        }
                    }
    
                    res.send(resultado)
                }
                else {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: { }
                    }
                    res.send(resultado)
                }
                
            })
            .catch(error => {
                res.send(error)
            })
        }
        else {
            let respuestaError = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            res.send(respuestaError)
        }
    })
    .catch(error => {
        res.send(error)
    })
})


router.put("/bloquearContacto", async (req, res) => {
    const infoContacto = req.body || {}

    microservicioTypers.ObtenerTyperPorUsuario(infoContacto.contacto)
    .then(values => {
        if (values.status === true) {
            let contactoBody = {
                idTyper: infoContacto.idTyper,
                idContacto: values.data.result.IdTyper
            }

            microservicioContactos.BloquearContacto(contactoBody)
            .then(response => {
                let resultado;
                if(response.status === true) {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: {
                            bloqueado: response.data.result.bloqueado,
                            esFavorito: response.data.result.esFavorito,
                            contacto: values.data.result
                        }
                    }
    
                    res.send(resultado)
                }
                else {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: { }
                    }
                    res.send(resultado)
                }
            })
            .catch(error => {
                res.send(error)
            })
        }
        else 
        {
            let respuestaError = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            res.send(respuestaError)
        }
    })
    .catch(error => {
        res.send(error)
    })
})


router.put("/desbloquearContacto", async (req, res) => {
    const infoContacto = req.body || {}

    microservicioTypers.ObtenerTyperPorUsuario(infoContacto.contacto)
    .then(values => {
        if (values.status === true) {
            let contactoBody = {
                idTyper: infoContacto.idTyper,
                idContacto: values.data.result.IdTyper
            }

            microservicioContactos.DesbloquearContacto(contactoBody)
            .then(response => {
                let resultado;
                if(response.status === true) {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: {
                            bloqueado: response.data.result.bloqueado,
                            esFavorito: response.data.result.esFavorito,
                            contacto: values.data.result
                        }
                    }
    
                    res.send(resultado)
                }
                else {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: { }
                    }
                    res.send(resultado)
                }
            })
            .catch(error => {
                res.send(error)
            })
        }
        else 
        {
            let respuestaError = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            res.send(respuestaError)
        }
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/agregarContactoAFavorito", async (req, res) => {
    const infoContacto = req.body || {}

    microservicioTypers.ObtenerTyperPorUsuario(infoContacto.contacto)
    .then(values => {
        if (values.status === true) {
            let contactoBody = {
                idTyper: infoContacto.idTyper,
                idContacto: values.data.result.IdTyper
            }

            microservicioContactos.AgregarAFavorito(contactoBody)
            .then(response => {
                let resultado;
                if(response.status === true) {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: {
                            bloqueado: response.data.result.bloqueado,
                            esFavorito: response.data.result.esFavorito,
                            contacto: values.data.result
                        }
                    }
    
                    res.send(resultado)
                }
                else {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: { }
                    }
                    res.send(resultado)
                }
            })
            .catch(error => {
                res.send(error)
            })
        }
        else 
        {
            let respuestaError = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            res.send(respuestaError)
        }
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/quitarContactoFavorito", async (req, res) => {
    const infoContacto = req.body || {}

    microservicioTypers.ObtenerTyperPorUsuario(infoContacto.contacto)
    .then(values => {
        if (values.status === true) {
            let contactoBody = {
                idTyper: infoContacto.idTyper,
                idContacto: values.data.result.IdTyper
            }

            microservicioContactos.QuitarFavorito(contactoBody)
            .then(response => {
                let resultado;
                if(response.status === true) {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: {
                            bloqueado: response.data.result.bloqueado,
                            esFavorito: response.data.result.esFavorito,
                            contacto: values.data.result
                        }
                    }
    
                    res.send(resultado)
                }
                else {
                    resultado = {
                        status: response.status,
                        message: response.data.message,
                        result: { }
                    }
                    res.send(resultado)
                }
            })
            .catch(error => {
                res.send(error)
            })
        }
        else 
        {
            let respuestaError = {
                status: values.status,
                message: values.data.message,
                result: values.data.result
            }
            res.send(respuestaError)
        }
    })
    .catch(error => {
        res.send(error)
    })
})

router.put("/eliminarContacto", async (req, res) => {
    const infoContacto = req.body || {}

    microservicioContactos.EliminarContacto(infoContacto)
    .then(response => {
        if (response.status == true)
        {
            let idTyper = response.data.result.idContacto
            microservicioTypers.ObtenerTyperPorId(idTyper)
            .then(typer => {
                let resultado = {
                    status: response.status,
                    message: response.data.message,
                    result: {
                        bloqueado: response.data.result.bloqueado,
                        esFavorito: response.data.result.esFavorito,
                        contacto: typer
                    }
                }

                res.send(resultado)
            })
        }
        else {
            res.send(response)
        }
    })
    .catch(error => {
        res.send(error)
    })
})

export default router;
