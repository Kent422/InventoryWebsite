using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding; // For BindNever
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // For ValidateNever

namespace InventoryWebsite.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        public int CategoryID { get; set; }  // FK to Category

        public Category? Category { get; set; } // navigation property


        //public ICollection<Stock> Stocks { get; set; } // optional reverse nav
    }
}
