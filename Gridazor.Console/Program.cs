using Gridazor;
using Microsoft.Extensions.FileProviders;

var assembly = typeof(GridazorExtensions).Assembly;
var fileProvider = new EmbeddedFileProvider(assembly, "content");
var contents = fileProvider.GetDirectoryContents(string.Empty);

foreach (var content in contents)
{
    Console.WriteLine(content.Name); // Log the files found in the embedded wwwroot
}