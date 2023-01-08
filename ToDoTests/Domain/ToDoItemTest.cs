using ToDoLibrary.Domain;

namespace ToDoTests.Domain;

public class ToDoItemTest
{
    [Theory]
    [InlineData("", "make a to do list")]
    [InlineData("do the dishes", "do the dishes")]
    [InlineData("take out the trash", "take out the trash")]
    public void TestToDoItem_CreateToDoItem_HappyPath(string actualText, string expectedText)
    {
        // Arrange
        var expected = new ToDoItem {
            Complete = false,
            Text = expectedText,
            Visible = true,
        };

        // Act
        var actual = ToDoItem.CreateToDoItem(actualText);

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Theory]
    [InlineData("   do the dishes", "do the dishes")]
    [InlineData("take out the trash   ", "take out the trash")]
    public void TestToDoItem_CreateToDoItem_TrimsText(string actualText, string expectedText)
    {
        // Arrange
        var expected = new ToDoItem {
            Complete = false,
            Visible = true,
            Text = expectedText,
        };

        // Act
        var actual = ToDoItem.CreateToDoItem(actualText);

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoItem_MakeComplete() {
        // Arrange
        var expected = new ToDoItem {
            Complete = true,
            Visible = true,
            Text = "do the dishes",
        };

        // Act
        var actual = ToDoItem.CreateToDoItem("do the dishes");
        actual.MakeComplete();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoItem_MakeHidden() {
        // Arrange
        var expected = new ToDoItem {
            Complete = false,
            Visible = false,
            Text = "do the dishes",
        };

        // Act
        var actual = ToDoItem.CreateToDoItem("do the dishes");
        actual.MakeHidden();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoItem_Complete_ThenMakeIncomplete() {
        // Arrange
        var expected = new ToDoItem {
            Complete = false,
            Visible = true,
            Text = "do the dishes",
        };

        // Act
        var actual = ToDoItem.CreateToDoItem("do the dishes");
        actual.MakeComplete();
        actual.MakeIncomplete();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoItem_MakeHidden_ThenMakeVisible() {
        // Arrange
        var expected = new ToDoItem {
            Complete = false,
            Visible = true,
            Text = "do the dishes",
        };

        // Act
        var actual = ToDoItem.CreateToDoItem("do the dishes");
        actual.MakeHidden();
        actual.MakeVisible();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Text, actual.Text);
        Assert.Equal(expected.Visible, actual.Visible);
    }
}
