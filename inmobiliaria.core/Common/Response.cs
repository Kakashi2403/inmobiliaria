using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Common
{
    public class Response<T>
    {
        public T? Body { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
        public IEnumerable<Warning>? Warnings { get; set; }
    }
}
