using Gridazor.Abstractions;
using Microsoft.AspNetCore.Html;
using System;
using System.Text;

namespace Gridazor.Core;


internal class HtmlGenerator : IHtmlGenerator
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

    public HtmlString Generate(HtmlParams htmlParams, params string[] innerHtml)
    {
        var tagBuilder = new StringBuilder();
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
            tagBuilder.Append(htmlParams.Attributes);
        }

        tagBuilder.Append('>');

        foreach (var content in innerHtml)
        {
            tagBuilder.Append(content);
        }

        tagBuilder.AppendFormat("</{0}>", htmlParams.Tag);

        return new HtmlString(tagBuilder.ToString());
    }
}
