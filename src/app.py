from flask import Flask, request, jsonify, Response
from flask_pymongo import PyMongo
from bson import json_util
import os

app = Flask(__name__)

app.config['MONGO_URI'] = os.environ['MONGO_URI']
app.config.from_pyfile('../config.cfg')

mongo = PyMongo(app)


@app.route('/obtenerContactos/<idTyper>', methods=['GET'])
def obtener_contactos_de_typer(idTyper):
    contactos_buscados = mongo.db.contacta.find_one({'idTyper': idTyper}, {'_id': 0, 'contactos': 1})

    if contactos_buscados:
        resultado = convertirResultadoExitoso("Contactos encontrados", contactos_buscados["contactos"])
    else:
        resultado = convertirResultadoFallido("Contactos no encontrados")
    
    return resultado

@app.route('/agregar', methods=['POST'])
def agregar_contacto():
    idTyper = request.json['idTyper']
    idContacto = request.json['idContacto']

    if idTyper and idContacto:

        existeTyper = mongo.db.contacta.count_documents({'idTyper': idTyper}) > 0

        nuevo_contacto = {}
        if existeTyper:
            ya_tiene_contacto_agregado = mongo.db.contacta.find_one({'contactos.idContacto': idContacto})

            if ya_tiene_contacto_agregado:
                resultado = convertirResultadoFallido("El contacto ya se encuentra agrgeado")

                return resultado
            else:
                nuevo_contacto = {
                    'idContacto': idContacto,
                    'bloqueado': False,
                    'esFavorito': False
                }

                mongo.db.contacta.update_one({'idTyper': idTyper}, {'$push': {'contactos': nuevo_contacto}})
        else:
            nuevo_contacto = agregar_primer_contacto(idTyper, idContacto)

        resultado = convertirResultadoExitoso("Contacto agregado", nuevo_contacto)
    else:
        resultado = convertirResultadoFallido("Contacto no agregado")
    
    return resultado

@app.route('/bloquear', methods=['PUT'])
def bloquear_contacto():
    idTyper = request.json['idTyper']
    idContacto = request.json['idContacto']

    if idTyper and idContacto:
        se_bloqueo = mongo.db.contacta.update_one({'idTyper': idTyper, 'contactos.idContacto': idContacto},
        {'$set': {'contactos.$.bloqueado': True}})

        if se_bloqueo.matched_count > 0:
            contacto_bloqueado = mongo.db.contacta.find_one({'idTyper': idTyper, 'contactos.idContacto': idContacto}, 
            {'_id': 0, 'contactos': {'$elemMatch': {'idContacto': idContacto}}})
            resultado = convertirResultadoExitoso("Contacto bloqueado", contacto_bloqueado["contactos"][0])
        else:
            resultado = convertirResultadoFallido("No se pudo bloquear al contacto")
    else:
        resultado = convertirResultadoFallido("El idTyper y/o idContacto no fueron especificados")


    return resultado

@app.route('/desbloquear', methods=['PUT'])
def desbloquear_contacto():
    idTyper = request.json['idTyper']
    idContacto = request.json['idContacto']

    if idTyper and idContacto:
        se_desbloqueo = mongo.db.contacta.update_one({'idTyper': idTyper, 'contactos.idContacto': idContacto},
        {'$set': {'contactos.$.bloqueado': False}})

        if se_desbloqueo.matched_count > 0:
            contacto_desbloqueado = mongo.db.contacta.find_one({'idTyper': idTyper, 'contactos.idContacto': idContacto}, 
            {'_id': 0, 'contactos': {'$elemMatch': {'idContacto': idContacto}}})
            resultado = convertirResultadoExitoso("Contacto desbloqueado", contacto_desbloqueado["contactos"][0])
        else:
            resultado = convertirResultadoFallido("No se pudo desbloquear al contacto")
    else:
        resultado = convertirResultadoFallido("El idTyper y/o idContacto no fueron especificados")


    return resultado

