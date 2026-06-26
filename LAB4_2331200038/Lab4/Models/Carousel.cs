using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Carousel
    {
        [Key]
        public int CarouselId { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh là bắt buộc")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? LinkUrl { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}