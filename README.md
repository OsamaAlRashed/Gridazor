# Gridazor

**Gridazor** is a C# library that provides HTML helpers for Razor views/pages in ASP.NET Core applications. It simplifies the process of generating and managing data tables in your web applications by using the powerful [Ag-Grid](https://www.ag-grid.com/) library for the front end. With Gridazor, you can easily bind lists to tables and submit data without needing AJAX.

![Demo](https://github.com/user-attachments/assets/48bceec0-7309-4ee3-89d3-156e7585f845)


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
    public List<Product> Products { get; set; } = [];
}

public class Product
{
    [Editable(false)]
    [Hide]
    [Required(false)] // auto generated
    public Guid Id { get; set; } = Guid.NewGuid();

    [RowSelection]
    [Required(true)]
    public string Name { get; set; } = string.Empty;

    [CellEditor("agLargeTextCellEditor")]
    public string? Description { get; set; }

    [HeaderName("Category")]
    public int CatId { get; set; }

    [Required(true)]
    [HeaderName("Image")]
    public FileInput? Image { get; set; }
}

public class FileInput : IFileInput
{
    public IFormFile? File { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
}
```

### Example Razor View

Download the required js/css files from here:  
[Gridazor.zip](https://github.com/user-attachments/files/17090694/Gridazor.zip)

Import them in Layout.cshtml:
```csharp
<link rel="stylesheet" href="~/lib/Gridazor/GridazorDropdown/GridazorDropdown.css" />
<link rel="stylesheet" href="~/lib/Gridazor/GridazorFileInput/GridazorFileInput.css" />

<script src="https://cdn.jsdelivr.net/npm/ag-grid-community/dist/ag-grid-community.min.js"></script>
<script src="~/lib/Gridazor/GridazorDropdown/GridazorDropdown.js"></script>
<script src="~/lib/Gridazor/GridazorFileInput/GridazorFileInput.js"></script>
<script src="~/lib/Gridazor/Gridazor.js"></script>
```

```csharp
@model IndexVM
@using Gridazor;

<div class="card card-primary">
    @using (Html.BeginForm("Index", "Home", FormMethod.Post, new
    {
        enctype = "multipart/form-data"
    }))
    {
        <div class="card-header">
            <button id="delete-button" type="button" class="btn btn-danger">Delete</button>
        </div>
        <div class="card-body">
            @Html.GridEditorFor(x => x.Products, "myGrid", "ag-theme-quartz")
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
    <script>
        const dropdownValues = @Html.Raw(Json.Serialize(ViewBag.Cats));
        $("#myGrid").gridazor({
            propertyName: '@nameof(Model.Products)',
            overrideColumnDefs: [
                {
                    field: "catId",
                    cellEditor: GridazorDropdown,
                    cellEditorParams: {
                        values: dropdownValues.map(item => ({
                            value: item.id,
                            text: item.name
                        }))
                    },
                    valueFormatter: (params) => gridazorDropdownHelper.valueFormatter(params)
                },
                {
                    field: "image",
                    cellEditor: GridazorFileInput,
                    cellRenderer: (params) => gridazorFileInputHelper.cellRender(params)
                }
            ],
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
- **RTL Support**: Enable or disable right-to-left text direction.
- **Row Deletion**: Enable row deletion with a custom delete button.

## Contributing

Contributions are welcome! especially front-end developers, to improve the custom editors styles :)

## Todo
 - Support Custom button for inserting the row.
 - Improve the style of the custom inputs.
 - Improve the rendering of the html elements.
 - Fix the override columns functionality.
 - Include the css/js files inside the Nuget package.
