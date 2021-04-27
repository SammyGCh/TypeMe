import express from 'express';
import typerRouter from './routes/typers.js';

const app = express();

const PORT = 4000;

app.use("/typers", typerRouter);

app.all("*", (req, res) => res.send({
    success: false,
    msg: "This route does not exist"}, 404));

app.listen(PORT, () => console.log(`Server running in port ${PORT}`));