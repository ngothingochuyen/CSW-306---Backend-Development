using Lab5.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lab5.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [Required]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } = "Borrowed";
    }
}
