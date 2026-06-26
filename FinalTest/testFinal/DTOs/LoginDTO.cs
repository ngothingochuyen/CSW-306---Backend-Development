using System.ComponentModel.DataAnnotations;

namespace testFinal.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } 
        public string Password { get; set; }
    }
}