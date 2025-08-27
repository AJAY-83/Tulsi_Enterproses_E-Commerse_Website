using Microsoft.AspNetCore.Mvc;

namespace Sample_LadiesClothings.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
