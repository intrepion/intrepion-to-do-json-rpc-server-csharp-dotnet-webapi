using System.Text.Json.Serialization;

namespace ToDoWebApi.Results;

public class LoginResultUser
{
    [JsonPropertyName("guid")]
    public Guid? Guid { get; set; }

    [JsonPropertyName("username")]
    public string? UserName { get; set; }
}
