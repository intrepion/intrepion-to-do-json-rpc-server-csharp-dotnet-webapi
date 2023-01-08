using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace ToDoWebApi.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }
}
