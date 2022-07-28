using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
