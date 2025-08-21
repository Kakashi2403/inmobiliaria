using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace inmobiliaria.core.Common
{
    public static class Json
    {
        private readonly static JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, options);
        }
        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
