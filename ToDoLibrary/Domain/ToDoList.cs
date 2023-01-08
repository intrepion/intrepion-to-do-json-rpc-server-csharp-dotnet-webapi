using System.Text.Json.Serialization;

namespace ToDoLibrary.Domain;

public class ToDoList
{
    [JsonPropertyName("complete")]
    public bool? Complete { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    public static ToDoList CreateToDoList(string title) {
        title = title.Trim();

        if (string.IsNullOrEmpty(title)) {
            title = "shopping list";
        }

        return new ToDoList {
            Complete = false,
            Title = title,
            Visible = true,
        };
    }

    public void MakeComplete() {
        Complete = true;
    }

    public void MakeHidden() {
        Visible = false;
    }

    public void MakeIncomplete() {
        Complete = false;
    }

    public void MakeVisible() {
        Visible = true;
    }    
}
