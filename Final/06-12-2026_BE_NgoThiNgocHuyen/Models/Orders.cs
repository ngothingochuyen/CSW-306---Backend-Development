using System.ComponentModel.DataAnnotations;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public Users? User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [CustomValidation(
            typeof(Orders),
            nameof(ValidateOrderDate))]
        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        [RegularExpression(
            "Pending|Completed|Cancelled",
            ErrorMessage = "Status must be Pending, Completed or Cancelled"
        )]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<OrderItems> OrderItems { get; set; }
            = new List<OrderItems>();

        public static ValidationResult? ValidateOrderDate(
            DateTime orderDate,
            ValidationContext context)
        {
            if (orderDate > DateTime.Now)
            {
                return new ValidationResult(
                    "Order date cannot be in the future."
                );
            }

            return ValidationResult.Success;
        }
    }
}