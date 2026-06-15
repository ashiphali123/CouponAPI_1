using System.ComponentModel.DataAnnotations;

namespace CouponMVC.Models
{
    public class RegistrationRequestDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? UserEmail { get; set; }
        [Required]
        public string? UserPassWord { get; set; }
        [Required]
        public string? RoleName { get; set; }
        //public string? UserMobile { get; set; }
    }
}
