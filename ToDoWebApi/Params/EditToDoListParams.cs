using System.Text.Json.Serialization;

namespace ToDoWebApi.Params;

public class EditToDoListParams
{
    [JsonPropertyName("complete")]
    public string? Complete { get; set; }

    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("visible")]
    public string? Visible { get; set; }
}
