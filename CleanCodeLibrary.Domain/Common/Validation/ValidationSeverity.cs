

using System.Text.Json.Serialization;

namespace CleanCodeLibrary.Domain.Common.Validation
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error
    }
}
