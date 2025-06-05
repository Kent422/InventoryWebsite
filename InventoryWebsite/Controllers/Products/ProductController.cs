using InventoryWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace InventoryWebsite.Controllers.Products
{
    public class ProductController : Controller
    {
        private readonly ISDBContext _context;

        public ProductController(ISDBContext context)
        {
            _context = context;
        }

        // GET: Product/ViewProduct
        [HttpGet]
        public async Task<IActionResult> ViewProduct()
        {
            var products = await _context.Product.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // GET: Product/AddProduct
        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.CategoryList = _context.Category
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),           // This is what gets submitted
                    Text = $"{c.CategoryID} - {c.CategoryName}" // This is what the user sees
                }).ToList();

            return View();
        }

        // POST: Product/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product, string selectedCategoryName)
        {
            var currentYear = DateTime.Now.Year.ToString();

            // Get CategoryID based on the selected CategoryName
            if (!string.IsNullOrEmpty(selectedCategoryName))
            {
                var category = await _context.Category
                    .FirstOrDefaultAsync(c => c.CategoryName == selectedCategoryName);

                if (category != null)
                {
                    product.CategoryID = category.CategoryID;
                }
                else
                {
                    ModelState.AddModelError("CategoryID", "Selected Category does not exist.");
                }
            }

            // Ensure that the ProductName field is not empty before generating ProductID
            if (!string.IsNullOrWhiteSpace(product.ProductName))
            {
                var firstLetter = product.ProductName.Trim()[0].ToString().ToUpper();

                var lastProduct = await _context.Product
                    .Where(p => p.ProductID.StartsWith(firstLetter) && p.ProductID.EndsWith(currentYear))
                    .OrderByDescending(p => p.ProductID)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;
                if (lastProduct != null)
                {
                    var numberPart = lastProduct.ProductID.Substring(1, 3);
                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                product.ProductID = $"{firstLetter}{nextNumber:D3}-{currentYear}";
            }

            // Clear and re-validate model
            ModelState.Clear();
            TryValidateModel(product);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ViewProduct");
                }
                catch (Exception ex)
                {
                    TempData["DbError"] = $"Database error: {ex.Message}";
                }
            }
            else
            {
                // Get detailed validation errors
                var validationErrors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => $"{e.Key}: {string.Join(", ", e.Value.Errors.Select(err => err.ErrorMessage))}");

                TempData["ValidationErrors"] = string.Join("\n", validationErrors);
            }

            // Reload category dropdown
            ViewBag.CategoryList = _context.Category
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = $"{c.CategoryID} - {c.CategoryName}"
                }).ToList();

            return View(product);
        }


    }
}
