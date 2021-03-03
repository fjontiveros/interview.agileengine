using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgileEngine.ImageGallerySearch.Helpers
{
    public class CustomJsonSerializer
    {
        public static string Serialize<TValue>(TValue value)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(value, jsonSerializerOptions);
        }

        public static TValue? Deserialize<TValue>(string json)
        {

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return JsonSerializer.Deserialize<TValue>(json, jsonSerializerOptions);
        }
    }
}
