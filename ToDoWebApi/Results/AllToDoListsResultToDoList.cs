using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class AllToDoListsResultToDoList
{
    [JsonPropertyName("complete")]
    public bool? Complete { get; set; }

    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }
}
