using System.Linq.Expressions;
using inmobiliaria.core.Common;
using inmobiliaria.core.EntitiesCore;
using inmobiliaria.core.Interfaces;
using inmobiliaria.infrastructure.Persistencia.entidades;

namespace inmobiliaria.infrastructure.Dependencias.Adapters;

public class InmobiliariaAdapter : IInmobiliariaAdapter
{
    private readonly IUnitOfWork _uow;

    public InmobiliariaAdapter(IUnitOfWork uow) => _uow = uow;

    // ---------------- Properties ----------------
    //public async Task<PagedList<PropertyDto>> GetPropertiesAsync(
    //    int pageNumber, int pageSize,
    //    string? name = null, string? address = null,
    //    decimal? minPrice = null, decimal? maxPrice = null,
    //    int? year = null, int? idOwner = null,
    //    bool includeRelations = false)
    //{
    //    var repo = _uow.Repository<Property>();
    //    Expression<Func<Property, bool>>? filter = BuildPropertyFilter(name, address, minPrice, maxPrice, year, idOwner);
    //    var includes = includeRelations ? "IdOwnerNavigation,PropertyImages,PropertyTraces" : string.Empty;

    //    var paged = await repo.GetPagedListAsync(
    //        pageNumber, pageSize,
    //        filter: filter,
    //        orderBy: q => q.OrderBy(p => p.Name).ThenBy(p => p.IdProperty),
    //        includeProperties: includes);

    //    // mapear PagedList<Property> -> PagedList<PropertyDto>
    //    var mapped = new List<PropertyDto>(paged.List!.ToList());
    //    //foreach (var p in paged.List!) mapped.Add(Map(p, includeRelations));

    //    return new PagedList<PropertyDto>
    //    { 
    //        List = mapped, 
    //        TotalPages = paged.TotalPages, 
    //        PageNumber = paged.PageNumber, 
    //        PageSize = paged.PageSize
    //    };
    //}

    public async Task<PagedList<PropertyDto>> GetPropertiesAsync(
    int pageNumber, int pageSize,
    string? name = null, string? address = null,
    decimal? minPrice = null, decimal? maxPrice = null,
    int? year = null, int? idOwner = null,
    bool includeRelations = false)
    {
        var repo = _uow.Repository<Property>();
        Expression<Func<Property, bool>>? filter =
            BuildPropertyFilter(name, address, minPrice, maxPrice, year, idOwner);

        var includes = includeRelations
            ? "IdOwnerNavigation,PropertyImages,PropertyTraces"
            : string.Empty;

        var paged = await repo.GetPagedListAsync(
            pageNumber, pageSize,
            filter: filter,
            orderBy: q => q.OrderBy(p => p.Name).ThenBy(p => p.IdProperty),
            includeProperties: includes);

        var mapped = paged.List!
                          .Select(p => Map(p, includeRelations))
                          .ToList();

        return new PagedList<PropertyDto>
        {
            List = mapped,
            TotalPages = paged.TotalPages,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize
            // si tu PagedList tiene TotalCount, asígnalo también:
            // TotalCount = paged.TotalCount
        };
    }


    public async Task<PropertyDto?> GetPropertyByIdAsync(int idProperty, bool includeRelations = false)
    {
        var includes = includeRelations ? "IdOwnerNavigation,PropertyImages,PropertyTraces" : string.Empty;
        var repo = _uow.Repository<Property>();
        var entity = (await repo.GetAsync(p => p.IdProperty == idProperty, includeProperties: includes)).FirstOrDefault();
        return entity is null ? null : Map(entity, includeRelations);
    }

    public async Task<int> CreatePropertyAsync(PropertyDto dto)
    {
        var entity = Map(dto);
        await _uow.Repository<Property>().AddAsync(entity);
        await _uow.SaveAsync();
        return entity.IdProperty;
    }

    public async Task UpdatePropertyAsync(PropertyDto dto)
    {
        var entity = Map(dto);
        _uow.Repository<Property>().Update(entity);
        await _uow.SaveAsync();
    }

    public async Task DeletePropertyAsync(int idProperty)
    {
        await _uow.Repository<Property>().DeleteAsync(idProperty);
        await _uow.SaveAsync();
    }

    // ---------------- Images ----------------
    public async Task<IEnumerable<PropertyImageDto>> GetImagesAsync(int idProperty)
    {
        var list = await _uow.Repository<PropertyImage>()
            .GetAsync(i => i.IdProperty == idProperty,
                      orderBy: q => q.OrderByDescending(i => i.Enabled).ThenBy(i => i.IdPropertyImage));

        return list.Select(Map);
    }

    public async Task<int> AddImageAsync(PropertyImageDto dto)
    {
        var entity = Map(dto);
        await _uow.Repository<PropertyImage>().AddAsync(entity);
        await _uow.SaveAsync();
        return entity.IdPropertyImage;
    }

    public async Task ToggleImageAsync(int idPropertyImage, bool enabled)
    {
        var repo = _uow.Repository<PropertyImage>();
        var img = (await repo.GetAsync(i => i.IdPropertyImage == idPropertyImage)).FirstOrDefault();
        if (img is null) return;
        img.Enabled = enabled;
        repo.Update(img);
        await _uow.SaveAsync();
    }

