using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSTypers.Utilidades
{
    public static class ConvertidorDeJson
    {
        private static JObject _resultado;
        private static JObject _dataObject;
        private static bool _status;

        private static readonly JsonSerializerSettings _jsonSerializerSetting = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Convierte en un JObject el resultado exitoso de una operación.
        /// </summary>
        /// <param name="message">Mensaje a mostrar en el resultado</param>
        /// <param name="data">Los datos del objeto a mostrar en el resultado. Puede ser un arreglo o no.</param>
        /// <returns>La serialización de un JObject</returns>
        public static JObject ConvertirResultadoExitoso(string message, object data)
        {
            JObject _dataObject = new JObject
            {
                { "message", message }
            };


            string dataJson = JsonConvert.SerializeObject(data, Formatting.Indented, _jsonSerializerSetting);

            if (EsTipoLista(data))
            {
                _dataObject["result"] = JArray.Parse(dataJson);
            }
            else
            {
                _dataObject["result"] = JObject.Parse(dataJson);
            }

            _status = true;
            JObject _resultado = new JObject
            {
                {"status", _status },
                {"data", _dataObject}
            };

            return _resultado;
        }

        /// <summary>
        /// Convierte en un JObject el resultado fallido de una operación.
        /// </summary>
        /// <param name="message">Mensaje de error a mostrar</param>
        /// <returns>La serialización de un JObjecto</returns>
        public static JObject ConvertirResultadoFallido(string message)
        {
            JObject _dataObject = new JObject
            {
                { "message", message },
                { "result", null }
            };

            _status = false;
            JObject _resultado = new JObject
            {
                { "status", _status },
                { "data", _dataObject }
            };

            return _resultado;
        }

        private static bool EsTipoLista(object data)
        {
            return (data.GetType().IsGenericType && data is IEnumerable<object>);
        }
    }
}