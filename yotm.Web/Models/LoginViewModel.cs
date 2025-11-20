using System.ComponentModel.DataAnnotations;

namespace yotm.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Telefon numarası gereklidir")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class VerifyOtpViewModel
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğrulama kodu gereklidir")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Kod 6 haneli olmalıdır")]
        [Display(Name = "Doğrulama Kodu")]
        public string Code { get; set; } = string.Empty;
    }
}
