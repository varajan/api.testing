using System.Text.Json.Serialization;

namespace api.testing.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        Actor = 0,
        Manager = 1,
        Producer = 2
    }
}
