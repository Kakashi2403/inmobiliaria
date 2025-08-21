using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Common
{
    public class Request<T> where T : class
    {
        [Required(ErrorMessage = "El cuerpo de la peticion es obligatorio")]
        public T Body { get; set; } = null!;

        [Required(ErrorMessage = "Las etiquetas de la peticion son obligatorias")]
        [MinLength(1, ErrorMessage = "La peticion debe tener al menos una etiqueta")]
        public Dictionary<string, string> Tags { get; set; } = null!;
    }
}
