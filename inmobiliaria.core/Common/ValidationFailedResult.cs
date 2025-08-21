using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Common
{
    public static class ValidationFailedResult
    {
        public static BadRequestObjectResult ModelInvalidResult(ModelStateDictionary model)
        {
            var response = new Response<string>()
            {
                Errors = model.Values.SelectMany(v => v.Errors)
                .Select(x =>
                new Error()
                {
                    Message = x.ErrorMessage
                })
            };
            return new BadRequestObjectResult(response);
        }
    }
}
