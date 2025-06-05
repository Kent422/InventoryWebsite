using InventoryWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebsite.Controllers.Products
{
    public class CategoryController : Controller
    {
        private readonly ISDBContext _context;

        public CategoryController(ISDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }

        public IActionResult AddProductCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
