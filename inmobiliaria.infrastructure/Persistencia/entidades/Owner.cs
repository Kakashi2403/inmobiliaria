using System;
using System.Collections.Generic;

namespace inmobiliaria.infrastructure.Persistencia.entidades;

public partial class Owner
{
    public int IdOwner { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Photo { get; set; }

    public DateOnly? Birthday { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
