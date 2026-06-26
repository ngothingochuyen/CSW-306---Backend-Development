using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models;

[Index("Email", Name = "UQ__Users__A9D10534421E5EB7", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
