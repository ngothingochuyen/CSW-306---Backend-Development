using System.ComponentModel.DataAnnotations;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models
{
    public class Fruits
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Price must be possitive")]
        public decimal Price { get; set; }

        public float StockQuantity { get; set; }

        public int CategoryId { get; set; }

        public Categories Categories { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
