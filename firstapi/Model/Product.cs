using System.ComponentModel.DataAnnotations;

namespace firstapi.Model
{
    public class Product
    {
        [Key]
        public int pId { get; set; }

        public string ProductName { get; set; }
        [Required]
        public string ProductPrice { get; set; }
    }
}
