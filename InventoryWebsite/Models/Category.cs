using System.ComponentModel.DataAnnotations;

namespace InventoryWebsite.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }

        // Optional: navigation property
        
    }
}
