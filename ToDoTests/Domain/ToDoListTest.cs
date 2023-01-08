using ToDoLibrary.Domain;

namespace ToDoTests.Domain;

public class ToDoListTest
{
    [Theory]
    [InlineData("", "shopping list")]
    [InlineData("do the dishes", "do the dishes")]
    [InlineData("take out the trash", "take out the trash")]
    public void TestToDoList_CreateToDoList_HappyPath(string actualTitle, string expectedTitle)
    {
        // Arrange
        var expected = new ToDoList {
            Complete = false,
            Title = expectedTitle,
            Visible = true,
        };

        // Act
        var actual = ToDoList.CreateToDoList(actualTitle);

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Theory]
    [InlineData("   do the dishes", "do the dishes")]
    [InlineData("take out the trash   ", "take out the trash")]
    public void TestToDoList_CreateToDoList_TrimsTitle(string actualTitle, string expectedTitle)
    {
        // Arrange
        var expected = new ToDoList {
            Complete = false,
            Visible = true,
            Title = expectedTitle,
        };

        // Act
        var actual = ToDoList.CreateToDoList(actualTitle);

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoList_MakeComplete() {
        // Arrange
        var expected = new ToDoList {
            Complete = true,
            Visible = true,
            Title = "do the dishes",
        };

        // Act
        var actual = ToDoList.CreateToDoList("do the dishes");
        actual.MakeComplete();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoList_MakeHidden() {
        // Arrange
        var expected = new ToDoList {
            Complete = false,
            Visible = false,
            Title = "do the dishes",
        };

        // Act
        var actual = ToDoList.CreateToDoList("do the dishes");
        actual.MakeHidden();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoList_Complete_ThenMakeIncomplete() {
        // Arrange
        var expected = new ToDoList {
            Complete = false,
            Visible = true,
            Title = "do the dishes",
        };

        // Act
        var actual = ToDoList.CreateToDoList("do the dishes");
        actual.MakeComplete();
        actual.MakeIncomplete();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Visible, actual.Visible);
    }

    [Fact]
    public void TestToDoList_MakeHidden_ThenMakeVisible() {
        // Arrange
        var expected = new ToDoList {
            Complete = false,
            Visible = true,
            Title = "do the dishes",
        };

        // Act
        var actual = ToDoList.CreateToDoList("do the dishes");
        actual.MakeHidden();
        actual.MakeVisible();

        // Assert
        Assert.Equal(expected.Complete, actual.Complete);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Visible, actual.Visible);
    }
}
