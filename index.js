import express from 'express';
import typerRouter from './routes/typers.js';
import mensajeRouter from './routes/mensajes.js';

const app = express();
const PORT = 4000;

app.use(express.urlencoded({extended: true})); 
app.use(express.json());

app.use("/typers", typerRouter);
app.use("/mensajes", mensajeRouter);

app.all("*", (req, res) => res.status(404).send(
    {success: false, 
    msg: "This route does not exist"}));

app.listen(PORT, () => console.log(`Server running in port ${PORT}`));