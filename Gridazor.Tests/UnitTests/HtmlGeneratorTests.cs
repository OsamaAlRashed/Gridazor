using FluentAssertions;
using Gridazor.Abstractions;
using Gridazor.Core;
using Microsoft.AspNetCore.Html;

namespace Gridazor.Tests.UnitTests;

public class HtmlGeneratorTests
{
    private readonly HtmlGenerator _htmlGenerator;

    public HtmlGeneratorTests()
    {
        _htmlGenerator = HtmlGenerator.Instance;
    }


    [Fact]
    public void Generate_ShouldReturnCorrectHtml_ForSingleDivWithClassAndStyle()
    {
        // Arrange
        var htmlParams = new HtmlParams("div", "my-class", "color:red;");

        // Act
        HtmlString result = _htmlGenerator.Generate(htmlParams);

        // Assert
        result.ToString().Should().Be("<div class=\"my-class\" style=\"color:red;\"></div>");
    }

    [Fact]
    public void Generate_ShouldReturnCorrectHtml_WithNestedElements()
    {
        // Arrange
        var htmlParamsParent = new HtmlParams("div", "parent-class", "background-color:blue;");
        var htmlParamsChild = new HtmlParams("span", "child-class", "color:white;");

        // Act
        HtmlString childHtml = _htmlGenerator.Generate(htmlParamsChild, "Child Content");
        HtmlString parentHtml = _htmlGenerator.Generate(htmlParamsParent, childHtml.ToString());

        // Assert
        parentHtml.ToString().Should().Be("<div class=\"parent-class\" style=\"background-color:blue;\"><span class=\"child-class\" style=\"color:white;\">Child Content</span></div>");
    }

    [Fact]
    public void Generate_ShouldReturnCorrectHtml_WithNestedElements3Levels()
    {
        // Arrange
        var htmlParamsL1= new HtmlParams("div", "l1-class", "background-color:blue;");
        var htmlParamsL2 = new HtmlParams("div", "l2-class", "");
        var htmlParamsL3 = new HtmlParams("span", "l3-class", "color:white;");

        // Act
        HtmlString l3Html = _htmlGenerator.Generate(htmlParamsL3, "Child Content");
        HtmlString l2Html = _htmlGenerator.Generate(htmlParamsL2, l3Html.ToString());
        HtmlString l1Html = _htmlGenerator.Generate(htmlParamsL1, l2Html.ToString());

        // Assert
        l1Html.ToString().Should().Be("<div class=\"l1-class\" style=\"background-color:blue;\"><div class=\"l2-class\"><span class=\"l3-class\" style=\"color:white;\">Child Content</span></div></div>");
    }

    [Fact]
    public void Generate_ShouldReturnCorrectHtml_WhenAttributesAreProvided()
    {
        // Arrange
        var htmlParams = new HtmlParams("input", "input-class", "", " type=\"text\" value=\"test\"");

        // Act
        HtmlString result = _htmlGenerator.Generate(htmlParams);

        // Assert
        result.ToString().Should().Be("<input class=\"input-class\" type=\"text\" value=\"test\"></input>");
    }

    [Fact]
    public void Generate_ShouldHandleEmptyClassStyleAndAttributes()
    {
        // Arrange
        var htmlParams = new HtmlParams("p");

        // Act
        HtmlString result = _htmlGenerator.Generate(htmlParams, "Text content");

        // Assert
        result.ToString().Should().Be("<p>Text content</p>");
    }
}
