using inmobiliaria.core.Common;
using inmobiliaria.core.EntitiesCore;

namespace inmobiliaria.core.Interfaces;

public interface IInmobiliariaAdapter
{
    // Properties
    Task<PagedList<PropertyDto>> GetPropertiesAsync(
        int pageNumber, int pageSize,
        string? name = null, string? address = null,
        decimal? minPrice = null, decimal? maxPrice = null,
        int? year = null, int? idOwner = null,
        bool includeRelations = false);

    Task<PropertyDto?> GetPropertyByIdAsync(int idProperty, bool includeRelations = false);
    Task<int> CreatePropertyAsync(PropertyDto dto);
    Task UpdatePropertyAsync(PropertyDto dto);
    Task DeletePropertyAsync(int idProperty);

    // Images
    Task<IEnumerable<PropertyImageDto>> GetImagesAsync(int idProperty);
    Task<int> AddImageAsync(PropertyImageDto dto);
    Task ToggleImageAsync(int idPropertyImage, bool enabled);

    // Traces
    Task<IEnumerable<PropertyTraceDto>> GetTracesAsync(int idProperty);
    Task<int> AddTraceAsync(PropertyTraceDto dto);

    // Owners
    Task<OwnerDto?> GetOwnerByIdAsync(int idOwner);
    Task<IEnumerable<OwnerDto>> GetOwnersAsync();
    Task<int> CreateOwnerAsync(OwnerDto dto);
    Task UpdateOwnerAsync(OwnerDto dto);
    Task DeleteOwnerAsync(int idOwner);
}
