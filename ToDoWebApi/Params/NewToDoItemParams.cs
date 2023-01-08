using System.Text.Json.Serialization;

namespace ToDoWebApi.Params;

public class NewToDoItemParams
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
