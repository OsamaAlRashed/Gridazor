using Gridazor.Abstractions;
using Gridazor.Core;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        string id,
        string className)
        where TResult : class 
            => GridEditorFor(htmlHelper, expression, id, className, null);

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
            throw new ArgumentException("The property must be an enumerable type");
        }

        var propertyName = memberExpression.Member.Name;
        var propertyType = typeof(TResult).GetGenericArguments().FirstOrDefault() ?? throw new ArgumentException("Unable to determine property type from TResult");
        var modelData = expression.Compile()(htmlHelper.ViewData.Model);

        if (modelData is not IEnumerable<object> data)
        {
            return HtmlString.Empty;
        }

        var columnsProvider = customColumnsProvider ?? DefaultColumnProvider.Instance;
        var columns = columnsProvider.Get(propertyType);

        var htmlGenerator = HtmlGenerator.Instance;
        var htmlString = htmlGenerator.Generate(
            new HtmlParams("div", null, null, $"id=\"gridazor-{propertyName}\"", null,
                new HtmlParams("div", null, "display: none;", $"id=\"columnDefs-{propertyName}\"", JsonSerializer.Serialize(columns, _jsonOptions)),
                new HtmlParams("div", null, "display: none;", $"id=\"jsonData-{propertyName}\"", JsonSerializer.Serialize(data, _jsonOptions)),
                new HtmlParams("div", null, "display: none;", $"id=\"data-{propertyName}\"", null,
                    data.Select((row, index) =>
                    {
                        var rowHtml = new HtmlParams("div", "row", null, null, null,
                            propertyType.GetProperties().Select(property =>
                            {
                                var value = property.GetValue(row);
                                var input = new HtmlParams("input", null, null, $"id=\"{propertyName}_{index}__{property.Name}\" type=\"hidden\" name=\"{propertyName}[{index}].{property.Name}\" value=\"{value}\"");
                                
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
        => (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
            type.GetInterfaces().Any(
                t => t.IsGenericType
             && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

}
