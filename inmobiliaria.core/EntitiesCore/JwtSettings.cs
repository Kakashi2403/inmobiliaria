using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmobiliaria.core.EntitiesCore
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = null!;
        public int ExpirationMinutes { get; set; }
    }
}
