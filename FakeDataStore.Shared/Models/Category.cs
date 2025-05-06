using Gridazor.Attributes;

namespace FakeDataStore.Shared.Models;

public class Category
{
    [Editable(false)]
    [Hide]
    [Required(false)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [RowSelection]
    [Required(true)]
    public string Name { get; set; } = string.Empty;

    [CellEditor("agLargeTextCellEditor")]
    public string? Description { get; set; }
}
