using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AzureStorage;
using AzureStorage.Models;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace MvcWebApp.Controllers
{
    public class TableStorageController : Controller
    {
        private readonly INoSqlStorage<Product> _sqlStorage;
        private readonly ILogger<HomeController> _logger;

        public TableStorageController(ILogger<HomeController> logger,
        INoSqlStorage<Product> sqlStorage)
        {
            _sqlStorage = sqlStorage;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.products = _sqlStorage.All().ToList();
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.RowKey = Guid.NewGuid().ToString();
            product.PartitionKey = "Kalemler";
            await _sqlStorage.Add(product);
            return RedirectToAction("Index");
        }

    }
}
