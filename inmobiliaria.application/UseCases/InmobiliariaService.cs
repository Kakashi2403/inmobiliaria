
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inmobiliaria.core.Common;
using inmobiliaria.core.EntitiesCore;
using inmobiliaria.core.Interfaces;

namespace inmobiliaria.application.Service
{
    public class InmobiliariaService : IInmobiliariaService
    {
        private readonly IInmobiliariaAdapter _adapter;

        public InmobiliariaService(IInmobiliariaAdapter adapter)
        {
            _adapter = adapter;
        }

        private static Response<T> Ok<T>(T body) => new Response<T> { Body = body };

        // --------- Casos de uso ---------

        public async Task<Response<PagedList<PropertyDto>>> BuscarPropiedadesAsync(
            int page, int size,
            string? name = null, string? address = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            int? year = null, int? idOwner = null,
            bool includeRelations = false)
        {
            var data = await _adapter.GetPropertiesAsync(page, size, name, address, minPrice, maxPrice, year, idOwner, includeRelations);
            return Ok(data);
        }

        public async Task<Response<PropertyDto>> ObtenerPropiedadAsync(int id, bool includeRelations = false)
            => Ok(await _adapter.GetPropertyByIdAsync(id, includeRelations));

        public async Task<Response<int>> CrearPropiedadAsync(PropertyDto dto)
        {
            if (dto.Price < 0) throw new ArgumentException("El precio no puede ser negativo.");
            var id = await _adapter.CreatePropertyAsync(dto);
            return Ok(id);
        }

        public async Task<Response<bool>> ActualizarPropiedadAsync(PropertyDto dto)
        {
            await _adapter.UpdatePropertyAsync(dto);
            return Ok(true);
        }

        public async Task<Response<bool>> EliminarPropiedadAsync(int id)
        {
            await _adapter.DeletePropertyAsync(id);
            return Ok(true);
        }

        public async Task<Response<IEnumerable<PropertyImageDto>>> ObtenerImagenesAsync(int idProperty)
            => Ok(await _adapter.GetImagesAsync(idProperty));

        public async Task<Response<int>> AgregarImagenAsync(PropertyImageDto dto)
            => Ok(await _adapter.AddImageAsync(dto));

        public async Task<Response<bool>> CambiarEstadoImagenAsync(int idPropertyImage, bool enabled)
        {
            await _adapter.ToggleImageAsync(idPropertyImage, enabled);
            return Ok(true);
        }

        public async Task<Response<IEnumerable<PropertyTraceDto>>> ObtenerTrazasAsync(int idProperty)
            => Ok(await _adapter.GetTracesAsync(idProperty));

        public async Task<Response<int>> AgregarTrazaAsync(PropertyTraceDto dto)
            => Ok(await _adapter.AddTraceAsync(dto));

        public async Task<Response<IEnumerable<OwnerDto>>> ListarOwnersAsync()
            => Ok(await _adapter.GetOwnersAsync());

        public async Task<Response<OwnerDto>> ObtenerOwnerAsync(int idOwner)
            => Ok(await _adapter.GetOwnerByIdAsync(idOwner));

        public async Task<Response<int>> CrearOwnerAsync(OwnerDto dto)
            => Ok(await _adapter.CreateOwnerAsync(dto));

        public async Task<Response<bool>> ActualizarOwnerAsync(OwnerDto dto)
        {
            await _adapter.UpdateOwnerAsync(dto);
            return Ok(true);
        }

        public async Task<Response<bool>> EliminarOwnerAsync(int idOwner)
        {
            await _adapter.DeleteOwnerAsync(idOwner);
            return Ok(true);
        }
    }
}
