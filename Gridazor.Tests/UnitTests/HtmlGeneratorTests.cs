using FluentAssertions;
using Gridazor.Abstractions;
using Gridazor.Core;

namespace Gridazor.Tests.UnitTests;

public class HtmlGeneratorTests
{
    private readonly HtmlGenerator _htmlGenerator;

    public HtmlGeneratorTests()
    {
        _htmlGenerator = HtmlGenerator.Instance;
    }


    [Fact]
    public void Generate_SingleElement_ReturnsCorrectHtml()
    {
        // Arrange
        var htmlParams = new HtmlParams("div", "container", "color: red;", "id=\"main\"");

        var expectedHtml = "<div class=\"container\" style=\"color: red;\" id=\"main\"></div>";

        // Act
        var result = _htmlGenerator.Generate(htmlParams);

        // Assert
        result.ToString().Should().Be(expectedHtml);
    }

    [Fact]
    public void Generate_NestedElements_ReturnsCorrectHtml()
    {
        //Arrange
        var parentParams = new HtmlParams("div", "parent", "padding: 10px;", "id=\"parent\"", null,
            new HtmlParams("span", "child", "font-weight: bold;", "id=\"child\""));

        var expectedHtml = "<div class=\"parent\" style=\"padding: 10px;\" id=\"parent\">" +
                           "<span class=\"child\" style=\"font-weight: bold;\" id=\"child\"></span>" +
                           "</div>";

        // Act
        var result = _htmlGenerator.Generate(parentParams);

        // Assert
        result.ToString().Should().Be(expectedHtml);
    }

    [Fact]
    public void Generate_NoAttributes_ReturnsTagOnly()
    {
        // Arrange
        var htmlParams = new HtmlParams("br");

        var expectedHtml = "<br></br>";

        // Act
        var result = _htmlGenerator.Generate(htmlParams);

        // Assert
        result.ToString().Should().Be(expectedHtml);
    }

    [Fact]
    public void Generate_DeeplyNestedElements_ReturnsCorrectHtml()
    {
        // Arrange
        var expectedHtml =
            "<div id=\"gridazor\">" +
                "<div id=\"jsonData\">JsonData</div>" +
                "<div id=\"columnDefs\">ColumnDefs</div>" +
                "<div id=\"data\">" +
                    "<div class=\"row\">" +
                        "<input type=\"hidden\" name=\"test\" value=\"test\"></input>" +
                    "</div>" +
                "</div>" +
            "</div>";

        // Act
        var result = _htmlGenerator.Generate(
            new HtmlParams("div", null, null, "id=\"gridazor\"", null,
                new HtmlParams("div", null, null, "id=\"jsonData\"", "JsonData"),
                new HtmlParams("div", null, null, "id=\"columnDefs\"", "ColumnDefs"),
                new HtmlParams("div", null, null, "id=\"data\"", null,
                    new HtmlParams("div", "row", null, null, null,
                        new HtmlParams("input", null, null, "type=\"hidden\" name=\"test\" value=\"test\"")
                        )
                    )
                )
        );

        // Assert
        result.ToString().Should().Be(expectedHtml);
    }
}
