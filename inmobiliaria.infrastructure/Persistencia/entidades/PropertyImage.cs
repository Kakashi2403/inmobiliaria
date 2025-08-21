using System;
using System.Collections.Generic;

namespace inmobiliaria.infrastructure.Persistencia.entidades;

public partial class PropertyImage
{
    public int IdPropertyImage { get; set; }

    public int IdProperty { get; set; }

    public string File { get; set; } = null!;

    public bool Enabled { get; set; }

    public virtual Property IdPropertyNavigation { get; set; } = null!;
}
