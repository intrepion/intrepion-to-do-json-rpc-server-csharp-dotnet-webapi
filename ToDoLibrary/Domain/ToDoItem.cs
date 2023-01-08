using System.Text.Json.Serialization;

namespace ToDoLibrary.Domain;

public class ToDoItem
{
    [JsonPropertyName("complete")]
    public bool? Complete { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    public static ToDoItem CreateToDoItem(string text) {
        text = text.Trim();

        if (string.IsNullOrEmpty(text)) {
            text = "make a to do list";
        }

        return new ToDoItem() {
            Complete = false,
            Text = text,
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
