using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models;

[Index("FruitsName", Name = "UQ__Fruits__126CAD46FD6E94A1", IsUnique = true)]
public partial class Fruit
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FruitsName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public double? StockQuantity { get; set; }

    public int CategoryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Fruits")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Fruit")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
