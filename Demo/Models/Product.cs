using Gridazor.Attributes;
using Gridazor.Models;

namespace Gridazor.Demo.Models;

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
