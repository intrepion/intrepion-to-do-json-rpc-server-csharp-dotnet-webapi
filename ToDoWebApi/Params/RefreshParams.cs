using System.Text.Json.Serialization;

namespace ToDoWebApi.Params;

public class RefreshParams
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}
