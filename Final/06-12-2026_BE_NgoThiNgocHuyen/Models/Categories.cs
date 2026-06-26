using System.ComponentModel.DataAnnotations;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models
{
    public class Categories
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Fruits> Fruits { get; set; }
            = new List<Fruits>();
    }
}