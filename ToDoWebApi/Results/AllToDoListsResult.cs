using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class AllToDoListsResult
{
    [JsonPropertyName("to_do_lists")]
    public List<AllToDoListsResultToDoList>? ToDoLists { get; set; }
}
