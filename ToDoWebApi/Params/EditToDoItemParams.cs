using System.Text.Json.Serialization;

namespace ToDoWebApi.Params;

public class EditToDoItemParams
{
    [JsonPropertyName("complete")]
    public string? Complete { get; set; }

    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("visible")]
    public string? Visible { get; set; }
}
