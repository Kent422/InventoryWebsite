namespace InventoryWebsite.Models
{
    public class Location
    {
        public string? LocationID { get; set; }
        public string? ProductID { get; set; } // Nullable string

        public string? Availability { get; set; } // Nullable string

        public int? Capacity { get; set; } // Nullable string

        public Product? Product { get; set; } // Navigation Property to Product
    }
}
