using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Common
{
    public static class GenericError<T>
    {
        public static Response<T> ExceptionMessage(string customMessage = "Ha ocurrido un error procesando la solicitud.")
        {
            return new()
            {
                Errors = new List<Error>() { new() { Message = customMessage } }
            };
        }
        public static Response<T> UpdateElementNotFound()
        {
            return new()
            {
                Errors = new List<Error>() { new() { Message = "El elemento que esta intentando actualizar no existe." } }
            };
        }
        public static Response<T> AlreadyExists(string fieldName = "El registro")
        {
            return new()
            {
                Errors = new List<Error>()
            {
                new() { Message = $"{fieldName} que intenta registrar ya existe." }
            }
            };
        }
    }
}
