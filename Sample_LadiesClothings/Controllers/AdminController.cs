using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sample_LadiesClothings.Migrations;
using Sample_LadiesClothings.Models;
using System.Drawing.Printing;

namespace Sample_LadiesClothings.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(MyContext myContext,IWebHostEnvironment webHostEnvironment)
        {
            _context = myContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
          var admin_session=  HttpContext.Session.GetString("Admin_Id");
            if(admin_session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
           
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin_Id");
            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult Login(string Admin_Email,string Admin_Password)
        {
            var admin_Data= _context.tbl_admin.Where(a => a.Admin_Email == Admin_Email && a.Admin_Password==Admin_Password).FirstOrDefault();
            if(admin_Data != null )
            {
                HttpContext.Session.SetString("Admin_Id", admin_Data.Admin_Id.ToString() ?? "");
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.message = "Invalid Email or Password";
                return View();
            }
               
        }

        public IActionResult Profile()
        {
           var Admin_Id= HttpContext.Session.GetString("Admin_Id");
           var row= _context.tbl_admin.Where(a=> a.Admin_Id==Convert.ToInt64(Admin_Id)).ToList();
            return View(row);
        }
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
           _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        [HttpPost]
        public IActionResult Change_Profile_Picture(IFormFile Admin_Image,Admin admin)
        {
            
            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin_Images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Generate a unique filename using original name + datetime
            string fileExtension = Path.GetExtension(Admin_Image.FileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(Admin_Image.FileName);
            string uniqueFileName = $"{fileNameWithoutExt}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
            string imagePath = Path.Combine(directoryPath, uniqueFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                Admin_Image.CopyTo(stream);
                admin.Admin_Image = uniqueFileName; // Save unique name in DB
            }

            _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }

        //public IActionResult FetchCustomers()
        //{
        //    return View(_context.tbl_customer.ToList());
        //}
        public IActionResult CustomerDetail(int Id)
        {
            
            return View(_context.tbl_customer.Where(a => a.Customer_Id == Id).FirstOrDefault());
        }
        public IActionResult UpdateCustomer(int Id)
        {

            return View(_context.tbl_customer.Where(a => a.Customer_Id == Id).FirstOrDefault());
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer,IFormFile Customer_Image)
        {
            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Customer_Images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (Customer_Image != null)
            {
                string fileExtension = Path.GetExtension(Customer_Image.FileName);
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(Customer_Image.FileName);
                string uniqueFileName = $"{fileNameWithoutExt}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
                string imagePath = Path.Combine(directoryPath, uniqueFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Customer_Image.CopyTo(stream);
                    customer.Customer_Image = uniqueFileName; // Save unique name in DB
                }
            }

            _context.tbl_customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("FetchCustomers");

        }


        public IActionResult DeleteCustomer(int Id)
        {
            var customer = _context.tbl_customer.Where(a => a.Customer_Id == Id).FirstOrDefault();
            if (customer != null)
            {
                _context.tbl_customer.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("FetchCustomers");
       
        }

      
        public IActionResult DeletPerssion(int Id)
        {
          
            return View(_context.tbl_customer.Where(a => a.Customer_Id == Id).FirstOrDefault());
        }

        public IActionResult FetchCategory()
        {
           
            return View(_context.tbl_category.ToList());
        }
        public IActionResult AddCategory()
        {
            return View();

        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _context.tbl_category.Add(category);
            _context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }

        public IActionResult UpdateCategory(int id)
        {
          var category=  _context.tbl_category.Where(c => c.Category_Id == id).FirstOrDefault();
            return View(category);

        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            _context.tbl_category.Update(category);
            _context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }

        public IActionResult DeletePermissionCategory(int id)
        {
            var category = _context.tbl_category.Where(c => c.Category_Id == id).FirstOrDefault();
            return View(category);

        }
        [HttpPost]
        public IActionResult DeleteCategory(Category category)
        {
            _context.tbl_category.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }

        public IActionResult DeleteCategory(int Id)
        {
            var category = _context.tbl_category.Where(a => a.Category_Id == Id).FirstOrDefault();
            if (category != null)
            {
                _context.tbl_category.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("FetchCategory");

        }

        //public IActionResult FetchProducts()
        //{
        //    // var products = _context.tbl_products.Include(p => p.category).ToList();
        //    return View(_context.tbl_products.ToList());
        //}

        [HttpGet]
        public IActionResult FetchProducts(int page = 1, int pageSize = 10, string search = "")
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.tbl_products.AsQueryable();

            // 🔍 Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Product_Name.Contains(search) ||
                    p.Product_Price.Contains(search)); // Removed .ToString()
            }

            var totalRecords = query.Count();

            var products = query
                .OrderByDescending(p => p.Product_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Search = search
            };

            return View(model);
        }


        public IActionResult FetchCustomers(int page = 1)
        {
            // Example: fetch all customers from DB
            var customers = _context.tbl_customer.OrderBy(c => c.Customer_Id).ToList();

            // Total customers count
            int totalRecords = customers.Count;
            int pageSize = 10;

            // Total pages
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Apply Skip + Take for pagination
            var pagedCustomers = customers
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            // Pass pagination info to view
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(pagedCustomers);
        }


        public IActionResult AddProduct()
        {        
            List<Category> categories = _context.tbl_category.ToList();
            ViewData["Categories"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product product, List<IFormFile> Product_Image)
        {
            //var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Product_Images");
            //if (!Directory.Exists(directoryPath))
            //{
            //    Directory.CreateDirectory(directoryPath);
            //}
            //if (Product_Image != null)
            //{
            //    var fileExtension = Path.GetExtension(Product_Image.FileName);
            //    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(Product_Image.FileName);
            //    var uniqueFileName = $"{fileNameWithoutExt}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
            //    var imagePath = Path.Combine(directoryPath, uniqueFileName);
            //    using (var stream = new FileStream(imagePath, FileMode.Create))
            //    {
            //        Product_Image.CopyTo(stream);
            //        product.Product_Image = uniqueFileName; // Save unique name in DB
            //    }
            //}
            //_context.tbl_products.Add(product);
            //_context.SaveChanges();
            var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Product_Images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            List<string> fileNames = new List<string>();

            if (Product_Image != null && Product_Image.Count > 0)
            {
                foreach (var image in Product_Image)
                {
                    if (image.Length > 0)
                    {
                        var fileExtension = Path.GetExtension(image.FileName);
                        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(image.FileName);
                        var uniqueFileName = $"{fileNameWithoutExt}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";

                        var imagePath = Path.Combine(directoryPath, uniqueFileName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        fileNames.Add(uniqueFileName); // Add to list
                    }
                }

                // Store comma-separated names in DB
                product.Product_Image = string.Join(",", fileNames);
            }
            _context.tbl_products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("FetchProducts");


          
        }

      
        public IActionResult AllCustomers(int page = 1, int pageSize = 10)
        {
            // Example: fetch all customers from DB
            var customers = _context.tbl_customer.OrderBy(c => c.Customer_Id).ToList();

            // Total customers count
            int totalRecords = customers.Count;

            // Total pages
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Apply Skip + Take for pagination
            var pagedCustomers = customers
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            // Pass pagination info to view
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return RedirectToAction("FetchCustomers",pagedCustomers);
        }



        public IActionResult ProductDetail(int Id)
        {


            return View(_context.tbl_products.Include(p => p.category).FirstOrDefault(p => p.Product_Id == Id));
        }
        public IActionResult DeletePermissionProduct(int id)
        {
            var product = _context.tbl_products.Where(c => c.Product_Id == id).FirstOrDefault();
            return View(product);

        }
       
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.tbl_products.Where(c => c.Product_Id == id).FirstOrDefault();
            if (product != null)
            {
                _context.tbl_products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("FetchProducts");
        }

        public IActionResult UpdateProduct(int id)
        {
            List<Category> categories = _context.tbl_category.ToList();
            ViewData["Categories"] = categories;
            
            var product = _context.tbl_products.Where(c => c.Product_Id == id).FirstOrDefault();
            ViewBag.Cat_id = product.Category_Id;
            return View(product);

        }
        

        [HttpPost]
        public IActionResult Change_Product_Picture(IFormFile Product_Image, Product product)
        {

            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Product_images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Generate a unique filename using original name + datetime
            string fileExtension = Path.GetExtension(Product_Image.FileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(Product_Image.FileName);
            string uniqueFileName = $"{fileNameWithoutExt}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
            string imagePath = Path.Combine(directoryPath, uniqueFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                Product_Image.CopyTo(stream);
                product.Product_Image = uniqueFileName; // Save unique name in DB
            }

            _context.tbl_products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("FetchProducts");
        }


        //[HttpGet]
        //public IActionResult ManageFeatured()
        //{
        //    var products = _context.tbl_products
        //                           .OrderByDescending(p => p.Product_Id)
        //                           .ToList();

        //    return View(products);
        //}
        [HttpGet]
        public IActionResult ManageFeatured(int page = 1, int pageSize = 10, string search = "")
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.tbl_products.AsQueryable();

            // 🔍 server-side search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Product_Name.Contains(search) ||
                    p.Product_Price.Contains(search)); // Removed .ToString()
            }

            var totalRecords = query.Count();

            var products = query
                .OrderByDescending(p => p.Product_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Search = search
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult UpdateFeatured(List<int> featuredIds)
        {
            var products = _context.tbl_products.ToList();

            foreach (var product in products)
            {
                product.IsFeatured = featuredIds.Contains(product.Product_Id);
            }

            _context.SaveChanges();

            TempData["Message"] = "Featured products updated successfully!";
            return RedirectToAction("ManageFeatured");
        }

        // Replace this block:
        //public IActionResult ManageFeatured(int page = 1, int pageSize = 10, string search = "")
        //{
        //    using (var db = new MyContext())
        //    {
        //        var query = db.tbl_products.AsQueryable();

        //        // ✅ Search filter
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            query = query.Where(p =>
        //                p.Product_Name.Contains(search) ||
        //                p.Product_Price.ToString().Contains(search));
        //        }

        //        // ✅ Pagination
        //        int totalRecords = query.Count();
        //        var products = query
        //            .OrderBy(p => p.Product_Name)
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize)
        //            .ToList();

        //        ViewBag.CurrentPage = page;
        //        ViewBag.PageSize = pageSize;
        //        ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        //        ViewBag.Search = search;

        //        return View(products);
        //    }
        //}

        // With this:
       


    }
}
