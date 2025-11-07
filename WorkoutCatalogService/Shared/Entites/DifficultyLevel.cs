using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WorkoutCatalogService.Shared.Entites
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum DifficultyLevel
    {
        [EnumMember(Value = "Beginner")]
        Beginner,
        [EnumMember(Value = "Intermediate")]
        Intermediate,
        [EnumMember(Value = "Advanced")]
        Advanced
    }
}