    // ---------------- Traces ----------------
    public async Task<IEnumerable<PropertyTraceDto>> GetTracesAsync(int idProperty)
    {
        var list = await _uow.Repository<PropertyTrace>()
            .GetAsync(t => t.IdProperty == idProperty,
                      orderBy: q => q.OrderByDescending(t => t.DateSale).ThenBy(t => t.IdPropertyTrace));

        return list.Select(Map);
    }

    public async Task<int> AddTraceAsync(PropertyTraceDto dto)
    {
        var entity = Map(dto);
        await _uow.Repository<PropertyTrace>().AddAsync(entity);
        await _uow.SaveAsync();
        return entity.IdPropertyTrace;
    }

    // ---------------- Owners ----------------
    public async Task<OwnerDto?> GetOwnerByIdAsync(int idOwner)
    {
        var e = (await _uow.Repository<Owner>().GetAsync(o => o.IdOwner == idOwner)).FirstOrDefault();
        return e is null ? null : Map(e);
    }

    public async Task<IEnumerable<OwnerDto>> GetOwnersAsync()
    {
        var list = await _uow.Repository<Owner>()
            .GetAsync(orderBy: q => q.OrderBy(o => o.Name));
        return list.Select(Map);
    }

    public async Task<int> CreateOwnerAsync(OwnerDto dto)
    {
        var e = Map(dto);
        await _uow.Repository<Owner>().AddAsync(e);
        await _uow.SaveAsync();
        return e.IdOwner;
    }

    public async Task UpdateOwnerAsync(OwnerDto dto)
    {
        var e = Map(dto);
        _uow.Repository<Owner>().Update(e);
        await _uow.SaveAsync();
    }

    public async Task DeleteOwnerAsync(int idOwner)
    {
        await _uow.Repository<Owner>().DeleteAsync(idOwner);
        await _uow.SaveAsync();
    }

    // --------------- helpers ----------------
    private static Expression<Func<Property, bool>>? BuildPropertyFilter(
        string? name, string? address, decimal? minPrice, decimal? maxPrice, int? year, int? idOwner)
    {
        Expression<Func<Property, bool>>? filter = null;

        void And(Expression<Func<Property, bool>> expr)
        {
            if (filter == null) filter = expr;
            else
            {
                var param = Expression.Parameter(typeof(Property));
                var body = Expression.AndAlso(
                    Expression.Invoke(filter, param),
                    Expression.Invoke(expr, param));
                filter = Expression.Lambda<Func<Property, bool>>(body, param);
            }
        }

        if (!string.IsNullOrWhiteSpace(name)) And(p => p.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(address)) And(p => p.Address.Contains(address));
        if (minPrice.HasValue) And(p => p.Price >= minPrice.Value);
        if (maxPrice.HasValue) And(p => p.Price <= maxPrice.Value);
        if (year.HasValue) And(p => p.Year == year.Value);
        if (idOwner.HasValue) And(p => p.IdOwner == idOwner.Value);

        return filter;
    }

    // --------- mappers entidad <-> dto ---------
    private static PropertyDto Map(Property e, bool includeRelations = false) => new()
    {
        IdProperty = e.IdProperty,
        Name = e.Name,
        Address = e.Address,
        Price = e.Price,
        CodeInternal = e.CodeInternal,
        Year = e.Year,
        IdOwner = e.IdOwner,
        OwnerName = includeRelations ? e.IdOwnerNavigation?.Name : null
    };
    private static Property Map(PropertyDto d) => new()
    {
        IdProperty = d.IdProperty,
        Name = d.Name,
        Address = d.Address,
        Price = d.Price,
        CodeInternal = d.CodeInternal,
        Year = (short)d.Year,
        IdOwner = d.IdOwner
    };

    private static OwnerDto Map(Owner e) => new()
    {
        IdOwner = e.IdOwner,
        Name = e.Name,
        Address = e.Address,
        Photo = e.Photo,
        Birthday = e.Birthday
    };
    private static Owner Map(OwnerDto d) => new()
    {
        IdOwner = d.IdOwner,
        Name = d.Name,
        Address = d.Address,
        Photo = d.Photo,
        Birthday = d.Birthday
    };

    private static PropertyImageDto Map(PropertyImage e) => new()
    {
        IdPropertyImage = e.IdPropertyImage,
        IdProperty = e.IdProperty,
        File = e.File,
        Enabled = e.Enabled
    };
    private static PropertyImage Map(PropertyImageDto d) => new()
    {
        IdPropertyImage = d.IdPropertyImage,
        IdProperty = d.IdProperty,
        File = d.File,
        Enabled = d.Enabled
    };

    private static PropertyTraceDto Map(PropertyTrace e) => new()
    {
        IdPropertyTrace = e.IdPropertyTrace,
        DateSale = e.DateSale,
        Name = e.Name,
        Value = e.Value,
        Tax = e.Tax,
        IdProperty = e.IdProperty
    };
    private static PropertyTrace Map(PropertyTraceDto d) => new()
    {
        IdPropertyTrace = d.IdPropertyTrace,
        DateSale = d.DateSale,
        Name = d.Name,
        Value = d.Value,
        Tax = d.Tax,
        IdProperty = d.IdProperty
    };
}
