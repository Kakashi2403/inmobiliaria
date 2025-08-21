using inmobiliaria.core.Common;
using inmobiliaria.core.EntitiesCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.Interfaces
{
    public interface IInmobiliariaService
    {
        // Propiedades
        Task<Response<PagedList<PropertyDto>>> BuscarPropiedadesAsync(
            int page, int size,
            string? name = null, string? address = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            int? year = null, int? idOwner = null,
            bool includeRelations = false);

        Task<Response<PropertyDto>> ObtenerPropiedadAsync(int id, bool includeRelations = false);
        Task<Response<int>> CrearPropiedadAsync(PropertyDto dto);
        Task<Response<bool>> ActualizarPropiedadAsync(PropertyDto dto);
        Task<Response<bool>> EliminarPropiedadAsync(int id);

        // Imágenes
        Task<Response<IEnumerable<PropertyImageDto>>> ObtenerImagenesAsync(int idProperty);
        Task<Response<int>> AgregarImagenAsync(PropertyImageDto dto);
        Task<Response<bool>> CambiarEstadoImagenAsync(int idPropertyImage, bool enabled);

        // Trazas
        Task<Response<IEnumerable<PropertyTraceDto>>> ObtenerTrazasAsync(int idProperty);
        Task<Response<int>> AgregarTrazaAsync(PropertyTraceDto dto);

        // Dueños
        Task<Response<IEnumerable<OwnerDto>>> ListarOwnersAsync();
        Task<Response<OwnerDto>> ObtenerOwnerAsync(int idOwner);
        Task<Response<int>> CrearOwnerAsync(OwnerDto dto);
        Task<Response<bool>> ActualizarOwnerAsync(OwnerDto dto);
        Task<Response<bool>> EliminarOwnerAsync(int idOwner);
    }
}
