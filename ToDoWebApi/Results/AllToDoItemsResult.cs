using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class AllToDoItemsResult
{
    [JsonPropertyName("to_do_items")]
    public List<AllToDoItemsResultToDoItem>? ToDoItems { get; set; }
}
