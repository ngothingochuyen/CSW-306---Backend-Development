using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace testFinal.Models;

[Index("Email", Name = "UQ__Users__A9D10534421E5EB7", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public Role Role { get; set; } 

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
public enum Role
{
    Admin,
    User
}