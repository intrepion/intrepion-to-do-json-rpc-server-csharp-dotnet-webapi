using System.Text.Json.Serialization;

namespace ToDoWebApi.Params;

public class NewToDoListParams
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
