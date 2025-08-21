using System;
using System.Collections.Generic;

namespace inmobiliaria.infrastructure.Persistencia.entidades;

public partial class Property
{
    public int IdProperty { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Price { get; set; }

    public string CodeInternal { get; set; } = null!;

    public short Year { get; set; }

    public int IdOwner { get; set; }

    public virtual Owner IdOwnerNavigation { get; set; } = null!;

    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

    public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
}
