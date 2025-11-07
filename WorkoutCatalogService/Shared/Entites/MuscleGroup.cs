using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WorkoutCatalogService.Shared.Entites
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum MuscleGroup
    {
        [EnumMember(Value = "Full Body")]
        FullBody,

        [EnumMember(Value = "Chest")]
        Chest,

        [EnumMember(Value = "Arms")]
        Arms,

        [EnumMember(Value = "Shoulders")]
        Shoulders,

        [EnumMember(Value = "Back")]
        Back,

        [EnumMember(Value = "Legs")]
        Legs,

        [EnumMember(Value = "Stomach")]
        Stomach
    }
}
