using Gridazor.Attributes;
using Gridazor.Models;
using Gridazor.Statics;

namespace Gridazor.Demo.Models;

public class Product
{
    [Editable(false)]
    [Hide]
    [Required(false)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [RowSelection]
    [Required(true)]
    public string Name { get; set; } = string.Empty;

    public bool Selected { get; set; }

    public DateOnly Date { get; set; }
    public DateTime DateTime { get; set; }

    [CellEditor(CellEditor.AgLargeTextCellEditor)]
    public string? Description { get; set; }

    [HeaderName("Category")]
    public int CatId { get; set; }

    [Required(true)]
    public FileInput? Image { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public Quality Quality { get; set; }
}

public enum Quality
{
    Low,
    Medium, 
    High
}

public class FileInput : IFileInput
{
    public IFormFile? File { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
}
