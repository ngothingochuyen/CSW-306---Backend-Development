using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models;

[Index("CategoriesName", Name = "UQ__Categori__C41451B631134842", IsUnique = true)]
public partial class Category
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
    public virtual ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();
}
