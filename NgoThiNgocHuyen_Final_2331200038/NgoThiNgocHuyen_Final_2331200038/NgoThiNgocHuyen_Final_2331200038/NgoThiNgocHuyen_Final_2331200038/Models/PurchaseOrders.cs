using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Models
{
    public class PurchaseOrders
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<PurchaseItems> PurchaseItems { get; set; } =
        new List<PurchaseItems>();
    }
}
