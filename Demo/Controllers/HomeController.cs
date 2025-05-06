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
            Name = "Name 1",
            Description = "Description 1",
            CatId = 2,
            SubCatId = 3,
            Image = new FileInput()
            {
                Name = "Image 1",
                Path = "/images/dark.png"
            },
            Date = DateOnly.MaxValue,
            Selected = false,
            Price = 100.23,
            Quantity = 5,
            Quality = Quality.High,
            DateTime = DateTime.Now
        },
        new Product() 
        {
            Id = Guid.NewGuid(),
            Name = "Name 2",
            Description = "Description 2",
            CatId = 1,
            SubCatId = 2,
            Image = new FileInput()
            {
                Name = "Image 2",
                Path = "/images/dark.png"
            },
            Date = DateOnly.MaxValue,
            Selected = false,
            Price = 10.11,
            Quantity = 1,
            Quality = Quality.Low,
            DateTime = DateTime.Now
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

        var subCats = new List<SubCategory>()
        {
            new(1, "Sub Cat 1-1", 1),
            new(2, "Sub Cat 1-2", 1),
            new(3, "Sub Cat 2-1", 2),
            new(4, "Sub Cat 2-2", 2),
        };

        ViewBag.SubCategories = subCats;

        ViewBag.QualityList = Enum.GetValues<Quality>()
            .Select(x => new SelectDto((int)x, x.ToString()))
            .ToList();

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

    [HttpGet]
    public IActionResult GetCategories(string search)
    {
        var cats = new List<SelectDto>()
        {
            new(1, "Category 1"),
            new(2, "Category 2"),
        };

        return Json(cats.Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search)).Select(x =>
            new {
                text = x.Name,
                value = x.Id
            }));
    }

    [HttpGet]
    public IActionResult GetSubCategories(int categoryId, string search)
    {
        var subCats = new List<SubCategory>()
        {
            new(1, "Sub Cat 1-1", 1),
            new(2, "Sub Cat 1-2", 1),
            new(3, "Sub Cat 2-1", 2),
            new(4, "Sub Cat 2-2", 2),
        };

        return Json(subCats
            .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search))
            .Where(x => x.CatId == categoryId)
            .Select(x =>
                new {
                    text = x.Name,
                    value = x.Id
                }));
    }

    record SubCategory(int Id, string Name, int CatId);

    private static async Task UploadFile(Product item)
    {
        if (item.Image?.File == null || item.Image.File.Length == 0)
        {
            return;
        }

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
