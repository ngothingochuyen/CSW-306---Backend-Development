using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lab5.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string BookCode { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public int AvailableCopies { get; set; } = 0;

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

        public string? CoverImageUrl { get; set; }
        public string? PdfUrl { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
