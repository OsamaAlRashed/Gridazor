using Gridazor.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gridazor.Demo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly static List<Product> _products = 
    [
        new Product() 
        {
            Id = Guid.NewGuid(),
            Name = "Name1",
            Description = "Description1",
            CatId = 2,
            Image = new FileInput()
            {
                Name = "Image 1",
                Path = "/images/dark.png"
            }
        },
        new Product() 
        {
            Id = Guid.NewGuid(),
            Name = "Name2",
            Description = "Description2",
            CatId = 1,
            Image = new FileInput()
            {
                Name = "Image 2",
                Path = "/images/dark.png"
            }
        }
    ];

    public HomeController(ILogger<HomeController> logger) => _logger = logger;

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Cats = new List<SelectDto>()
        {
            new(1, "Category 1"),
            new(2, "Category 2")
        };

        return View(new IndexVM()
        {
            Products = _products
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(IndexVM indexVM)
    {
        _products.Clear();
        foreach (var item in indexVM.Products)
        {
            await UploadFile(item);

            _products.Add(item);
        }

        return RedirectToAction(nameof(Index));
    }

    private static async Task UploadFile(Product item)
    {
        if (item.Image?.File != null && item.Image.File.Length > 0)
        {
            var fileName = Path.GetFileName(item.Image.File.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await item.Image.File.CopyToAsync(stream);
            }

            item.Image.Path = "images/" + fileName;
            item.Image.Name = fileName;
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
