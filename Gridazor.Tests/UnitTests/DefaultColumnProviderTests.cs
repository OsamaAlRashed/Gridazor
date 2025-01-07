using FluentAssertions;
using Gridazor.Attributes;
using Gridazor.Core;
using Gridazor.Models;

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
        public required string TestProperty { get; set; }

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
        var columns = provider.Get(type, []).ToList();

        // Assert
        columns.Should().HaveCount(3);

        var firstColumn = columns.FirstOrDefault(c => c.Field == "customField");
        firstColumn.Should().NotBeNull();
        firstColumn!.HeaderName.Should().Be("Custom Header");
        firstColumn.Field.Should().Be("customField");
        firstColumn.Editable.Should().BeFalse();
        firstColumn.CellDataType.Should().Be("string");
        firstColumn.CellEditor.Should().Be("text");
        firstColumn.Required.Should().BeTrue();
        firstColumn.Hide.Should().BeTrue();
        firstColumn.IsRowSelectable.Should().BeTrue();

        var secondColumn = columns.FirstOrDefault(c => c.Field == "anotherProperty");
        secondColumn.Should().NotBeNull();
        secondColumn!.HeaderName.Should().Be("AnotherProperty");
        secondColumn.Field.Should().Be("anotherProperty");
        secondColumn.Editable.Should().BeTrue();
        secondColumn.CellDataType.Should().Be("number");
        secondColumn.CellEditor.Should().Be("agNumberCellEditor");
        secondColumn.Required.Should().BeTrue();
        secondColumn.Hide.Should().BeFalse();
        secondColumn.IsRowSelectable.Should().BeFalse();

        var thirdColumn = columns.FirstOrDefault(c => c.Field == "dateProperty");
        thirdColumn.Should().NotBeNull();
        thirdColumn!.HeaderName.Should().Be("DateProperty");
        thirdColumn.Field.Should().Be("dateProperty");
        thirdColumn.Editable.Should().BeTrue();
        thirdColumn.CellDataType.Should().Be("dateString");
        thirdColumn.CellEditor.Should().Be("agDateStringCellEditor");
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
        var columns = provider.Get(type, []).ToList();

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

    [Fact]
    public void GetWithOverrideMetadataValues_ShouldReturnColumnWithDefaultValues()
    {
        // Arrange
        var provider = DefaultColumnProvider.Instance;
        var type = typeof(DefaultClass);

        var dic = new Dictionary<string, Func<Column, object>>
        {
            { nameof(CellDataTypeAttribute), (column) => "Custom_" + column.CellDataType },
            { nameof(CellEditorAttribute), (column) => "Custom_" + column.CellEditor },
            { nameof(EditableAttribute), (column) => !column.Editable },
            { nameof(FieldAttribute), (column) =>  "Custom_" + column.Field },
            { nameof(HeaderNameAttribute), (column) => "Custom_" + column.HeaderName },
            { nameof(HideAttribute), (column) => !column.Hide },
            { nameof(RequiredAttribute), (column) => !column.Required },
            { nameof(RowSelectionAttribute), (column) => !column.IsRowSelectable },
        };

        // Act
        var columns = provider.Get(type, dic).ToList();

        // Assert
        columns.Should().HaveCount(1);

        var column = columns.First();
        column.CellDataType.Should().Be("Custom_text");
        column.CellEditor.Should().Be("Custom_");
        column.Editable.Should().BeFalse();
        column.Field.Should().Be("Custom_defaultProperty");
        column.HeaderName.Should().Be("Custom_DefaultProperty");
        column.Hide.Should().BeTrue();
        column.Required.Should().BeTrue();
        column.IsRowSelectable.Should().BeTrue();
    }
}

