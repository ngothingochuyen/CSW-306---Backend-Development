using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using testFinal.Models;

namespace testFinal.Models;

[Index("FruitsName", Name = "UQ__Fruits__126CAD46FD6E94A1", IsUnique = true)]
public partial class Fruit
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string FruitsName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public double? StockQuantity { get; set; }

    public int CategoryId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }= DateTime.Now;

    [ForeignKey("CategoryId")]
    [InverseProperty("Fruits")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Fruit")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}