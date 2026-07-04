using System.ComponentModel.DataAnnotations;

namespace MyDpprProject.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Rol zorunludur.")]
        public int RoleId { get; set; }

        public string? RoleName { get; set; }
    }
}