using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Models
{
    public class Pets
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string PetsName { get; set; } = null!;
        [StringLength(100)]
        public string Breed { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int Age { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public double? StockQuantity { get; set; }
        public int CategoryId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("CategoryId")]
        [InverseProperty("Pets")]
        public virtual PetCategory PetCategory { get; set; } = null!;
        [InverseProperty("Pet")]
        public virtual ICollection<PurchaseItems> PurchaseItems { get; set; } = new
        List<PurchaseItems>();

    }
}
