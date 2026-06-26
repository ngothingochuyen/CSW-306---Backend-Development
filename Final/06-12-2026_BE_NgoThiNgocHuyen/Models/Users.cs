using System.ComponentModel.DataAnnotations;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    public enum UserRole
    {
        Admin,
        User
    }

    
}
