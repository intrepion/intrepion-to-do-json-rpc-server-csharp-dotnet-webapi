using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace ToDoWebApi.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("refresh_token_expiry_time")]
    public DateTime? RefreshTokenExpiryTime { get; set; }

    [JsonPropertyName("to_do_lists")]
    public ICollection<ToDoListEntity>? ToDoLists { get; set; }
}
