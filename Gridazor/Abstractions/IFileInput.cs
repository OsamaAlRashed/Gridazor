using Microsoft.AspNetCore.Http;

namespace Gridazor.Abstractions;

/// <summary>
/// Represents Gridazor File Input
/// </summary>
public interface IFileInput
{
    /// <summary>
    /// Gets or sets the form file.
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Gets or sets the name of file.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the path of file.
    /// </summary>
    public string? Path { get; set; }
}
