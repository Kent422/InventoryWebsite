using InventoryWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace InventoryWebsite.Controllers.Products
{
    public class LocationController : Controller
    {

        private readonly ISDBContext _context;

        public LocationController(ISDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ViewLocation()
        {
            var locations = await _context.Location.ToListAsync();

            // Sort based on parts of LocationID: Letter, Row, Column
            var sortedLocations = locations.OrderBy(l =>
            {
                var parts = l.LocationID.Split('-');
                return (
                    parts[0],                // Letter
                    int.Parse(parts[1]),     // Row
                    int.Parse(parts[2])      // Column
                );
            }).ToList();

            return View(sortedLocations);
        }
        
        public IActionResult AddLocation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddLocation(Location location)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                for (char letter = 'A'; letter <= 'K'; letter++) // Iterate through A-K
                {
                    for (int i = 1; i <= 5; i++) // Iterate 1-5 for row
                    {
                        for (int j = 1; j <= 10; j++) // Iterate 1-10 for column
                        {
                            var newLocation = new Location
                            {
                                LocationID = $"{letter}-{i}-{j}", // Create unique LocationID (e.g., A-1-1)
                                ProductID = null, // or set a default product ID
                                Availability = "Available", // Default Availability status
                                Capacity = null // or set to a default capacity if needed
                            };

                            // Add new location entity to the context
                            _context.Location.Add(newLocation); // Add 'newLocation' instead of 'location'
                        }
                    }
                }

                await _context.SaveChangesAsync(); // Save all changes in one go
                await transaction.CommitAsync(); // Commit the transaction

                TempData["Success"] = "Locations added successfully!";
                return RedirectToAction("ViewLocation"); // Redirect after success
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback transaction in case of error
                TempData["Error"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("AddLocation"); // Redirect back to AddLocation in case of error
            }
        }



    }
}


