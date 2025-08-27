using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample_LadiesClothings.Models;

namespace Sample_LadiesClothings.Controllers
{
    public class CartController : Controller
    {
        private readonly MyContext _db;
        private const string CartKey = "CART_STATE";

        public CartController(MyContext db) { _db = db; }

        private List<CartItem> GetCart()
        {
            var data = HttpContext.Session.GetString(CartKey);
            if (string.IsNullOrEmpty(data)) return new List<CartItem>();
            return data.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(part =>
            {
                var cols = part.Split(",,,");
                return new CartItem
                {
                    Product_Id = int.Parse(cols[0]),
                    Product_Name = cols[1],
                    Product_Price = decimal.Parse(cols[2]),
                    Product_Image = cols[3],
                    Quantity = int.Parse(cols[4])
                };
            }).ToList();
        }
        private void SaveCart(List<CartItem> items)
        {
            var payload = string.Join("|", items.Select(i => $"{i.Product_Id},,,{i.Product_Name},,,{i.Product_Price},,,{i.Product_Image},,,{i.Quantity}"));
            HttpContext.Session.SetString(CartKey, payload);
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        public IActionResult Add(int id, int qty = 1)
        {
            var p =  _db.tbl_products.FirstOrDefault(x => x.Product_Id == id);
            if (p == null) return NotFound();
            var cart = GetCart();
            var line = cart.FirstOrDefault(i => i.Product_Id == id);
            if (line == null)            
                cart.Add(new CartItem { Product_Id = p.Product_Id, Product_Name = p.Product_Name, Product_Price = decimal.Parse(p.Product_Price), Product_Image = p.Product_Image, Quantity = qty });
            else
                line.Quantity += qty;
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.Product_Id == id);
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index");
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index");

            var order = new Order
            {
                Customer_Id = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value),
                Order_Date = DateTime.UtcNow,
                Status = "Placed",
                Total = cart.Sum(i => i.LineTotal),
                Items = cart.Select(i => new OrderItem
                {
                    Product_Id = i.Product_Id,
                    Quantity = i.Quantity,
                    Price = i.Product_Price
                }).ToList()
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            SaveCart(new List<CartItem>());
            TempData["OrderId"] = order.Order_Id;
            return RedirectToAction("Thanks");
        }

        public IActionResult Thanks() => View();
    }
}
