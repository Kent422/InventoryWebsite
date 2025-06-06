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

        // GET: /Category/Edit/5
        public async Task<IActionResult> EditCategory(int categoryID)
        {
            var category = await _context.Category.FindAsync(categoryID);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: /Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int categoryID, Category category)
        {
            if (categoryID != category.CategoryID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index"); // or your list view
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Category.Any(c => c.CategoryID == categoryID))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(category);
        }


        // GET: /Category/Delete/5
        public async Task<IActionResult> DeleteCategory(int CategoryID)
        {
            var category = await _context.Category.FindAsync(CategoryID);
            if (category == null)
                return NotFound();

            return View(category); // Shows confirmation page
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int categoryID)
        {
            var category = await _context.Category.FindAsync(categoryID);
            if (category == null)
                return NotFound();

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
