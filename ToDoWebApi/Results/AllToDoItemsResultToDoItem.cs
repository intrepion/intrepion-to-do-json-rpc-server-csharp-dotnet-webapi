using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class AllToDoItemsResultToDoItem
{
    [JsonPropertyName("complete")]
    public bool? Complete { get; set; }

    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }
}
