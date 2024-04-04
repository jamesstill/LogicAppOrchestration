using System.Text.Json;

namespace TargetApi.Repository
{
    public static class SerializerOptions
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }
    }
}