@app.route('/agregarAFavorito', methods=['PUT'])
def agregar_favorito():
    idTyper = request.json['idTyper']
    idContacto = request.json['idContacto']

    if idTyper and idContacto:
        seAgregoAFavorito = mongo.db.contacta.update_one({'idTyper': idTyper, 'contactos.idContacto': idContacto},
        {'$set': {'contactos.$.esFavorito': True}})

        if seAgregoAFavorito.matched_count > 0:
            contacto_favorito = mongo.db.contacta.find_one({'idTyper': idTyper, 'contactos.idContacto': idContacto}, 
            {'_id': 0, 'contactos': {'$elemMatch': {'idContacto': idContacto}}})
            resultado = convertirResultadoExitoso("Contacto agregado a favorito", contacto_favorito["contactos"][0])
        else:
            resultado = convertirResultadoFallido("No se pudo agregar a favoritos el contacto")
    else:
        resultado = convertirResultadoFallido("El idTyper y/o idContacto no fueron especificados")


    return resultado

@app.route('/quitarFavorito', methods=['PUT'])
def quitar_favorito():
    idTyper = request.json['idTyper']
    idContacto = request.json['idContacto']

    if idTyper and idContacto:
        seQuitoFavorito = mongo.db.contacta.update_one({'idTyper': idTyper, 'contactos.idContacto': idContacto},
        {'$set': {'contactos.$.esFavorito': False}})

        if seQuitoFavorito.matched_count > 0:
            contacto = mongo.db.contacta.find_one({'idTyper': idTyper, 'contactos.idContacto': idContacto}, 
            {'_id': 0, 'contactos': {'$elemMatch': {'idContacto': idContacto}}})
            resultado = convertirResultadoExitoso("Contacto eliminado de favorito", contacto["contactos"][0])
        else:
            resultado = convertirResultadoFallido("No se pudo eliminar de favoritos el contacto")
    else:
        resultado = convertirResultadoFallido("El idTyper y/o idContacto no fueron especificados")


    return resultado

@app.route('/eliminarContacto', methods=['PUT'])
def eliminar_contacto():
    idTyper = request.json['idTyper']
    idContacto = request.json['idContacto']

    if idTyper and idContacto:

        contacto = mongo.db.contacta.find_one({'idTyper': idTyper, 'contactos.idContacto': idContacto})

        if not contacto:
            resultado = convertirResultadoFallido("No existe el contacto específicado")
            return resultado
        
        eliminado = mongo.db.contacta.delete_one({'idTyper': idTyper, 'contactos.idContacto': idContacto})
        if eliminado.deleted_count > 0:
            resultado = convertirResultadoExitoso("Contacto eliminado", contacto["contactos"][0])
        else:
            resultado = convertirResultadoFallido("No se pudo eliminar el contacto")
    else:
        resultado = convertirResultadoFallido("El idTyper y/o idContacto no fueron especificados")

    return resultado


def agregar_primer_contacto(idTyper, idContacto):
    primer_contacto = {
        'idContacto': idContacto,
        'bloqueado': False,
        'esFavorito': False
    }

    mongo.db.contacta.insert_one({
        'idTyper': idTyper,
        'contactos': [
            primer_contacto
        ]
    })
    return primer_contacto

@app.errorhandler(404)
def not_found(error=None):
    respuesta = jsonify({
        'status': 404,
        'data': {
            'message': 'Recurso no encontrado: ' + request.url,
            'result': []
        }
    })
    
    respuesta.status_code = 404

    return respuesta


def convertirResultadoExitoso(message, data):

    resultadoJson = jsonify({
        'status': True,
        'data': {
            'message': message,
            'result': data
        }
    })

    return resultadoJson

def convertirResultadoFallido(message):
    resultado = jsonify({
        'status': False,
        'data': {
            'message': message,
            'result': []
        }
    })

    return resultado

if __name__ == "__main__":
    app.run()