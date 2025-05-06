using FakeDataStore.Shared;
using FakeDataStore.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ZeroConigurationGrid.Controllers;
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(DataStore<Category>.Get());
    }

    [HttpPost]
    public IActionResult Index(List<Category> categories)
    {
        DataStore<Category>.Update(categories);

        return RedirectToAction(nameof(Index));
    }
}
