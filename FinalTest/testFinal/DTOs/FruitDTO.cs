using System.ComponentModel.DataAnnotations;

namespace testFinal.DTOs
{
    public class FruitDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public double? StockQuantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}