using FluentAssertions;
using Gridazor.Attributes;
using Gridazor.Core;

namespace Gridazor.Tests.UnitTests;

public class DefaultColumnProviderTests
{
    private class TestClass
    {
        [Field("customField")]
        [HeaderName("Custom Header")]
        [Editable(false)]
        [CellDataType("string")]
        [CellEditor("text")]
        [Required(true)]
        [Hide]
        [RowSelection]
        public string TestProperty { get; set; }

        public int AnotherProperty { get; set; }

        public DateTime DateProperty { get; set; }
    }

    private class DefaultClass
    {
        public string? DefaultProperty { get; set; }
    }

    [Fact]
    public void Get_ShouldReturnColumnsWithAttributes()
    {
        // Arrange
        var provider = DefaultColumnProvider.Instance;
        var type = typeof(TestClass);

        // Act
        var columns = provider.Get(type).ToList();

        // Assert
        columns.Should().HaveCount(3);

        var firstColumn = columns.FirstOrDefault(c => c.Field == "customField");
        firstColumn.Should().NotBeNull();
        firstColumn.HeaderName.Should().Be("Custom Header");
        firstColumn.Field.Should().Be("customField");
        firstColumn.Editable.Should().BeFalse();
        firstColumn.CellDataType.Should().Be("string");
        firstColumn.CellEditor.Should().Be("text");
        firstColumn.Required.Should().BeTrue();
        firstColumn.Hide.Should().BeTrue();
        firstColumn.IsRowSelectable.Should().BeTrue();

        var secondColumn = columns.FirstOrDefault(c => c.Field == "anotherProperty");
        secondColumn.Should().NotBeNull();
        secondColumn.HeaderName.Should().Be("AnotherProperty");
        secondColumn.Field.Should().Be("anotherProperty");
        secondColumn.Editable.Should().BeTrue();
        secondColumn.CellDataType.Should().Be("number");
        secondColumn.CellEditor.Should().Be("agNumberCellEditor");
        secondColumn.Required.Should().BeTrue();
        secondColumn.Hide.Should().BeFalse();
        secondColumn.IsRowSelectable.Should().BeFalse();

        var thirdColumn = columns.FirstOrDefault(c => c.Field == "dateProperty");
        thirdColumn.Should().NotBeNull();
        thirdColumn.HeaderName.Should().Be("DateProperty");
        thirdColumn.Field.Should().Be("dateProperty");
        thirdColumn.Editable.Should().BeTrue();
        thirdColumn.CellDataType.Should().Be("dateString");
        thirdColumn.CellEditor.Should().BeEmpty();
        thirdColumn.Required.Should().BeTrue();
        thirdColumn.Hide.Should().BeFalse();
        thirdColumn.IsRowSelectable.Should().BeFalse();
    }

    [Fact]
    public void Get_ShouldReturnColumnWithDefaultValues()
    {
        // Arrange
        var provider = DefaultColumnProvider.Instance;
        var type = typeof(DefaultClass);

        // Act
        var columns = provider.Get(type).ToList();

        // Assert
        columns.Should().HaveCount(1);

        var column = columns.First();
        column.HeaderName.Should().Be("DefaultProperty");
        column.Field.Should().Be("defaultProperty");
        column.Editable.Should().BeTrue();
        column.CellDataType.Should().Be("text");
        column.CellEditor.Should().BeEmpty();
        column.Required.Should().BeFalse();
        column.Hide.Should().BeFalse();
        column.IsRowSelectable.Should().BeFalse();
    }
}

