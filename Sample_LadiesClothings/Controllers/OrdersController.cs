using Microsoft.AspNetCore.Mvc;
using Sample_LadiesClothings.Models;

namespace Sample_LadiesClothings.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MyContext _db;
        public OrdersController(MyContext db) { _db = db; }

        public IActionResult My()
        {
            var cid = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var orders =  _db.Orders.Where(o => o.Customer_Id == cid)
                .OrderByDescending(o => o.Order_Date)
                .ToList();
            return View(orders);
        }
    }
}
