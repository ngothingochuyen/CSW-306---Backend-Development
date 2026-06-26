using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace DATENOW18062026_BE_NgoThiNgocHuyen.Models;

public class User
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
    public virtual ICollection<PurchaseOrders> Orders { get; set; } = new
    List<PurchaseOrders>();
}
public enum Role
{
    Admin,
    Customer
}