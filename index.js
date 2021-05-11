import express from 'express';
import typerRouter from './routes/typers.js';
import mensajeRouter from './routes/mensajes.js';
import cors from 'cors';

const app = express();
const PORT = 4000;

const allowedOrigins = [
    'http://localhost:3324',
    'http://localhost:3325',
    'http://localhost:3326',
    'http://localhost:3327',
    'http://localhost:5000',
    'https://localhost:5001'
]

var corsOptionsDelegate = function (req, callback) {
    var corsOptions;
    if (allowedOrigins.indexOf(req.header('Origin')) !== -1) {
        corsOptions = { origin: true }
    }
    else {
        corsOptions = { origin: false }
    }

    callback(null, corsOptions)
}

app.use(cors())

app.use(express.urlencoded({extended: true})); 
app.use(express.json());

app.use("/typers", cors(corsOptionsDelegate), typerRouter);
app.use("/mensajes", cors(corsOptionsDelegate), mensajeRouter);

app.all("*", cors(corsOptionsDelegate), (req, res) => res.status(404).send(
    {success: false, 
    msg: "This route does not exist"}));

app.listen(PORT, () => console.log(`Server running in port ${PORT}`));