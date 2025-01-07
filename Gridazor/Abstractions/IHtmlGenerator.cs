using Microsoft.AspNetCore.Html;

namespace Gridazor.Abstractions;

internal interface IHtmlGenerator
{
    HtmlString Generate(HtmlParams htmlParams);
}

internal record HtmlParams(
    string Tag,
    string? Classes = null,
    string? Styles = null, 
    string? Attributes = null,
    string? InnerHtml = null,
    params HtmlParams[] Childs);
