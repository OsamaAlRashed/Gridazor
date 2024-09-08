using Gridazor.Attributes;
using Gridazor.Models;

namespace Gridazor.Demo.Models;

public class Test
{
    [Field("id")]
    [HeaderName("Id")]
    [Editable(false)]
    [Hide]
    [Required(false)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Field("name")]
    [HeaderName("Name")]
    [Editable(true)]
    [RowSelection]
    public string Name { get; set; } = string.Empty;

    [Field("description")]
    [HeaderName("Description")]
    [Editable(true)]
    [CellEditor("agLargeTextCellEditor")]
    public string? Description { get; set; }

    public int CatId { get; set; }
}
