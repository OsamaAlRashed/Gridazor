using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly static List<Test> _tests = [
                    new Test() {
                        Description = "Description1",
                        Id = 1,
                        Name = "Name1",
                    },
                    new Test() {
                        Description = "Description2",
                        Id = 2,
                        Name = "Name2",
                    }
                ];

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
}
