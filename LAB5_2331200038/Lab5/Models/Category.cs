using System.ComponentModel.DataAnnotations;
namespace Lab5.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
