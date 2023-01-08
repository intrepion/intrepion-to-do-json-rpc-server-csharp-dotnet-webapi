using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class NewToDoListResult
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
