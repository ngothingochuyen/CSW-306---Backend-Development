using System.ComponentModel.DataAnnotations;

namespace testFinal.DTOs
{
    // DTO nhận từ client khi tạo đơn hàng
    public class CreateOrderDTO
    {
        [Required]
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }
}