using Gridazor.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Gridazor.Demo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly static List<Test> _tests = [
                new Test() {
                    Description = "Description1",
                    Id = Guid.NewGuid(),
                    Name = "Name1",
                    CatId = 2
                },
                new Test() {
                    Description = "Description2",
                    Id = Guid.NewGuid(),
                    Name = "Name2",
                    CatId = 1
                }
            ];

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.Cats = new List<SelectDto>()
        {
            new(1, "Value1"),
            new(2, "Value2")
        };

        return View(new IndexVM()
        {
            Tests = _tests
        });
    }

    [HttpPost]
    public IActionResult Index(IndexVM indexVM)
    {
        _tests.Clear();
        foreach (var item in indexVM.Tests)
        {
            _tests.Add(item);
        }

        return RedirectToAction("Index");
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
