using Microsoft.AspNetCore.Html;
using System;

namespace Gridazor.Abstractions;

internal interface IHtmlGenerator
{
    HtmlString Generate(
        HtmlParams htmlParams, params string[] childs);
}

internal record HtmlParams(
    string Tag,
    string? Classes = null,
    string? Styles = null, 
    string? Attributes = null,
    string? InnerHtml = null);
