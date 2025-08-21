using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.EntitiesCore
{
    public class OwnerDto
    {
        public int IdOwner { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Photo { get; set; }
        public DateOnly? Birthday { get; set; }
    }

    public class PropertyDto
    {
        public int IdProperty { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = default!;
        public int Year { get; set; }
        public int IdOwner { get; set; }

        // opcional para lecturas enriquecidas
        public string? OwnerName { get; set; }
    }

    public class PropertyImageDto
    {
        public int IdPropertyImage { get; set; }
        public int IdProperty { get; set; }
        public string File { get; set; } = default!;
        public bool Enabled { get; set; }
    }

    public class PropertyTraceDto
    {
        public int IdPropertyTrace { get; set; }
        public DateOnly DateSale { get; set; }
        public string Name { get; set; } = default!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public int IdProperty { get; set; }
    }
}
