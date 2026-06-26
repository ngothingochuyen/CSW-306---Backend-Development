using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Models
{
    public class PetCategory
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string CategoriesName { get; set; } = null!;
        [StringLength(255)]
        public string? Descriptions { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<Pets> Pets { get; set; } = new
        List<Pets>();

    }
}
