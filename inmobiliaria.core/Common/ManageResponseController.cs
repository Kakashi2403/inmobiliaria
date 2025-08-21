using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Common
{
    public class ManageResponseController : ControllerBase
    {
        public ActionResult<Response<T>> Evaluate<T>(Response<T> response)
        {
            return response.Errors != null && response.Errors.Any() ? BadRequest(response) : Ok(response);
        }
    }
}
