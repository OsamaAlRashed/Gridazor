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

namespace Gridazor
{
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
            var columnType = typeof(TResult).GetGenericArguments().FirstOrDefault();

            if (columnType is null)
            {
                throw new ArgumentException("Unable to determine column type from TResult");
            }

            var selector = expression.Compile();
            var modelData = selector(htmlHelper.ViewData.Model);

            if (htmlHelper.ViewData.Model is null || modelData is not IEnumerable data)
            {
                return HtmlString.Empty;
            }

            var columnsProvider = customColumnsProvider ?? DefaultColumnProvider.Instance;
            var columns = columnsProvider.Get(columnType);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"<div id=\"gridazor-{propertyName}\">");

            AppendHiddenColumns(stringBuilder, $"columnDefs-{propertyName}", columns);
            AppendHiddenJsonData(stringBuilder, $"jsonData-{propertyName}", data);
            AppendHiddenDataRows(stringBuilder, propertyName, columnType, data);

            stringBuilder.AppendLine($"<div id=\"{id}\" class=\"{className}\"></div>");
            stringBuilder.AppendLine("</div>");
            return new HtmlString(stringBuilder.ToString());
        }

        private static void AppendHiddenColumns(StringBuilder stringBuilder, string id, object data)
        {
            stringBuilder.AppendLine($"<div style=\"display: none;\" id=\"{id}\">");
            stringBuilder.AppendLine(JsonSerializer.Serialize(data, _jsonOptions));
            stringBuilder.AppendLine("</div>");
        }

        private static void AppendHiddenJsonData(StringBuilder stringBuilder, string id, object data)
        {
            stringBuilder.AppendLine($"<div style=\"display: none;\" id=\"{id}\">");
            stringBuilder.AppendLine(JsonSerializer.Serialize(data, _jsonOptions));
            stringBuilder.AppendLine("</div>");
        }

        private static void AppendHiddenDataRows(StringBuilder stringBuilder, string propertyName, Type columnType, IEnumerable data)
        {
            var properties = columnType.GetProperties();
            int rowIndex = 0;

            stringBuilder.AppendLine($"<div style=\"display: none;\" id=\"data-{propertyName}\">");

            foreach (var row in data)
            {
                stringBuilder.AppendLine("<div class=\"row\">");

                foreach (var property in properties)
                {
                    var value = property.GetValue(row);
                    stringBuilder.AppendLine($"<input type=\"hidden\" name=\"{propertyName}[{rowIndex}].{property.Name}\" value=\"{value}\" />");
                }

                stringBuilder.AppendLine("</div>");
                rowIndex++;
            }

            stringBuilder.AppendLine("</div>");
        }

        private static bool IsEnumerableType(Type type)
        {
            return type.GetInterfaces()
                   .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
               || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

    }
}
