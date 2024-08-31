using Gridazor.Abstractions;
using Microsoft.AspNetCore.Html;
using System;
using System.Text;

namespace Gridazor.Core;


internal sealed class HtmlGenerator : IHtmlGenerator
{
    private HtmlGenerator() { }

    private static readonly Lazy<HtmlGenerator> _lazy =
        new(() => new HtmlGenerator());
    public static HtmlGenerator Instance
    {
        get
        {
            return _lazy.Value;
        }
    }
    public HtmlString Generate(HtmlParams htmlParams)
    {
        var tagBuilder = new StringBuilder();
        BuildHtmlElement(tagBuilder, htmlParams);
        return new HtmlString(tagBuilder.ToString());
    }

    private static void BuildHtmlElement(StringBuilder tagBuilder, HtmlParams htmlParams)
    {
        tagBuilder.AppendFormat("<{0}", htmlParams.Tag);

        if (!string.IsNullOrEmpty(htmlParams.Classes))
        {
            tagBuilder.AppendFormat(" class=\"{0}\"", htmlParams.Classes);
        }

        if (!string.IsNullOrEmpty(htmlParams.Styles))
        {
            tagBuilder.AppendFormat(" style=\"{0}\"", htmlParams.Styles);
        }

        if (!string.IsNullOrEmpty(htmlParams.Attributes))
        {
            tagBuilder.Append(' ');
            tagBuilder.Append(htmlParams.Attributes.Trim());
        }

        tagBuilder.Append('>');

        if (!string.IsNullOrEmpty(htmlParams.InnerHtml)) 
        {
            tagBuilder.Append(htmlParams.InnerHtml);
        }

        if (htmlParams.Childs != null && htmlParams.Childs.Length > 0)
        {
            foreach (var child in htmlParams.Childs)
            {
                BuildHtmlElement(tagBuilder, child);
            }
        }

        tagBuilder.AppendFormat("</{0}>", htmlParams.Tag);
    }

}
