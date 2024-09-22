using Microsoft.AspNetCore.Http;

namespace Gridazor.Models;


/// <summary>
/// 
/// </summary>
public interface IFileInput
{
    /// <summary>
    /// 
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Path { get; set; }
}
