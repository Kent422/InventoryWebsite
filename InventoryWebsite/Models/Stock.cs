using InventoryWebsite.Models;

namespace InventoryWebsite.Models
{
    public class Stock
    {
        public int StockID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Supplier { get; set; }
        public DateTime DateRecieved { get; set; }

        //public Product Product { get; set; } // Navigation property
    }
}

    
	   
	   
	   
	   
