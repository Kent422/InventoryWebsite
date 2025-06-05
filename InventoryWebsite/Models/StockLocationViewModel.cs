namespace InventoryWebsite.Models
{
    public class StockLocationViewModel
    {
        public Stock Stock { get; set; }
        public string LocationID { get; set; }  // Selected LocationID
        public IEnumerable<Location> LocationList { get; set; }
    }
}
