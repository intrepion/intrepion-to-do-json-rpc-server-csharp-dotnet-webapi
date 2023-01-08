using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class NewToDoItemResult
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
