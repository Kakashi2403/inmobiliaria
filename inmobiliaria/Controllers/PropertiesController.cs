using inmobiliaria.application.Service;
using inmobiliaria.core.Common;
using inmobiliaria.core.EntitiesCore;
using inmobiliaria.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController(IInmobiliariaService inmobiliaria) : ManageResponseController
    {
        private readonly IInmobiliariaService _svc = inmobiliaria;

        [HttpGet("ListProperty")]
        [ProducesResponseType(typeof(Response<PagedList<PropertyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<PagedList<PropertyDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<PagedList<PropertyDto>>>> Get([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var resp = await _svc.BuscarPropiedadesAsync(page, size, includeRelations: true);
            return Evaluate<PagedList<PropertyDto>>(resp);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<int>>> Post(PropertyDto dto)
        {
            var resp = await _svc.CrearPropiedadAsync(dto);
            return Evaluate<int>(resp);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Response<PropertyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<PropertyDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response<PropertyDto>>> GetById(int id)
        {
            var resp = await _svc.ObtenerPropiedadAsync(id, includeRelations: true);
            return Evaluate<PropertyDto>(resp);
        }
    }
}
