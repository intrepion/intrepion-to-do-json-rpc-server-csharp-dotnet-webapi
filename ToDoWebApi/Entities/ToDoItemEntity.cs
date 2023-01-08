using System.Text.Json.Serialization;
using ToDoLibrary.Domain;

namespace ToDoWebApi.Entities;

public class ToDoItemEntity : ToDoItem
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("to_do_list")]
    public ToDoListEntity? ToDoList { get; set; }
}
