using System.ComponentModel.DataAnnotations;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models
{
    public class OrderItems
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Orders Order { get; set; }

        public int FruitId { get; set; }

        public Fruits Fruit { get; set; }

        [Required]
        [Range(1, int.MaxValue,
            ErrorMessage = "Quantity must be positive")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "UnitPrice must be positive")]
        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
