import express from 'express';
import { login } from '../clients/http/typer.js';

const router = express.Router();

router.get("/loginTyper", async (req, res) => {
    const { username, password } = req.query;

    login(username, password)
    .then(values => {
        res.send(values);
    })
    .catch(error => {
        res.send("typers/loginTyper", error);
    })
})


export default router;