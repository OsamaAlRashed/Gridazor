using Microsoft.AspNetCore.Http;

namespace Gridazor.Models;

public interface IFileInput
{
    public IFormFile? File { get; set; }
    public string? Name { get; set; }
}
