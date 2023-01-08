using System.Text.Json.Serialization;
using ToDoLibrary.Domain;

namespace ToDoWebApi.Entities;

public class ToDoListEntity : ToDoList
{
    [JsonPropertyName("application_user")]
    public ApplicationUser? ApplicationUser { get; set; }

    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("to_do_items")]
    public ICollection<ToDoItemEntity>? ToDoItems { get; set; }
}
