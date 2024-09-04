﻿using FluentAssertions;
using Gridazor.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq.Expressions;
using System.Text.Json;

namespace Gridazor.Tests.IntegrationTests;

public class GridEditorForTests
{
    private readonly static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact]
    public void GridEditorFor_WithValidEnumerableType_ReturnsExpectedHtml()
    {
        // Arrange
        var htmlHelperMock = new Mock<IHtmlHelper<TestModel>>();
        var viewData = new ViewDataDictionary<TestModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = new TestModel
            {
                Items =
                [
                    new TestItem { Name = "Item1", Value = 10 },
                    new TestItem { Name = "Item2", Value = 20 }
                ]
            }
        };

        htmlHelperMock.Setup(x => x.ViewData).Returns(viewData);

        Expression<Func<TestModel, IEnumerable<TestItem>>> expression = model => model.Items;

        var expectedPropertyName = nameof(TestModel.Items);
        var expectedColumnDefsJson = JsonSerializer.Serialize(DefaultColumnProvider.Instance.Get(typeof(TestItem)), _jsonOptions);
        var expectedJsonData = JsonSerializer.Serialize(viewData.Model.Items, _jsonOptions);

        // Act
        var result = htmlHelperMock.Object.GridEditorFor(expression, "grid-id", "grid-class");

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().Contain($"id=\"gridazor-{expectedPropertyName}\"");
        result.ToString().Should().Contain($"id=\"columnDefs-{expectedPropertyName}\" style=\"display: none;\">{expectedColumnDefsJson}</div>");
        result.ToString().Should().Contain($"id=\"jsonData-{expectedPropertyName}\" style=\"display: none;\">{expectedJsonData}</div>");
        result.ToString().Should().Contain("id=\"grid-id\" class=\"grid-class\"");
    }

    [Fact]
    public void GridEditorFor_WithInvalidType_ThrowsArgumentException()
    {
        // Arrange
        var htmlHelperMock = new Mock<IHtmlHelper<TestModel>>();
        var testModel = new TestModel { Items = new List<TestItem>() };
        htmlHelperMock.SetupGet(x => x.ViewData.Model).Returns(testModel);

        Expression<Func<TestModel, TestItem>> expression = model => model.Items.First();

        // Act
        Action act = () => htmlHelperMock.Object.GridEditorFor(expression, "grid-id", "grid-class");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("TResult must be an enumerable type");
    }

    [Fact]
    public void GridEditorFor_WithNonEnumerableType_ThrowsArgumentException()
    {
        // Arrange
        var htmlHelperMock = new Mock<IHtmlHelper<TestModel>>();
        var testModel = new TestModel();
        htmlHelperMock.SetupGet(x => x.ViewData.Model).Returns(testModel);

        Expression<Func<TestModel, string>> expression = model => model.NonEnumerableProperty;

        // Act
        Action act = () => htmlHelperMock.Object.GridEditorFor(expression, "grid-id", "grid-class");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("TResult must be an enumerable type");
    }

    public class TestModel
    {
        public IEnumerable<TestItem> Items { get; set; }
        public string NonEnumerableProperty { get; set; }
    }

    public class TestItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
