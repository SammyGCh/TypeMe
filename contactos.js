db.createUser(
    {
        user: "adminContactos",
        pwd: "proyecto2021",
        roles: [
            {
                role: "readWrite",
                db: "contactos"
            }
        ]
    }
);