namespace Gridazor.Statics;

/// <summary>
/// Cell data type class
/// </summary>
public static class CellDataType
{
    /// <summary>
    /// Number type
    /// </summary>
    public const string Number = "number";

    /// <summary>
    /// Date String type
    /// </summary>
    public const string DateString = "dateString";

    /// <summary>
    /// Date type
    /// </summary>
    public const string Date = "date";

    /// <summary>
    /// Text type
    /// </summary>
    public const string Text = "text";

    /// <summary>
    /// Boolean type
    /// </summary>
    public const string Boolean = "boolean";
}


/// <summary>
/// Cell editor class
/// </summary>
public static class CellEditor
{
    /// <summary>
    /// AgNumber Cell Editor
    /// </summary>
    public const string AgNumberCellEditor = "agNumberCellEditor";

    /// <summary>
    /// AgCheckbox Cell Editor
    /// </summary>
    public const string AgCheckboxCellEditor = "agCheckboxCellEditor";

    /// <summary>
    /// AgDateString Cell Editor
    /// </summary>
    public const string AgDateStringCellEditor = "agDateStringCellEditor";

    /// <summary>
    /// AgDate Cell Editor
    /// </summary>
    public const string AgDateCellEditor = "agDateCellEditor";

    /// <summary>
    /// AgLargeText Cell Editor
    /// </summary>
    public const string AgLargeTextCellEditor = "agLargeTextCellEditor";
}

internal static class HtmlConstants
{
    internal const string Div = "div";
    internal const string Input = "input";
    internal const string HideElement = "display: none;";
}