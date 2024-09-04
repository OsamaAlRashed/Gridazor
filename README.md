# Gridazor

**Gridazor** is a C# library that provides HTML helpers for Razor views/pages in ASP.NET Core applications. It simplifies the process of generating and managing data tables in your web applications by using the powerful [Ag-Grid](https://www.ag-grid.com/) library for the front end. With Gridazor, you can easily bind lists to tables and submit data without needing AJAX.

![Gridazor Exa![- Demo - Personal - Microsoft_ Edge 2024-09-04 21-46-54](https://github.com/user-attachments/assets/3219a231-c4ae-42aa-90d1-06916798004e)
mple]() <!-- Add your GIF image path here -->

## Features

- **Easy Table Generation**: Automatically generate tables from your models.
- **Data Binding**: Bind lists to tables with minimal setup.
- **Non-AJAX Submission**: Submit table data without the need for AJAX.
- **Ag-Grid Integration**: Utilize the robust features of Ag-Grid for front-end table management.
- **Customizable**: Customize column definitions, cell editors, and more.

## Installation

You can install Gridazor via NuGet Package Manager:

```bash
Install-Package Gridazor
```
Or via the .NET CLI:

```bash
dotnet add package Gridazor
```

## Usage
### Example Model
```csharp
public class IndexVM
{
    public List<Test> Tests { get; set; } = [];
}

public class Test
{
    [Field("id")]
    [HeaderName("Id")]
    [Editable(false)]
    [Hide]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Field("name")]
    [HeaderName("Name")]
    [Editable(true)]
    [Selectable]
    public string Name { get; set; } = string.Empty;

    [Field("description")]
    [HeaderName("Description")]
    [Editable(true)]
    [CellEditor("agLargeTextCellEditor")]
    public string? Description { get; set; }

    public int CatId { get; set; }
}
```

### Example Razor View

```csharp
@model IndexVM
@using Gridazor;

<div class="card card-primary">
    @using (Html.BeginForm("Index", "Home", FormMethod.Post))
    {
        <div class="card-header">
            <button id="delete-button" type="button" class="btn btn-danger">Delete</button>
        </div>
        <div class="card-body">
            @Html.GridEditorFor(x => x.Tests, "myGrid", "ag-theme-quartz")
        </div>
        <div class="card-footer">
            <button type="submit" class="btn btn-primary">Submit</button>
        </div>
    }
</div>
```

### Example Script Section
```js
@section Scripts{
    <script src="https://cdn.jsdelivr.net/npm/ag-grid-community/dist/ag-grid-community.min.js"></script>
    <script>
        const dropdownValues = @Html.Raw(Json.Serialize(ViewBag.Cats));
        console.log(dropdownValues)
        $("#myGrid").gridazor({
            propertyName: '@nameof(Model.Tests)',
            enableRtl: false,
            overrideColumnDefs: [{
                field: "catId",
                headerName: "Cat Id",
                cellEditor: CustomDropdownEditor,
                cellEditorParams: {
                    values: dropdownValues.map(item => ({
                        value: item.id,
                        text: item.name
                    }))
                },
                valueFormatter: function (params) {
                    console.log(params)
                    const option = params.colDef.cellEditorParams.values.find(opt => opt.value === params.value);
                    console.log(option)
                    return option ? option.text : params.value;
                }
            }],
            enableDelete: true,
            deleteButtonId: "delete-button"
        });
    </script>
}
```

## Customization

Gridazor allows for extensive customization, including:

- **Column Definitions**: Override default column definitions.
- **Cell Editors**: Use custom cell editors or pre-built Ag-Grid editors.
- **Value Formatters**: Format the display of cell values based on custom logic.
- **RTL Support**: Enable or disable right-to-left text direction.
- **Row Deletion**: Enable row deletion with a custom delete button.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

