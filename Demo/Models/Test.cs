using Gridazor.Attributes;

namespace Demo.Models
{
    public class Test
    {
        [Field("id")]
        [HeaderName("Id")]
        [Editable(false)]
        [Hide]
        [Required(false)]
        public int Id { get; set; }

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
    }
}
