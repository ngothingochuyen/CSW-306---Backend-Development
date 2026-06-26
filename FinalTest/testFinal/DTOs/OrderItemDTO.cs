using System.ComponentModel.DataAnnotations;

namespace testFinal.DTOs
{


    // DTO cho từng dòng trong đơn hàng
    public class OrderItemDTO
    {
        [Required]
        public int FruitId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity phải lớn hơn 0.")]
        public int Quantity { get; set; }
    }
}