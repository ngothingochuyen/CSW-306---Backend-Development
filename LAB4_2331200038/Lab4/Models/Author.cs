using System.ComponentModel.DataAnnotations;
namespace Lab4.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress] 
        public string Email { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public string? AvatarUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

}
