﻿using Gridazor.Abstractions;
using Gridazor.Core;
using Gridazor.Models;
using Gridazor.Statics;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace Gridazor;

/// <summary>
/// Represents the Gridazor Extensions
/// </summary>
public static class GridazorExtensions
{
    private readonly static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Generates an HTML grid editor for the specified model property.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TResult">The result type, which must be a class.</typeparam>
    /// <param name="htmlHelper">The HTML helper instance.</param>
    /// <param name="expression">The expression representing the model property.</param>
    /// <param name="gridId">The unique identifier for the grid.</param>
    /// <param name="agGridTheme">The AG Grid theme to apply.</param>
    /// <returns>Returns the generated HTML content for the grid editor.</returns>
    public static IHtmlContent GridEditorFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        string gridId,
        string agGridTheme)
        where TResult : class 
            => GridEditorFor(htmlHelper, expression, gridId, agGridTheme, null, new Dictionary<string, Func<Column, object>>());

    /// <summary>
    /// Generates an HTML grid editor for the specified model property.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TResult">The result type, which must be a class.</typeparam>
    /// <param name="htmlHelper">The HTML helper instance.</param>
    /// <param name="expression">The expression representing the model property.</param>
    /// <param name="gridId">The unique identifier for the grid.</param>
    /// <param name="agGridTheme">The AG Grid theme to apply.</param>
    /// <param name="customColumnsProvider">Custom Columns Provider</param>
    /// <returns>Returns the generated HTML content for the grid editor.</returns>
    public static IHtmlContent GridEditorFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        string gridId,
        string agGridTheme,
        IColumnsProvider? customColumnsProvider)
        where TResult : class
            => GridEditorFor(htmlHelper, expression, gridId, agGridTheme, customColumnsProvider, new Dictionary<string, Func<Column, object>>());

    /// <summary>
    /// Generates an HTML grid editor for the specified model property.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    /// <typeparam name="TResult">The result type, which must be a class.</typeparam>
    /// <param name="htmlHelper">The HTML helper instance.</param>
    /// <param name="expression">The expression representing the model property.</param>
    /// <param name="gridId">The unique identifier for the grid.</param>
    /// <param name="agGridTheme">The AG Grid theme to apply.</param>
    /// <param name="overrideColumnMetadataValues">Dictionary that contaians the metadata</param>
    /// <returns>Returns the generated HTML content for the grid editor.</returns>
    public static IHtmlContent GridEditorFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        string gridId,
        string agGridTheme,
        Dictionary<string, Func<Column, object>> overrideColumnMetadataValues)
        where TResult : class
            => GridEditorFor(htmlHelper, expression, gridId, agGridTheme, null, overrideColumnMetadataValues);

    private static IHtmlContent GridEditorFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        string gridId, 
        string agGridTheme,
        IColumnsProvider? customColumnsProvider,
        Dictionary<string, Func<Column, object>> overrideColumnMetadataValues)
        where TResult : class
    {
        ArgumentNullException.ThrowIfNull(nameof(expression));
        ArgumentNullException.ThrowIfNull(nameof(htmlHelper));
        ArgumentNullException.ThrowIfNull(nameof(htmlHelper.ViewData));

        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException(nameof(expression.Body));
        }

        if (!IsEnumerableType(typeof(TResult)))
        {
            throw new ArgumentException("The property type must be an enumerable type");
        }
        var propertyType = typeof(TResult).GetGenericArguments().FirstOrDefault() ??
            throw new ArgumentException("The property type must be generic type.");

        var propertyName = memberExpression.Member.Name;
        var propertyData = expression.Compile()(htmlHelper.ViewData.Model);

        if (propertyData is not IEnumerable<object> data)
        {
            return HtmlString.Empty;
        }

        var columnsProvider = customColumnsProvider ?? DefaultColumnProvider.Instance;
        var columns = columnsProvider.Get(propertyType, overrideColumnMetadataValues);

        var htmlGenerator = HtmlGenerator.Instance;
        var htmlString = htmlGenerator.Generate(
            new HtmlParams(HtmlConstants.Div, null, null, $"id=\"gridazor-{propertyName}\"", null,
                new HtmlParams(HtmlConstants.Div, null, HtmlConstants.HideElement, $"id=\"columnDefs-{propertyName}\"", JsonSerializer.Serialize(columns, _jsonOptions)),
                new HtmlParams(HtmlConstants.Div, null, HtmlConstants.HideElement, $"id=\"jsonData-{propertyName}\"", JsonSerializer.Serialize(data, _jsonOptions)),
                new HtmlParams(HtmlConstants.Div, null, HtmlConstants.HideElement, $"id=\"data-{propertyName}\"", null,
                    GenerateInputsFromData(propertyType, propertyName, data)
                ),
                new HtmlParams(HtmlConstants.Div, agGridTheme, null, $"id=\"{gridId}\"")
            )
        );

        return htmlString;
    }

    private static HtmlParams[] GenerateInputsFromData(Type propertyType, string propertyName, IEnumerable<object> data)
    {
        return data.Select((row, index) =>
        {
            var rowHtml = new HtmlParams(HtmlConstants.Div, "row", null, null, null,
                propertyType.GetProperties().Select(property =>
                {
                    if (property.PropertyType.GetInterfaces().Contains(typeof(IFileInput)))
                    {
                        var propertyValue = property.GetValue(row);
                        return new HtmlParams(HtmlConstants.Div, null, null, null, null,
                            property.PropertyType.GetProperties().Select(fileProperty =>
                            {
                                object? value = default;
                                if (propertyValue is not null)
                                {
                                    value = fileProperty.GetValue(propertyValue);
                                }

                                return new HtmlParams(HtmlConstants.Input, null, null, $"id=\"{propertyName}_{index}__{property.Name}__{fileProperty.Name}\" type=\"hidden\" name=\"{propertyName}[{index}].{property.Name}.{fileProperty.Name}\" value=\"{value}\"");
                            }).ToArray()
                        );
                    }

                    var value = property.GetValue(row);
                    var input = new HtmlParams(HtmlConstants.Input, null, null, $"id=\"{propertyName}_{index}__{property.Name}\" type=\"hidden\" name=\"{propertyName}[{index}].{property.Name}\" value=\"{value}\"");

                    return input;
                }).ToArray());

            return rowHtml;
        }).ToArray();
    }

    private static bool IsEnumerableType(Type type)
        => (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
            type.GetInterfaces().Any(
                t => t.IsGenericType
             && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

}
