using Microsoft.AspNetCore.Mvc;
using MyNamespace;

namespace netgo_ui_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;

        public HomeController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var client = new Client("localhost:8081/api", httpClient);
            var products = await client.ProductsGETAsync(title: string.Empty, page: 1, pageSize: 20);
            return View();
        }
    }
}
