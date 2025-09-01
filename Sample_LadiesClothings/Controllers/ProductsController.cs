    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample_LadiesClothings.Models;

namespace Sample_LadiesClothings.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyContext _db;
        public ProductsController(MyContext db) { _db = db; }

        //public IActionResult Index(string? q, int? categoryId)
        //{
        //    var query = _db.tbl_products.AsQueryable();
        //    if (!string.IsNullOrWhiteSpace(q))
        //        query = query.Where(p => p.Product_Name.Contains(q));
        //    if (categoryId.HasValue)
        //        query = query.Where(p => p.Category_Id == categoryId.Value);

        //    var products =  query.OrderBy(p => p.Product_Name).ToList();
        //    ViewBag.Categories =  _db.tbl_category.OrderBy(c => c.Category_Name).ToList();
        //    ViewBag.Search = q;
        //    ViewBag.CategoryId = categoryId;
        //    return View(products);
        //}
        public IActionResult Index(string q, int? categoryId)
        {
            var products = _db.tbl_products.AsQueryable();

            if (!string.IsNullOrEmpty(q))
                products = products.Where(p => p.Product_Name.Contains(q));

            if (categoryId.HasValue)
                products = products.Where(p => p.Category_Id == categoryId);

            // ✅ Get Featured Products (max 5)
            var featuredProducts = _db.tbl_products
                                           .Where(p => p.IsFeatured)
                                           .Take(5)
                                           .ToList();

            ViewBag.FeaturedProducts = featuredProducts;

            // Pass categories for dropdown
            ViewBag.Categories = _db.tbl_category.ToList();

            return View(products.ToList());
        }


        public IActionResult Details(int id)
        {
            var p =  _db.tbl_products.FirstOrDefault(p => p.Product_Id == id);
            if (p == null) return NotFound();
            var category =  _db.tbl_category.Find(p.Category_Id);
            ViewBag.CategoryName = category?.Category_Name;
            return View(p);
        }
    }
}
