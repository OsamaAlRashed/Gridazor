using Gridazor.Abstractions;
using Gridazor.Core;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace Gridazor;

public static class GridazorExtenstions
{
    private readonly static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static IHtmlContent GridEditorFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        string id, string className)
        where TResult : class
    {
        return GridEditorFor(htmlHelper, expression, id, className, null);
    }

    public static IHtmlContent GridEditorFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        string id, 
        string className,
        IColumnsProvider? customColumnsProvider)
        where TResult : class
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException(nameof(expression.Body));
        }

        if (!IsEnumerableType(typeof(TResult)))
        {
            throw new ArgumentException("TResult must be an enumerable type");
        }

        var propertyName = memberExpression.Member.Name;
        var columnType = typeof(TResult).GetGenericArguments().FirstOrDefault() ?? throw new ArgumentException("Unable to determine column type from TResult");
        var selector = expression.Compile();
        var modelData = selector(htmlHelper.ViewData.Model);

        if (htmlHelper.ViewData.Model is null || modelData is not IEnumerable<object> data)
        {
            return HtmlString.Empty;
        }

        var columnsProvider = customColumnsProvider ?? DefaultColumnProvider.Instance;
        var columns = columnsProvider.Get(columnType);

        var htmlGenerator = HtmlGenerator.Instance;
        var htmlString = htmlGenerator.Generate(
            new HtmlParams("div", null, null, $"id=\"gridazor-{propertyName}\"", null,
                new HtmlParams("div", null, "display: none;", $"id=\"columnDefs-{propertyName}\"", JsonSerializer.Serialize(columns, _jsonOptions)),
                new HtmlParams("div", null, "display: none;", $"id=\"jsonData-{propertyName}\"", JsonSerializer.Serialize(data, _jsonOptions)),
                new HtmlParams("div", null, "display: none;", $"id=\"data-{propertyName}\"", null,
                    data.Select((row, index) =>
                    {
                        var rowHtml = new HtmlParams("div", "row", null, null, null,
                            columnType.GetProperties().Select(property =>
                            {
                                var value = property.GetValue(row);
                                var input = new HtmlParams("input", null, null, $"type=\"hidden\" name=\"{propertyName}[{index}].{property.Name}\" value=\"{value}\"");
                                return input;
                            }).ToArray());
                        return rowHtml;
                    }).ToArray()
                ),
                new HtmlParams("div", className, null, $"id=\"{id}\"")
            )
        );
        
        return htmlString;
    }

    private static bool IsEnumerableType(Type type)
    {
        return type.GetInterfaces()
               .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
           || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

}
