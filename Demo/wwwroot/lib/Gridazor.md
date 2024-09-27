Welcome to the Gridazor wiki!

## Installation

You can install Gridazor via NuGet Package Manager:

```bash
Install-Package Gridazor
```

Import the Aggrid library in your page/layout:

```csharp
<script src="https://cdn.jsdelivr.net/npm/ag-grid-community/dist/ag-grid-community.min.js"></script>
```

Download this zip file that contains all the required CSS/js files, later I will try to include it inside the Nuget package itself.

Import Gridazor.js file into your page or layout:

```csharp
<script src="yourPath/Gridazor.js"></script>
```

## Usage

Define your list inside your ViewModel:
```csharp
public class IndexVM
{
    public List<Product> Products { get; set; } = [];
}
```

In the View you can add your form and use Gridazor HTML helper:

```csharp
@Html.GridEditorFor(x => x.Products, "myGrid", "ag-theme-quartz")
```
GridEditorFor takes 3 parameters:
  - Expression represents the list
  - The ID for the grid.
  - The AG Grid theme to apply, please see: https://www.ag-grid.com/javascript-data-grid/themes

In the scripts part you must initiate the Gridazor object:
```js
const gridElement = document.querySelector('#myGrid');
const gridazor = new Gridazor(gridElement, {
    propertyName: '@nameof(Model.Products)',
    enableDelete: true,
    deleteButtonId: "delete-button"
});
```

Gridazor object has the following properties:
- ```propertyName```: The name of your list property, and it's required.
- ```enableRtl```: enables right to left direction, the default is false.
- ```enableDelete```: enables deleting the rows, the default is true,
- ```deleteButtonId```: the id of the custom delete button, if you set enableDelete to true then you should pass the id.
- ```addByButton```: enables adding the row by custom button, the default is false where when you fill all required cells, then the row will be inserted automatically.
- ```addButtonId```: the id of the custom add button, if you set addByButton to true then you should pass the id.
- ```overrideColumnDefs```: list of column definitions that override the column definition in csharp, by now the override will override the whole object, I'm working on it to make the overriding by property.
See more here about the column definition: https://www.ag-grid.com/javascript-data-grid/column-definitions/

In your class, you can configure the properties using Attributes:
 - `[CellDataType("Cell Type")]`: specify the data type of a column's cell, please see: https://www.ag-grid.com/javascript-data-grid/cell-data-types/
 - `[CellEditor("Cell Editor")]`: specify the cell editor, please see: https://www.ag-grid.com/javascript-data-grid/cell-editors/
 - `[Editable(true / false)]`: specify whether the property is editable in the grid.
 - `[Field("Filed Name")]`: specify the field name for the grid column
 - `[HeaderName("Header Name")]`: specify a custom header name for the grid column.
 - `[Hide]`: specify that the column should be hidden.
 - `[Required(true / false)]`: specify whether the property is required, the default is true for the non-nullable value type, and false otherwise.
 - `[RowSelection]`: specify that the column is selectable, this attribute will add these properties behind the sense:
   ```js
   headerCheckboxSelection: true,
   checkboxSelection: true,
   showDisabledCheckboxes: true
   ```
   See more: https://www.ag-grid.com/javascript-data-grid/row-selection/

Example usage:

```csharp
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
}
```

## Advance

### Custom Columns Provider

If you don't like the attribute approach you can define your custom `IColumnsProvider`:
```csharp
public class CustomColumnProvider : IColumnsProvider { // .... }
```
and you can pass it to Gridazor Html Helper:
```csharp
@Html.GridEditorFor(x => x.Products, "myGrid", "ag-theme-quartz", myCustomColumnProvide)
```
My advice here is: inject your custom provider as Singlton.


### Select cell:
Aggrid provides two select editors: 
- 'agSelectCellEditor': https://www.ag-grid.com/angular-data-grid/provided-cell-editors-select/ (Free)
- 'agRichSelectCellEditor': https://www.ag-grid.com/angular-data-grid/provided-cell-editors-rich-select/ (Paid)

the issue with 'agSelectCellEditor' is very limited, so if you don't need to go with paid option, I added a custom Dropdown, and you can find it inside the zip file.
To use it you should GridazorDropdown.css and GridazorDropdown.js:
```csharp
<link rel="stylesheet" href="~/lib/Gridazor/GridazorDropdown/GridazorDropdown.css" />
<script src="~/lib/Gridazor/GridazorDropdown/GridazorDropdown.js"></script>
```

Add the property in your class:
```csharp
public int CatId { get; set; }
```

and you must configure it from javascript only:
```js
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
```

### File cell
Gridazor offers basic file input so you can upload and download the files.
To use it you should define a property that implements the `IFileInput` interface:
```csharp
public class FileInput : IFileInput
{
    public IFormFile? File { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
}

public class Product
{
   // ..

   [Required(true)]
   public FileInput? Image { get; set; }
}
```

In js part:
```js
overrideColumnDefs: [
    ........
    ........
    {
        field: "image",
        cellEditor: GridazorFileInput,
        cellRenderer: (params) => gridazorFileInputHelper.cellRender(params)
    }
],
```