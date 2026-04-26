using Newtonsoft.Json;

namespace TowerOblivion.Infrastructure.Persistence
{
    public static class JsonSerializationHelper
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };

        public static string ToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
