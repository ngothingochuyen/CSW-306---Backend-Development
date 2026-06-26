using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Models
{
    public class PurchaseItems
    {
        [Key]
        public int Id { get; set; }
        public int PurchaseOrdersId{ get; set; }
        public int PetId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("PetId")]
        [InverseProperty("OrderItems")]
        public virtual Pets Pet { get; set; } = null!;
        [ForeignKey("OrderId")]
        [InverseProperty("OrderItems")]
        public virtual PurchaseOrders PurchaseOrders { get; set; } = null!;
    }
}
