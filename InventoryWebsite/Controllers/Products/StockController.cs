using InventoryWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebsite.Controllers.Products
{
    public class StockController : Controller
    {
        private readonly ISDBContext _context;

        public StockController(ISDBContext context)
        {
            _context = context;
        }

        // GET: Stock/ViewStock
        [HttpGet]
        public async Task<IActionResult> ViewStock()
        {
            var stocks = await _context.Stock
        .Include(s => s.Product) // Assuming Stock has a navigation property to Product
        .ToListAsync();

            return View(stocks);
        }

        // GET: Stock/AddProductStock
        [HttpGet]
        public async Task<IActionResult> AddProductStock()
        {
            var model = new StockLocationViewModel
            {
                LocationList = await _context.Location
                    .Where(l => l.Availability == "Available")
                    .ToListAsync()
            };

            ViewBag.ProductList = await _context.Product
                .Select(p => new SelectListItem
                {
                    Value = p.ProductID.ToString(),
                    Text = $"{p.ProductID} - {p.ProductName}"
                }).ToListAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddProductStock(StockLocationViewModel model)
        {
            // 1. Check if the model is valid
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns in case of validation errors
                model.LocationList = await _context.Location
                    .Where(l => l.Availability == "Available")
                    .ToListAsync();

                ViewBag.ProductList = await _context.Product
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProductID.ToString(),
                        Text = $"{p.ProductID} - {p.ProductName}"
                    }).ToListAsync();

                // Return to the same view with validation errors
                return View(model);
            }

            // 2. Insert the Stock record first
            model.Stock.DateRecieved = DateTime.Now;  // Set the received date to current date
            _context.Stock.Add(model.Stock);
            await _context.SaveChangesAsync();  // Save Stock first to generate StockID

            // 3. Update the selected Location record
            var location = await _context.Location.FindAsync(model.LocationID);
            if (location != null && location.Availability == "Available")
            {
                // 3a. Update the ProductID and set the Availability to "Occupied"
                location.ProductID = model.Stock.ProductID;
                location.Availability = "Occupied";

                // 3b. Add the Stock Quantity to the Location Capacity
                location.Capacity += model.Stock.Quantity;

                // Save changes to Location table
                _context.Location.Update(location);
                await _context.SaveChangesAsync();
            }
            else
            {
                // If Location is not found or already occupied
                TempData["DbError"] = "Location not found or already occupied.";
                return RedirectToAction("AddProductStock");
            }

            // 4. Redirect to the ViewStock page after successful insertion and update
            return RedirectToAction("ViewStock");
        }


    }
}
